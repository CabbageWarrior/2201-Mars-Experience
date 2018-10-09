// Scripted by Roberto Leogrande

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VRTK;


namespace Lab_Minigame {

	public interface IMinigameLinker {

		MinigameNode	ParentNode { set; }

		Vector3			StartPosition { get; set; }

		Quaternion		StartRotation { get; set; }

		void			ResetLinker(); 

	}


	//////////////////////////////////////////////////////////////


	public class MinigameLinker : MonoBehaviour, IMinigameLinker, IPointerEnterHandler, IPointerExitHandler/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/ {

		private	static MinigameLink		LinkRes					= null;

		private	MinigameNode			m_ParentNode			= null;
		public	MinigameNode ParentNode
		{
			get { return m_ParentNode; }
			set { m_ParentNode = value; }
		}

		
		private	Vector3					m_StartPosition			= Vector3.zero;
		public	Vector3					StartPosition
		{
			get { return m_StartPosition; }
			set { m_StartPosition = value; }
		}
		
		private Quaternion				m_StartRotation			= Quaternion.identity;
		public	Quaternion				StartRotation
		{
			get { return m_StartRotation; }
			set { m_StartRotation = value; }
		}

		private CanvasGroup				m_CanvasGroup			= null;
		private MinigameNode			m_CurrentCollidedNode	= null;
		private	MinigameLink			m_CurrentLink			= null;
		private	MinigameMgr.Direction	m_CurrentLinkDirection	= MinigameMgr.Direction.NONE;
		private	Image					m_LinkerImage			= null;
		private	Color					m_StartColor			= Color.clear;



		//////////////////////////////////////////////////////////////////////////
		// AWAKE
		private void Awake()
		{
			// load prefab
			if ( LinkRes ==  null )
			{
				LinkRes = Resources.Load<MinigameLink>( "Lab_Minigame/MinigameLink" );
				LinkRes.transform.position = Vector3.up * 999f;
			}
	     
			m_CanvasGroup	= GetComponent<CanvasGroup>();
			m_LinkerImage	= GetComponent<Image>();
			m_StartColor	= m_LinkerImage.color;
		}
		

		//////////////////////////////////////////////////////////////////////////
		// OnPointerEnter
		public void OnPointerEnter( PointerEventData eventData )
		{}


		//////////////////////////////////////////////////////////////////////////
		// OnPointerExit
		public void OnPointerExit( PointerEventData eventData )
		{}

		/*
		//////////////////////////////////////////////////////////////////////////
		// OnBeginDrag
		public void OnBeginDrag( PointerEventData eventData )
		{
			m_CanvasGroup.blocksRaycasts = false;

			OnDrag( eventData );
		}


		//////////////////////////////////////////////////////////////////////////
		// OnDrag
//		protected RectTransform dragTransform;
		public void OnDrag( PointerEventData eventData )
		{
			m_CurrentLinkDirection = GetDirection();
			
			if ( m_CurrentLink == null )
			{
				m_CurrentLink = Instantiate( LinkRes, m_ParentNode.transform.parent );
				m_CurrentLink.Node1 = m_ParentNode;

				m_CurrentLink.transform.position = transform.position;
				m_CurrentLink.transform.rotation = transform.rotation;
			}

			// LINK GRAPHICS
			m_CurrentLink.UpdateLine( transform );

			Vector3 pointerPosition;
			if ( RectTransformUtility.ScreenPointToWorldPointInRectangle( transform.parent.parent as RectTransform, eventData.position, eventData.enterEventCamera, out pointerPosition ) )
			{
				transform.position = pointerPosition;
//				transform.rotation = dragTransform.rotation;
			}

//			transform.position = eventData.pointerCurrentRaycast.worldPosition;
		}
		

		//////////////////////////////////////////////////////////////////////////
		// OnEndDrag
		public void OnEndDrag( PointerEventData eventData )
		{
			print( "OnDragEnd" );
			m_CanvasGroup.blocksRaycasts = true;

			// if no node has been hitted, reset linker
			if ( m_CurrentCollidedNode == null )
			{
				ResetLinker();
				if ( m_CurrentLink != null )
					Destroy( m_CurrentLink.gameObject );
//				print( "OnSet: m_CurrentCollidedNode = null" );
				return;
			}

			// set link direction
			m_CurrentLink.LinkDirection = m_CurrentLinkDirection;

			// set link destination node
			m_CurrentLink.Node2 = m_CurrentCollidedNode;

			// reset this linker position and state
			ResetLinker();

			// remove reference to link
			m_CurrentLink = null;
		}
		*/

