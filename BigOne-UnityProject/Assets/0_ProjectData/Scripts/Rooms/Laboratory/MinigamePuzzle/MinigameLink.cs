// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using VRTK;

namespace Lab_Minigame {

	public interface IMinigameLink {

		MinigameMgr.Direction	LinkDirection { set; }
	}


	//////////////////////////////////////////////////////////////


	public class MinigameLink : MonoBehaviour, IMinigameLink {

		public static List<MinigameLink>	Links					= null;

		public	IMinigameLink				Interface				= null;

		public	static	MinigameNode		gNode1, gNode2;

		public	bool						IsOK					= false;

		[SerializeField]
		private	Sprite						m_Level1Sprite			= null;
		[SerializeField]
		private	Sprite						m_Level2Sprite			= null;
		
		[SerializeField]
		private	int							m_LinkLevel				= 1;
		public	int							LinkLevel
		{
			get { return m_LinkLevel; }
		}

		private MinigameNode				m_Node_1				= null;
		public	MinigameNode				Node1
		{
			get { return m_Node_1; }
		}

		private MinigameNode				m_Node_2				= null;
		public	MinigameNode				Node2
		{
			get { return m_Node_2; }
		}

		private MinigameMgr.Direction		m_LinkDirection			= MinigameMgr.Direction.NONE;
		public	MinigameMgr.Direction		LinkDirection
		{
			get { return m_LinkDirection; }
			set { m_LinkDirection = value; }
		}

		private Image						m_LinkImage				= null;


		////////////////////////////////////////////////////////////////
		// SCRIPT LOGIC

		//////////////////////////////////////////////////////////////////////////
		// AWAKE
		void Awake ()
		{
			Interface = this as IMinigameLink;

			m_LinkImage = GetComponent<Image>();

			MinigameLinkChecker chcker = transform.GetChild(0).GetComponent<MinigameLinkChecker>();
			chcker.Node1 = m_Node_1 = gNode1;
			chcker.Node2 = m_Node_2 = gNode2;

			UpdateLine();

			StartCoroutine( OnNode2Set() );
        }


        //////////////////////////////////////////////////////////////////////////
        // OnNode2Set
        private IEnumerator OnNode2Set()
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();

			IsOK = true;
			m_LinkImage.color = Color.white;

			// CHECK FOR ALREADY EXISTING LINK
			// ( so update the existing one and destroy this link )
			MinigameLink link = Links.Find( l => l.Node1 == m_Node_1 && l.Node2 == m_Node_2 || l.Node1 == m_Node_2 && l.Node2 == m_Node_1 );
			if ( link != null )
			{
				if ( link.LinkLevel == MinigameNode.MAX_N_LINKS )
				{
					Destroy( gameObject );
					yield break;
				}

				link.Node1.Interface.AvaiableLinks--;
				link.Node2.Interface.AvaiableLinks--;

				// remove one link availability to nodes direction counter
				link.Node1.Interface.AddLinkToDirection( link.LinkDirection );
				link.Node2.Interface.AddLinkToDirection( MinigameMgr.GetOppositeDirection( link.LinkDirection ) );

//				link.m_LinkImage.color = Color.blue;
				link.m_LinkImage.sprite = link.m_Level2Sprite;
				link.m_LinkLevel++;

				// check for level completion
				MinigameMgr.Instance.LevelCheck();

				Destroy( gameObject );
				yield break;
			}
			else
			{
				// add this link to all links list
				Links.Add( this );
			}

			// just update the line
			UpdateLine();

			// link color
			m_LinkImage.sprite = m_Level1Sprite;

			m_Node_1.AvaiableLinks--;
			m_Node_2.AvaiableLinks--;

			// remove one link availability to nodes direction counter
			m_Node_1.Interface.AddLinkToDirection( m_LinkDirection );
			m_Node_2.Interface.AddLinkToDirection( MinigameMgr.GetOppositeDirection( m_LinkDirection ) );

			transform.GetChild(0).gameObject.SetActive( true );

			// check for level completion
			MinigameMgr.Instance.LevelCheck();
		}


		//////////////////////////////////////////////////////////////////////////
		// PrevLevel
		public	void	PrevLevel()
		{
			m_LinkLevel--;

			m_Node_1.AvaiableLinks++;
			m_Node_2.AvaiableLinks++;

			// add one link availability to nodes direction counter
			m_Node_1.Interface.RemoveLinkFromDirection( m_LinkDirection );
			m_Node_2.Interface.RemoveLinkFromDirection( MinigameMgr.GetOppositeDirection( m_LinkDirection ) );

			if ( m_LinkLevel == 0 )
			{
				Links.Remove( this );
				Destroy( gameObject );
				return;
			}

			if ( m_LinkLevel == 1 )
			{
				m_LinkImage.sprite = m_Level1Sprite;
			}

			VRTK_VRInputModule_Extension.ResetItems( PlayerCommon.Instance.RightUIPointer, null );
			VRTK_VRInputModule_Extension.ResetItems( PlayerCommon.Instance.LeftUIPointer,  null );

		}
		

		//////////////////////////////////////////////////////////////////////////
		// UpdateLine
		private	void UpdateLine()
		{
			// position
			transform.position = ( m_Node_1.transform.position + m_Node_2.transform.position ) / 2f;

			// rotation
			Vector3 direction =  ( m_Node_1.transform.position - m_Node_2.transform.position );
			transform.rotation = Quaternion.LookRotation( m_Node_1.transform.forward, direction );

			// scale
			Vector3 scale = Vector3.Scale( Vector3.one, new Vector3( 0.3f, 1f, 1f ) );;
			scale.y = direction.magnitude * 10f;
			transform.localScale = scale;
		}

	}


}