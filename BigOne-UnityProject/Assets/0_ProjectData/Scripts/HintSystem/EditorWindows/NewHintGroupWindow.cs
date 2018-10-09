// Scripted by Roberto Leogrande

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;


namespace HintSystem {

	using System.Collections.Generic;

	public class NewHintGroupWindow : EditorWindow {

		public static	NewHintGroupWindow		pWindow				= null;

		private	HintPuzzle				m_HintPuzzleRef				= null;
		private	string					m_HintGroupName				= "Insert a name";
		private	bool					m_NameSet					= false;
		private	HintGroup				m_ThisHintGroup				= null;


		// Window show
		public static void Init( HintPuzzle hintPuzzle ) {

			if ( pWindow != null ) {
				pWindow.Close();
				pWindow = null;
			}
			
			if ( pWindow == null ) {
				pWindow = EditorWindow.GetWindow<NewHintGroupWindow>( true, "New Hint Group" );
				pWindow.minSize = pWindow.maxSize = new Vector2( 400f, 200f );
			}

			pWindow.m_HintPuzzleRef = hintPuzzle;

			pWindow.Show();

		}


		private	void	CreateDefaultHints() {

			{	// HINT PRO
				string assetPathPro = EditorInfo.ResPathOthers + m_HintPuzzleRef.name + "_" + m_HintGroupName + "_Generic.asset";
				Hint hintPro = ScriptableObject.CreateInstance<Hint>();
				hintPro.Father = m_ThisHintGroup;
				hintPro.Name = "Generic";
				hintPro.Path = assetPathPro;
				AssetDatabase.CreateAsset( hintPro, assetPathPro );
				EditorUtility.SetDirty( hintPro );
				m_ThisHintGroup.Childs.Add( hintPro );
			}

			{	// HINT NORM
				string assetPathNorm = EditorInfo.ResPathOthers + m_HintPuzzleRef.name + "_" + m_HintGroupName + "_Medium.asset";
				Hint hintNorm = ScriptableObject.CreateInstance<Hint>();
				hintNorm.Father = m_ThisHintGroup;
				hintNorm.Name = "Medium";
				hintNorm.Path = assetPathNorm;
				AssetDatabase.CreateAsset( hintNorm, assetPathNorm );
				EditorUtility.SetDirty( hintNorm );
				m_ThisHintGroup.Childs.Add( hintNorm );
			}

			{	// HINT NOOB
				string assetPathNoob = EditorInfo.ResPathOthers + m_HintPuzzleRef.name + "_" + m_HintGroupName + "_Specific.asset";
				Hint hintNoob = ScriptableObject.CreateInstance<Hint>();
				hintNoob.Father = m_ThisHintGroup;
				hintNoob.Name = "Specific";
				hintNoob.Path = assetPathNoob;
				AssetDatabase.CreateAsset( hintNoob, assetPathNoob );
				EditorUtility.SetDirty( hintNoob );
				m_ThisHintGroup.Childs.Add( hintNoob );
			}

		}


		//////////////////////////////////////////////////////////////////////////
		// SetupAsset
		private bool	SetupAsset() {

			string assetPath = EditorInfo.ResPathOthers + m_HintPuzzleRef.name + "_" + m_HintGroupName + ".asset";

			// if asset already exists return false
			if ( System.IO.File.Exists( assetPath ) )
				return false;

			m_ThisHintGroup = ScriptableObject.CreateInstance<HintGroup>();

			m_ThisHintGroup.Father = m_HintPuzzleRef;
			m_ThisHintGroup.Path = assetPath;
			m_ThisHintGroup.Name = m_HintGroupName;

			m_ThisHintGroup.Childs = new List<HintCommon>();
			CreateDefaultHints();

			AssetDatabase.CreateAsset( m_ThisHintGroup, assetPath );
			AssetDatabase.SaveAssets();


			return true;
		}


		//////////////////////////////////////////////////////////////////////////
		// Draw
		private void OnGUI() {
			
			if ( m_NameSet == false ) {

				GUILayout.Label( "Enter the hint group name" );

				GUI.SetNextControlName ( "groupNamingControl" );
				m_HintGroupName = GUILayout.TextField( m_HintGroupName );
				EditorGUI.FocusTextInControl ( "groupNamingControl" );

				if ( GUILayout.Button("Set")  || Event.current.keyCode == KeyCode.Return ) {
					if ( m_HintGroupName.Length > 0  && SetupAsset()) {
						m_NameSet = true;
						m_HintPuzzleRef.Childs.Add( m_ThisHintGroup );
						pWindow.Close();
					}
				}
			}

		}


		// Save
		private void OnDestroy() {
			
			if( m_ThisHintGroup ) {
				EditorUtility.SetDirty( m_ThisHintGroup );
				AssetDatabase.SaveAssets();
			}

		}


	}


}

#endif