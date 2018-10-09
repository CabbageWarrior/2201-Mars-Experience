// Scripted by Roberto Leogrande

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;


namespace HintSystem {

	using System.Collections.Generic;

	public class NewHintCycleWindow : EditorWindow {

		public static	NewHintCycleWindow		pWindow				= null;

		private	string					m_CycleName					= "Insert a name";
		private	bool					m_NameSet					= false;
		private	HintCycle				m_ThisHintCycle				= null;

		private	bool					m_IsTransition				= false;


		// Window show
		public static void  Init () {

			if ( pWindow != null ) {
				pWindow.Close();
				pWindow = null;
			}
			
			if ( pWindow == null ) {
				pWindow = EditorWindow.GetWindow<NewHintCycleWindow>( true, "New Hint Cycle" );
				pWindow.minSize = pWindow.maxSize = new Vector2( 400f, 200f );
			}

			pWindow.Show();

		}

		//////////////////////////////////////////////////////////////////////////
		// SetupAsset
		private bool	SetupAsset() {

			string assetPath = ( m_IsTransition ? EditorInfo.ResPathTransitions : EditorInfo.ResPathCycles ) + m_CycleName + ".asset";

			// if asset already exists return false
			if ( System.IO.File.Exists( assetPath ) )
				return false;

			m_ThisHintCycle = ScriptableObject.CreateInstance<HintCycle>();

			m_ThisHintCycle.Path = assetPath;
			m_ThisHintCycle.Name = m_CycleName;
			m_ThisHintCycle.IsTransition = m_IsTransition;

			m_ThisHintCycle.Childs = new List<HintCommon>();

			AssetDatabase.CreateAsset( m_ThisHintCycle, assetPath );
			AssetDatabase.SaveAssets();

			
			if ( m_IsTransition )
			{
				// CREATE TRANSITION PUZZLE
				HintPuzzle puzzle = ScriptableObject.CreateInstance<HintPuzzle>();
				puzzle.Father = m_ThisHintCycle;
				puzzle.name = m_CycleName + "_TransPuzzle";
				puzzle.Name = "TransPuzzle";
				puzzle.Path = EditorInfo.ResPathTransitions + puzzle.name + ".asset";
				AssetDatabase.CreateAsset( puzzle, puzzle.Path );

				m_ThisHintCycle.Childs.Add( puzzle );

				puzzle.Childs = new List<HintCommon>();

				// CREATE TRANSITION HINT GROUP
				HintGroup group = ScriptableObject.CreateInstance<HintGroup>();
				group.Father = puzzle;
				group.name = puzzle.name + "_TransGroup";
				group.Name = "TransGroup";
				group.Path = EditorInfo.ResPathTransitions + group.name + ".asset";
				AssetDatabase.CreateAsset( group, group.Path );

				puzzle.Childs.Add( group );

				group.Childs = new List<HintCommon>();

				EditHintGroupWindow.Init( group );
//				pWindow.Close();

			}
			

			return true;
		}


		//////////////////////////////////////////////////////////////////////////
		// Draw
		private void OnGUI() {
			
			if ( m_NameSet == false ) {

				GUILayout.Label( "Enter the cycle name" );

				GUI.SetNextControlName ( "cycleNamingControl" );
				m_CycleName = GUILayout.TextField( m_CycleName );
				EditorGUI.FocusTextInControl ("cycleNamingControl" );
				m_IsTransition = EditorGUILayout.ToggleLeft( "Is transition", m_IsTransition );

				if ( GUILayout.Button("Set") || Event.current.keyCode == KeyCode.Return ) {
					if ( m_CycleName.Length > 0  && SetupAsset()) {
						m_NameSet = true;
						HintTreeCreatorMgrWindow.pWindow.CycleCollection.Add( m_ThisHintCycle );
						HintTreeCreatorMgrWindow.pWindow.Repaint();
						pWindow.Close();
					}
				}
			}

		}


		// Save
		private void OnDestroy() {
			
			if ( m_ThisHintCycle ) {
				EditorUtility.SetDirty( m_ThisHintCycle );
				AssetDatabase.SaveAssets();
			}

		}

	}

}

#endif