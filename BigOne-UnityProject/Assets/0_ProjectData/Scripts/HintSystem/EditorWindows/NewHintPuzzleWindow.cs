// Scripted by Roberto Leogrande

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;


namespace HintSystem {

	using System.Collections.Generic;

	public class NewHintPuzzleWindow : EditorWindow {
	
		public static	NewHintPuzzleWindow		pWindow				= null;

		private	HintCycle				m_HintCycleRef				= null;
		private	string					m_PuzzleName				= "Insert a name";
		private	bool					m_NameSet					= false;
		private	HintPuzzle				m_ThisHintPuzzle			= null;

		// Window show
		public static void  Init ( HintCycle hintCycle ) {

			if ( pWindow != null ) {
				pWindow.Close();
				pWindow = null;
			}
			
			if ( pWindow == null ) {
				pWindow = EditorWindow.GetWindow<NewHintPuzzleWindow>( true, "New Hint Puzzle" );
				pWindow.minSize = pWindow.maxSize = new Vector2( 400f, 200f );
			}

			pWindow.m_HintCycleRef = hintCycle;

			pWindow.Show();

		}


		//////////////////////////////////////////////////////////////////////////
		// SetupAsset
		private bool	SetupAsset() {

			string assetPath = EditorInfo.ResPathOthers + m_HintCycleRef.name + "_" + m_PuzzleName + ".asset";

			// if asset already exists return false
			if ( System.IO.File.Exists( assetPath ) )
				return false;

			m_ThisHintPuzzle = ScriptableObject.CreateInstance<HintPuzzle>();

			m_ThisHintPuzzle.Father = m_HintCycleRef;
			m_ThisHintPuzzle.Path = assetPath;
			m_ThisHintPuzzle.Name = m_PuzzleName;

			m_ThisHintPuzzle.Childs = new List<HintCommon>();

			AssetDatabase.CreateAsset( m_ThisHintPuzzle, assetPath );
			AssetDatabase.SaveAssets();

			return true;
		}


		//////////////////////////////////////////////////////////////////////////
		// Draw
		private void OnGUI() {
			
			if ( m_NameSet == false ) {

				GUILayout.Label( "Enter the hint puzzle name" );

				GUI.SetNextControlName ( "puzzleNamingControl" );
				m_PuzzleName = GUILayout.TextField( m_PuzzleName );
				EditorGUI.FocusTextInControl ( "puzzleNamingControl" );

				if ( GUILayout.Button("Set") || Event.current.keyCode == KeyCode.Return ) {
					if ( m_PuzzleName.Length > 0  && SetupAsset()) {
						m_NameSet = true;
						m_HintCycleRef.Childs.Add( m_ThisHintPuzzle );
						HintTreeCreatorMgrWindow.pWindow.Repaint();
						pWindow.Close();
					}
				}
			}

		}


		// Save
		private void OnDestroy() {
			
			if ( m_ThisHintPuzzle ) {
				EditorUtility.SetDirty( m_ThisHintPuzzle );
				AssetDatabase.SaveAssets();
			}

		}


	}


}

#endif