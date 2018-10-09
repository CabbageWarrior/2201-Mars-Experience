// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lab_Minigame {

	// NODE ITSELF
	[System.Serializable]
	public class MinigameLevelNodeData {

		[HideInInspector]
		public int editX;
		
		[HideInInspector]
		public int editY;

		[SerializeField]
		public float x, y;

		[SerializeField]
		public int links;

		[SerializeField][HideInInspector]
		public bool enabled;

	}

	// NODE LIST
	[System.Serializable]
	public class MinigameLevelData {

		public List<MinigameLevelNodeData> NodeList;

		public MinigameLevelData()
		{
			NodeList = new List<MinigameLevelNodeData>();
		}

	}


	//////////////////////////////////////////////////////////////


	public class MinigameMgr : Puzzle {

		public	static	MinigameMgr		Instance			= null;

		public enum Direction
		{
			NONE = -1, UP, RIGHT, DOWN, LEFT	
		}
		
		private	int							m_TotLevels			= 0;

		
		public	SpriteCollection			SpritesCollection	= null;
		public	SpriteCollection			SpritesCollectionCompleted	= null;

		[SerializeField]
		private	GameEvent					OnLevel1Completed	= null;
		[SerializeField]
		private	GameEvent					OnLevel2Completed	= null;
		[SerializeField]
		private	GameEvent					OnLevel3Completed	= null;

//		[SerializeField]
		private	int							m_CurrentLevel		= 0;

		private MinigameLevelScriptable		m_LevelsData		= null;
		private	MinigameNode				m_MinigameNodeRes	= null;

		private	bool						m_IsOK				= false;

		private	Transform					m_DisplayTransform	= null;
//		[SerializeField]
		private	MinigameNode[]				m_ThisLevelNodes	= null;


		//////////////////////////////////////////////////////////////////////////
		// START
		protected void Start()
		{
            // STATIC REF
            Instance = this;
            // load scriptable object that contains the levels data

            m_LevelsData		= Resources.Load<MinigameLevelScriptable>( "Lab_Minigame/LevelsData" );
			m_MinigameNodeRes	= Resources.Load<MinigameNode>( "Lab_Minigame/MinigameNode" );

			if ( m_LevelsData == null )
			{
				print( "No Levels Data" );
				return;
			}

			if ( m_LevelsData.Levels == null || m_LevelsData.Levels.Count == 0 )
			{
				print( "No Levels" );
				return;
			}


			m_TotLevels = m_LevelsData.Levels.Count;
			m_DisplayTransform = transform.GetChild( 0 ).transform;
			m_IsOK = true;

			StartGame();
		}


		//////////////////////////////////////////////////////////////////////////
		// StartGame
		public	void	StartGame()
		{
			if ( m_IsOK == false )
				return;

			LoadLevel( m_CurrentLevel = 0 );
		}


		//////////////////////////////////////////////////////////////////////////
		// RestartLevel
		public	void	RestartLevel()
		{
			LoadLevel( m_CurrentLevel );
		}

		//////////////////////////////////////////////////////////////////////////
		// LoadLevel
		public	void	LoadLevel( int levelToLoad )
		{
			if ( m_IsOK == false )
				return;

			if ( levelToLoad == m_TotLevels )
			{
				OnGameOver();
				return;
			}

			// CLEAN CANVAS
			{
				foreach ( MinigameNode node in m_DisplayTransform.GetComponentsInChildren<MinigameNode>() )
					Destroy( node.gameObject );

				foreach ( MinigameLink link in m_DisplayTransform.GetComponentsInChildren<MinigameLink>() )
					Destroy( link.gameObject );

				if ( MinigameLink.Links != null )
					MinigameLink.Links.Clear();

				m_ThisLevelNodes = null;
			}

			RectTransform displayRectTransform = m_DisplayTransform as RectTransform;
			MinigameLevelData levelData =  m_LevelsData.Levels [ m_CurrentLevel = levelToLoad ];

			m_ThisLevelNodes = new MinigameNode[ levelData.NodeList.Count ];
			int currentNodeindex = 0;

			levelData.NodeList.ForEach ( 
				delegate ( MinigameLevelNodeData nodeData )
				{
					if ( nodeData == null || nodeData.enabled == false )
						return;

					MinigameNode node = Instantiate( m_MinigameNodeRes, displayRectTransform as Transform );
					node.name = "MinigameNode_" + currentNodeindex;
					node.transform.localPosition = new Vector3 (

						-( displayRectTransform.rect.width  / 2f ) + ( displayRectTransform.rect.width  * nodeData.y ) + 
						( ( m_MinigameNodeRes.transform as RectTransform ).rect.width  ) + 
						( ( m_MinigameNodeRes.transform as RectTransform ).pivot.x  ),
	
						( displayRectTransform.rect.height / 2f ) - ( displayRectTransform.rect.height * nodeData.x ) - 
						( ( m_MinigameNodeRes.transform as RectTransform ).rect.height ) -
						( ( m_MinigameNodeRes.transform as RectTransform ).pivot.y  ),
						0.0f

					);

					node.Setup( ref nodeData );

					m_ThisLevelNodes[ currentNodeindex ] = node;
					currentNodeindex++;
				}

			); // list ForEach

			MinigameLink.Links = new List<MinigameLink>();

		}


		//////////////////////////////////////////////////////////////////////////
		// GetOppositeDirection
		public static Direction GetOppositeDirection( Direction d )
		{
			switch( d )
			{
				case Direction.DOWN:	return Direction.UP;
				case Direction.UP:		return Direction.DOWN;
				case Direction.LEFT:	return Direction.RIGHT;
				case Direction.RIGHT:	return Direction.LEFT;
			}
			return Direction.NONE;
		}

		//////////////////////////////////////////////////////////////////////////
		// GetDirection
		public static	Direction	GetDirection( ref MinigameNode start, ref MinigameNode end )
		{
			if ( start.EditY == end.EditY )
			{
				return ( start.EditX < end.EditX ) ? Direction.LEFT : Direction.RIGHT;
			}

			if ( start.EditX == end.EditX )
			{
				return ( start.EditY < end.EditY ) ? Direction.UP : Direction.DOWN;
			}
			
/*			if ( start.EditX < end.EditX )									return Direction.RIGHT;
			if ( start.EditX > end.EditX )									return Direction.LEFT;
			if ( start.EditY < end.EditY )									return Direction.DOWN;
			if ( start.EditY > end.EditY )									return Direction.UP;
			*/
			return Direction.NONE;
		}
		

		//////////////////////////////////////////////////////////////////////////
		// GetLinearNearest
		private MinigameNode GetLinearNearest( MinigameNode node, bool sameXaxis, Direction dir )
		{
			MinigameNode nearestNode = null;
			float currentMinDistance = Mathf.Infinity;



			MinigameNode[] nodesOnSameAxis = System.Array.FindAll
			(
				m_ThisLevelNodes, 
				( n ) => { return ( sameXaxis == true ) ?  n.EditX == node.EditX : n.EditY == node.EditY; }
			);
			/*
			foreach( var a in nodesOnSameAxis )
				print( a.name );
			*/
			foreach( MinigameNode listNode in nodesOnSameAxis )
			{
				float distance = ( listNode.transform.position - node.transform.position ).sqrMagnitude;
				if ( distance < currentMinDistance && listNode != node )
				{
					if ( sameXaxis == true )
					{
//						print( "same x axis" );
						if ( dir == Direction.DOWN && listNode.EditY < node.EditY )
						{
//							print( "at last one found" );
							nearestNode = listNode;
							currentMinDistance = distance;
						}
						if ( dir == Direction.UP && listNode.EditY > node.EditY )
						{
//							print( "at last one found" );
							nearestNode = listNode;
							currentMinDistance = distance;
						}
					}
					else
					{
						if ( dir == Direction.RIGHT && listNode.EditX < node.EditX )
						{
							nearestNode = listNode;
							currentMinDistance = distance;
						}
						if ( dir == Direction.LEFT && listNode.EditX > node.EditX )
						{
							nearestNode = listNode;
							currentMinDistance = distance;
						}
					}
				}
			}
			return nearestNode;
		}


		//////////////////////////////////////////////////////////////////////////
		// CheckForNearest
		public bool		CheckForNearest( ref MinigameNode start, ref MinigameNode end, Direction dir )
		{
			MinigameNode nearestNode = GetLinearNearest( start, ( start.EditX == end.EditX ), dir );
			return ( nearestNode == end );
		}

		//Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
		//Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
		//same plane, use ClosestPointsOnTwoLines() instead.
		private bool LineLineIntersection( Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2 )
		{
			Vector3 lineVec3 = linePoint2 - linePoint1;
			Vector3 crossVec1and2	= Vector3.Cross( lineVec1, lineVec2 );
			Vector3 crossVec3and2	= Vector3.Cross( lineVec3, lineVec2 );
			float planarFactor		= Vector3.Dot( lineVec3, crossVec1and2 );

			//is coplanar, and not parrallel
			return ( Mathf.Abs( planarFactor ) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f );
		}


		//////////////////////////////////////////////////////////////////////////
		// CheckForIntersection
		public	bool	CheckForIntersection( ref MinigameNode start, ref MinigameNode end )
		{
			foreach( var link in MinigameLink.Links )
			{
				// skip the link with the same nodes
				if ( ( link.Node1 == start && link.Node2 == end ) || ( link.Node2 == start && link.Node1 == end ) )
					continue;

				if ( LineLineIntersection ( link.Node1.transform.position, link.Node2.transform.position, start.transform.position, end.transform.position ) )
					return true;
			}

			return false;
		}


		//////////////////////////////////////////////////////////////////////////
		// LevelCheck
		public	void	LevelCheck()
		{
			bool completed = true;
			foreach( IMinigameNode node in m_ThisLevelNodes )
			{
				if ( node.AvaiableLinks > 0 )
				{
					completed = false;
					break;
				}
			}

			if ( completed )
			{
				if ( m_CurrentLevel == 0 ) OnLevel1Completed.Invoke();
				if ( m_CurrentLevel == 1 ) OnLevel2Completed.Invoke();
				if ( m_CurrentLevel == 2 ) OnLevel3Completed.Invoke();

				LoadLevel( m_CurrentLevel + 1 );
			}
		}


		//////////////////////////////////////////////////////////////////////////
		// OnGameOver
		private	void	OnGameOver()
		{
			if ( IsCompleted )
				return;

			print( "puzzle completed" );

			// game over actions
			OnCompletion();

//			Destroy( gameObject );
		}

	}


}