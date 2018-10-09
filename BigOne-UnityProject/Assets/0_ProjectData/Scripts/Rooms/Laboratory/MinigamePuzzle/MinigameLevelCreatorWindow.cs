// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


namespace Lab_Minigame {


	// EDITOR
	public class MinigameLevelCreatorWindow : EditorWindow {


		public static MinigameLevelCreatorWindow		pWindow				= null;

		private static	List<MinigameLevelNodeData>		m_ThisLevelData		= null;
		
		private static  Vector2							m_GridSize			= Vector2.zero;

		public static	MinigameLevelNodeData [,]		m_DataMask			= null;


		public static void  Init ( Vector2 grid, int level ) {

			if ( pWindow != null ) {
				pWindow.Close();
				pWindow = null;
			}
			
			if ( pWindow == null ) {
				pWindow = EditorWindow.GetWindow<MinigameLevelCreatorWindow>( true, "Lab Minigame Level" );
				pWindow.maxSize = pWindow.minSize = new Vector2( grid.x * 80f + 150f, grid.y * 20f + 50f );
				
			}

			m_GridSize = grid;

			m_DataMask = new MinigameLevelNodeData[ (int)m_GridSize.x, (int)m_GridSize.y ];

			Setup( pWindow, level );

			pWindow.Show();
			
		}
		
		private static void Setup( MinigameLevelCreatorWindow w, int level  ) {

			m_ThisLevelData = MinigameLevelCreatorMgrWindow.pWindow.Levels[ level ].NodeList;
			

			for ( int i = 0; i < m_DataMask.GetLength(0); i++ ) {

				for ( int j = 0; j < m_DataMask.GetLength(1); j++ ) {

					MinigameLevelNodeData node = m_ThisLevelData.Find( n => n.editX == i && n.editY == j && n.enabled );

					if ( node != null ) {

						m_DataMask[ i, j ] = node;

					}
					else
						m_DataMask[ i, j ] = new MinigameLevelNodeData();

				}

			}

		}
		

		private void	OnGUI() {

			// close this window if mather has been closed
			if ( MinigameLevelCreatorMgrWindow.pWindow == null ) {
				pWindow.Close();
				return;
			}
			
			GUILayout.BeginVertical();
			for ( int i = 0; i < m_DataMask.GetLength(0); i++ ) {


				GUILayout.BeginHorizontal();
				for ( int j = 0; j < m_DataMask.GetLength(1); j++ ) {

					m_DataMask[ i, j ].enabled = EditorGUILayout.Toggle( m_DataMask[ i, j ].enabled );

					int links = EditorGUILayout.IntField( m_DataMask[i,j].links );
					if ( m_DataMask[ i, j ].enabled ) {

						m_DataMask[i,j].links = Mathf.Clamp( links, 0, 8 );
						m_DataMask[i,j].editX = i;
						m_DataMask[i,j].editY = j;
						m_DataMask[i,j].x = (float) i / m_DataMask.GetLength(0);
						m_DataMask[i,j].y = (float) j / m_DataMask.GetLength(1);
	
					}
					else {
						// read only
						m_DataMask[i,j].links = 0;
					}
					
				}
				GUILayout.EndHorizontal();


			}
			GUILayout.EndVertical();
			


			if ( GUILayout.Button( "OK" ) ) {

				pWindow.Close();

			}


		}

		private void OnDestroy() {

			m_ThisLevelData.Clear();

			// save content
			for ( int i = 0; i < m_DataMask.GetLength(0); i++ ) {

				for ( int j = 0; j < m_DataMask.GetLength(1); j++ ) {

					if ( m_DataMask[ i, j ].enabled ) {

						m_ThisLevelData.Add( m_DataMask[ i, j ] );

					}


				}

			}


		}


	}


}


#endif