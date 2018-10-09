// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Lab_Minigame {

	using VRTK;

	public interface IMinigameNode {

		int					AvaiableLinks { get; set; }

		int[]				LinkDirections { get; }

		/// <summary> Add a link number for a direction </summary>
		void				AddLinkToDirection( MinigameMgr.Direction d );

		/// <summary> Remove a link number for a direction </summary>
		void				RemoveLinkFromDirection( MinigameMgr.Direction d );

	}


	//////////////////////////////////////////////////////////////


	public class MinigameNode : MonoBehaviour, IMinigameNode {

		public	static MinigameNode		Selected				= null;

		private	static MinigameLink		LinkRes					= null;

		public IMinigameNode			Interface				= null;

		public const	int				MAX_N_LINKS				= 2;

		[SerializeField]
		private	int						m_AvaiableLinks			= 0;
		public int						AvaiableLinks
		{
			get { return m_AvaiableLinks; }
			set
			{
				OnAvaiableLinksModified( m_AvaiableLinks = value );
			}
		}
		[SerializeField]
		private	int						m_EditX					= 0;
		public	int						EditX
		{
			get { return m_EditX; }
		}
		[SerializeField]
		private	int						m_EditY					= 0;
		public	int						EditY
		{
			get { return m_EditY; }
		}
		[SerializeField]
		private	int[]					m_LinkDirections		= null;
		public	int[]					LinkDirections
		{
			get { return m_LinkDirections; }
		}


		private	int						m_StartAvaiableLinks	= 0;
		private	MinigameLink			m_CurrentLink			= null;
		[SerializeField]
		private MinigameMgr.Direction	m_CurrentLinkDirection;
		private	MinigameNode			m_Instance				= null;
		private	Image					m_Image					= null;

		private	Sprite					m_SpriteDefault			= null;
		private	Sprite					m_SpriteCompleted		= null;


		//////////////////////////////////////////////////////////////////////////
		// AWAKE
		private void Awake()
		{
			Interface = this as IMinigameNode;
			m_Instance = this;

			m_Image = transform.GetChild(0).GetComponent<Image>();

			// load prefab
			if ( LinkRes ==  null )
			{
				LinkRes = Resources.Load<MinigameLink>( "Lab_Minigame/MinigameLink" );
				LinkRes.transform.position = Vector3.up * 999f;
			}

			m_LinkDirections = new int[ 4 ];
		}


		//////////////////////////////////////////////////////////////////////////
		// CanBeSelected
		private	bool	CanBeSelected()
		{
			if ( this.m_AvaiableLinks < 1 )
				return false;

			return true;
		}


		//////////////////////////////////////////////////////////////////////////
		// CanCreateLink
		private	bool	CanCreateLink( ref MinigameNode target )
		{
			if ( this.m_AvaiableLinks < 1 )
			{
				print( "CanCreateLink:: " + name + " has not avaiable links" );
				return false;
			}
			if ( target.AvaiableLinks == 0 )
			{
				print( "CanCreateLink:: Target " + target.name + " has not avaiable links" );
				return false;
			}

			if ( ( m_CurrentLinkDirection = MinigameMgr.GetDirection( ref m_Instance, ref target ) ) == MinigameMgr.Direction.NONE )
			{
				print( "CanCreateLink:: " + name + " Invalid direction" );
				return false;
			}
//			print( " Current direction " + m_CurrentLinkDirection );

			if ( m_LinkDirections[ (int)m_CurrentLinkDirection ] == MAX_N_LINKS )
			{
				print( "CanCreateLink:: Node " + name + " has no links avaiable in direction " + m_CurrentLinkDirection+ "(" + (int)m_CurrentLinkDirection + ")" );
				return false;
			}

			MinigameMgr.Direction oppositeDirection = MinigameMgr.GetOppositeDirection( m_CurrentLinkDirection );
			if ( m_LinkDirections[ (int)oppositeDirection ] == MAX_N_LINKS )
			{
				print( "CanCreateLink:: Target : " + target.name + " cannot have a link in direction " + oppositeDirection + "(" + (int)oppositeDirection + ")" );
				return false;
			}

			if ( MinigameMgr.Instance.CheckForNearest( ref m_Instance, ref target, m_CurrentLinkDirection ) == false )
			{
				print( "CanCreateLink:: " + name + " No nearest node chosen" );
				return false;
			}

			return true;
		}


		//////////////////////////////////////////////////////////////////////////
		// OnPointerClick
		public void OnPointerClick()
		{
			// Skip if same node is selected
			if ( Selected == this )
			{
				// deselect this node
				m_Image.color = Color.white;
				Selected = null;
				return;
			}

			// Create link if avaiable
			if ( Selected != null )
			{
				// reset selection
				Selected.m_Image.color = m_Image.color = Color.white;

				if ( CanCreateLink ( ref Selected ) == false )
				{
					Selected = null;
					return;
				}

				// Create link
				MinigameLink.gNode1 = Selected;
				MinigameLink.gNode2 = this;
				MinigameLink link = Instantiate( LinkRes, transform.parent );
				link.LinkDirection = m_CurrentLinkDirection;

				// Save link ref into nodes
				Selected.m_CurrentLink = m_CurrentLink = link;
				Selected = null;
			}
			// Select this node
			else
			{
				if ( CanBeSelected() == false )
				{
					Selected.m_Image.color = m_Image.color = Color.white;
					Selected = null;
					return;
				}

				// make vidence of selection
				Selected = this;
				Selected.m_Image.color = Color.red;
			}
		}


		//////////////////////////////////////////////////////////////////////////
		// Setup
		public void Setup( ref MinigameLevelNodeData nodeData )
		{
			m_EditX = nodeData.editY;
			m_EditY = nodeData.editX;

			// set avaiable links
			m_StartAvaiableLinks = m_AvaiableLinks = nodeData.links;

			m_SpriteDefault		= MinigameMgr.Instance.SpritesCollection.Collection[ m_StartAvaiableLinks - 1 ];
			m_SpriteCompleted	= MinigameMgr.Instance.SpritesCollectionCompleted.Collection[ m_StartAvaiableLinks - 1 ];

			// Set correct sprite
			m_Image.sprite = m_SpriteDefault;
		}


		//////////////////////////////////////////////////////////////////////////
		// OnAvaiableLinksModified
		private	void	OnAvaiableLinksModified( int value )
		{
			m_Image.sprite = ( value == 0 ) ?  m_SpriteCompleted : m_SpriteDefault;
			GetComponent<Button>().interactable = ( value > 0 );
		}


		//////////////////////////////////////////////////////////////////////////
		// AddLinkToDirection
		/// <summary> Add a link number for a direction </summary>
		public void	AddLinkToDirection( MinigameMgr.Direction d )
		{
			if ( (int)d == -1 )
			{
				print( "AddLinkToDirection::Out of range" );
				return;
			}
			m_LinkDirections[ (int)d ]++;
		}


		//////////////////////////////////////////////////////////////////////////
		// RemoveLinkFromDirection
		/// <summary> Remove a link number for a direction </summary>
		public void	RemoveLinkFromDirection( MinigameMgr.Direction d )
		{
			if ( (int)d == -1 )
			{
				print( "RemoveLinkFromDirection::Out of range" );
				return;
			}
			m_LinkDirections[ (int)d ]--;
		}


		//////////////////////////////////////////////////////////////////////////
		// Equals ( overrider )
		public override bool Equals( object other )
		{
			MinigameNode otherNode = ( MinigameNode) other;
			return ( otherNode.EditX == EditX && otherNode.EditY == EditY );
		}


		//////////////////////////////////////////////////////////////////////////
		// GetHashCode ( overrider )
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}


}