		//////////////////////////////////////////////////////////////////////////
		// OnReset
		private void OnReset( object sender, UIDraggableItemEventArgs e ) {

			// Destroy the link
			if ( m_CurrentLink )
				Destroy( m_CurrentLink.gameObject );

			ResetLinker();

		}
	
		
		//////////////////////////////////////////////////////////////////////////
		// ResetLinker
		public void ResetLinker( ) {

			// reset linker position and rotation
			transform.SetPositionAndRotation( m_StartPosition, m_StartRotation );
			m_CurrentCollidedNode = null;

			transform.SetParent( m_ParentNode.transform );

			m_LinkerImage.color = m_StartColor;

		}
		
		
		//////////////////////////////////////////////////////////////////////////
		// IntersectLinksLine
		/*
		private	bool	IntersectLinksLine( Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2 )
		{
			float a = Vector3.Dot( lineVec1, lineVec1 );
			float b = Vector3.Dot( lineVec1, lineVec2 );
			float e = Vector3.Dot( lineVec2, lineVec2 );

			float d = a*e - b*b;

			print( d );

			//lines are not parallel
			if ( Mathf.Abs( d ) > 0.05f )
			{
				return true;
			}
			else
			{
				return false;
			}

		}
		
		const double coPlanerThreshold = 0.7; // Some threshold value that is application dependent
		const double lengthErrorThreshold = 1e-3;

		bool IntersectLinksLine( Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2 )
		{
			Vector3 da = linePoint1 - lineVec1;  	// Unnormalized direction of the ray
			Vector3 db = lineVec1   - linePoint2;
			Vector3 dc = linePoint2 - linePoint1;
	
			if (Mathf.Abs( Vector3.Dot( dc,  Vector3.Cross( da, db ) ) ) >= coPlanerThreshold) // Lines are not coplanar
				return false;
		
			float s = Vector3.Dot(Vector3.Cross( dc, db ), Vector3.Cross( da, db ) ) /  Vector3.Cross( da, db ).sqrMagnitude;

			if (s >= 0.0 && s <= 1.0)	// Means we have an intersection
			{
				Vector3 intersection = linePoint1 + s * da;
		
				// See if this lies on the segment
				if ( (intersection - linePoint2).sqrMagnitude + (intersection - lineVec2).sqrMagnitude <= ( linePoint2 - lineVec2 ).sqrMagnitude + lengthErrorThreshold )
					return true;
			}

			return false;
		}
		*/
		/*

		//////////////////////////////////////////////////////////////////////////
		// ProcessTrigger
		private	void	ProcessTrigger( Collider other ) {

			MinigameMgr.Direction currentLinkDirection = GetDirection();

			// invalid link direction
			if ( currentLinkDirection == MinigameMgr.Direction.NONE )
			{
				m_LinkerImage.color = Color.red;
				return;
			}

			MinigameLinker collidedMinigameLinker = other.GetComponent<MinigameLinker>();
			if ( collidedMinigameLinker == null )
				return;
			MinigameNode   collidedMinigameNode = collidedMinigameLinker.ParentNode;
			*/

			/*
			// Check for links intersections
			foreach( MinigameLink link in MinigameLink.Links )
			{
				if ( 
					( link.Node1 != m_ParentNode && link.Node2 != m_ParentNode ) &&
					( link.Node1 != collidedMinigameNode && link.Node2 != collidedMinigameNode ) 
					&& IntersectLinksLine( link.Node1.transform.position, link.Node2.transform.position, m_ParentNode.transform.position, transform.position ) )
				{
					print( "link intersection found" );
					m_LinkerImage.color = Color.red;
					return;
				}
			}
			*/
			/*
			// hitted node is the parent
			if ( collidedMinigameNode == m_ParentNode )
			{
				print( "collidedMinigameNode == m_ParentNode" );
				m_LinkerImage.color = Color.red;
				return;
			}

			if ( m_ParentNode.EditX != collidedMinigameNode.EditX && m_ParentNode.EditY != collidedMinigameNode.EditY )
			{
				print( "nodes are not on the same axis" );
				m_LinkerImage.color = Color.red;
				return;
			}

			// no avaiable links
			if ( collidedMinigameNode.AvaiableLinks == 0 )
			{
				print( "collidedMinigameNode.AvaiableLinks == 0" );
				m_LinkerImage.color = Color.red;
				return;
			}

			// no link avaiable in that direction
			if ( m_ParentNode.LinkDirections[ (int)currentLinkDirection ] == MinigameNode.MAX_N_LINKS )
			{
				print( "m_ParentNode.LinkDirection == MinigameNode.MAX_N_LINKS" );
				m_LinkerImage.color = Color.red;
				return;
			}

			// no link avaiable in opposite direction
			if ( collidedMinigameNode.LinkDirections[ (int)MinigameMgr.GetOppositeDirection( currentLinkDirection ) ] == MinigameNode.MAX_N_LINKS )
			{
				print( "collidedMinigameNode.LinkDirection == MinigameNode.MAX_N_LINKS" );
				m_LinkerImage.color = Color.red;
				return;
			}

			// this node is not consecutive
			if ( !MinigameMgr.Instance.CheckForNearest( m_ParentNode, collidedMinigameNode, currentLinkDirection ) )
			{
				print( "CheckForNearest == false" );
				m_LinkerImage.color = Color.red;
				return;
			}

			m_CurrentCollidedNode = collidedMinigameNode;

			VRTK_VRInputModule_Extension.ResetItems( PlayerCommon.Instance.RightUIPointer, null );
			VRTK_VRInputModule_Extension.ResetItems( PlayerCommon.Instance.LeftUIPointer,  null );
		}
		
		
		////////////////////////////////////////////////////////////////
		// UNITY 
		private void OnTriggerStay( Collider other )
		{

			if ( m_CurrentLink == null ) return;

			m_LinkerImage.color = Color.green;

			// no avaiable links
			if ( m_ParentNode.AvaiableLinks == 0 )
			{
				m_LinkerImage.color = m_StartColor;
//				m_LinkerImage.color = Color.red;
//				print( "Parent node has not avaiable links" );
				VRTK_VRInputModule_Extension.ResetItems( PlayerCommon.Instance.RightUIPointer, null );
				VRTK_VRInputModule_Extension.ResetItems( PlayerCommon.Instance.LeftUIPointer,  null );
				return;
			}

			ProcessTrigger( other );
		}


		private void OnTriggerExit( Collider other )
		{
			m_CurrentCollidedNode = null;
		}
		*/
	}


}