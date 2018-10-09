// Scripted by Roberto Leogrande

#if UNITY_EDITOR
using UnityEditor;



namespace Lab_Minigame {

	using UnityEngine;
	using System.Collections.Generic;

	// LAB MINIGAME LEVEL EDITOR MAIN CLASS
	public class MinigameLevelCreatorMgrWindow : EditorWindow {

		public static	MinigameLevelCreatorMgrWindow	pWindow				= null;

		private	static	MinigameLevelScriptable			m_LevelsData		= null;

		public	List<MinigameLevelData>		Levels {
			get { return  m_LevelsData.Levels; }	
		}	

		private Vector2									m_ScrollPosition	= Vector2.zero;
		private	Vector2									m_GridSize			= Vector2.zero;
		private	bool									m_GridSet			= false;


		[MenuItem ("Window/Lab Minigame/Level Editor")]
		private static void  Init ()
		{
			if ( pWindow != null )
			{
				pWindow.Close();
				pWindow = null;
			}
			
			if ( pWindow == null )
			{
				pWindow = EditorWindow.GetWindow<MinigameLevelCreatorMgrWindow>( true, "Lab Minigame LE" );
				pWindow.minSize = new Vector2( 800f, 400f );
			}
			pWindow.Show();

			Setup( pWindow );
			
		}


		//////////////////////////////////////////////////////////////////////////
		// Setup
		private static void Setup( MinigameLevelCreatorMgrWindow w )
		{
		
			// Create asset if not exists
			if ( !System.IO.File.Exists( "Assets/0_ProjectData/Resources/Lab_Minigame/LevelsData.asset" ) )
			{
				m_LevelsData = ScriptableObject.CreateInstance<MinigameLevelScriptable>();
				AssetDatabase.CreateAsset( m_LevelsData, "Assets/0_ProjectData/Resources/Lab_Minigame/LevelsData.asset" );
				AssetDatabase.SaveAssets();

				m_LevelsData.Levels = new List<MinigameLevelData>();
			}
			// else load data
			else
			{
				m_LevelsData = AssetDatabase.LoadAssetAtPath<MinigameLevelScriptable>( "Assets/0_ProjectData/Resources/Lab_Minigame/LevelsData.asset" );
				if ( m_LevelsData.GridSize.magnitude > 0 )
				{
					w.m_GridSize = m_LevelsData.GridSize;
					w.m_GridSet = true;
				}
			}

			EditorUtility.SetDirty( m_LevelsData );
		}


		//////////////////////////////////////////////////////////////////////////
		// Draw
		private	void	OnGUI () {


			if ( m_GridSet == false )
			{
				GUILayout.Label ( "DEFINE THE GRID SIZE" );

				m_GridSize.x = EditorGUILayout.IntField( (int)m_GridSize.x );
				m_GridSize.y = EditorGUILayout.IntField( (int)m_GridSize.y );

				if ( GUILayout.Button( "SET" ) )
				{
					if ( m_GridSize.x > 0 && m_GridSize.y > 0 )
					{
						m_LevelsData.GridSize = m_GridSize;
						m_GridSet = true;
					}
					else
					{
						EditorUtility.DisplayDialog( "Warning", "Invalid grid size !!!", "OK" );
					}
				}
				return;
			}


			// ADD LEVEL BUTTON
			if ( GUILayout.Button( "New Level" ) )
			{
				m_LevelsData.Levels.Add( new MinigameLevelData() );
			}

			// LEVELS
			m_ScrollPosition = GUILayout.BeginScrollView( m_ScrollPosition );
			{

				GUILayout.BeginVertical();
				{
					for ( int i = 0; i < m_LevelsData.Levels.Count; i++ ) {

						GUILayout.BeginHorizontal();
						{

							GUILayout.Label ( "Level " + ( i + 1 ) );
							if ( GUILayout.Button( "Edit" ) )
							{
								// show single level editor
								MinigameLevelCreatorWindow.Init( m_GridSize, i );
							}

							if ( GUILayout.Button( "Remove" ) )
							{
								m_LevelsData.Levels.RemoveAt( i );
								GUILayout.EndHorizontal();
								GUILayout.EndVertical();
								GUILayout.EndScrollView();
								return;
							}
						}
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndScrollView();


			// DELETE BUTTON
			if ( m_GridSet && GUILayout.Button( "RESET" ) )
			{
				if ( System.IO.File.Exists( "Assets/0_ProjectData/Resources/Lab_Minigame/LevelsData.asset" ) ) {
					AssetDatabase.DeleteAsset( "Assets/0_ProjectData/Resources/Lab_Minigame/LevelsData.asset" );
				}

				m_LevelsData	= null;
				m_GridSize		= Vector2.zero;
				m_GridSet		= false;

				Setup( pWindow );
			}
		}


		private void OnDestroy()
		{
			EditorUtility.SetDirty( m_LevelsData );
			AssetDatabase.SaveAssets();
		}



	}



}
#endif