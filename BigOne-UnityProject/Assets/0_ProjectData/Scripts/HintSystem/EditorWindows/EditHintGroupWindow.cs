// Scripted by Roberto Leogrande

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace HintSystem {

	using System.Collections.Generic;


	public class EditHintGroupWindow : EditorWindow {

		private	const	bool	bEnablePreview	= true;

		public static	EditHintGroupWindow	pWindow						= null;

		private	HintGroup					m_ThisHintGroup				= null;

		private	AudioSource					m_AudioSource				= null;

		public GameObject					m_SoundPlayer				= null;

		private Vector2						m_ScrollPosition			= Vector2.zero;

		private	bool						m_IsCycleTransition			= false;

		// Window show
		public static void Init( HintGroup	 hintGroup ) {
			
			if ( pWindow != null ) {
				pWindow.Close();
				pWindow = null;
			}
			
			if ( pWindow == null ) {
				pWindow = EditorWindow.GetWindow<EditHintGroupWindow>( true, "Hint Group Editor" );
				pWindow.minSize = pWindow.maxSize = new Vector2( 800f, 400f );
			}

			pWindow.m_SoundPlayer = new GameObject();
			pWindow.m_SoundPlayer.hideFlags = HideFlags.HideAndDontSave;
			pWindow.m_AudioSource = pWindow.m_SoundPlayer.AddComponent<AudioSource>();

			pWindow.m_ThisHintGroup = hintGroup;

			pWindow.Show();

			pWindow.m_IsCycleTransition = ( ( pWindow.m_ThisHintGroup.Father as HintPuzzle ).Father as HintCycle ).IsTransition;

			if ( pWindow.m_ThisHintGroup.Childs == null || pWindow.m_ThisHintGroup.Childs.Count < 3 || pWindow.m_IsCycleTransition )
				return;
			 
			Hint hint1 = pWindow.m_ThisHintGroup.Childs[0] as Hint;
			Hint hint2 = pWindow.m_ThisHintGroup.Childs[1] as Hint;
			Hint hint3 = pWindow.m_ThisHintGroup.Childs[2] as Hint;

			// auto serach other files for auto completion
			if ( hint1.ClipSound == null && hint2.ClipSound == null && hint2.ClipSound == null )
			{
				
				string hintsPath = EditorUtility.OpenFolderPanel("Hint Directory", "", "");

				if ( hintsPath == null || hintsPath.Length == 0 )
					return;

				string[] files = System.IO.Directory.GetFiles( hintsPath, "*.ogg" );

				if ( files.Length < 3 )
					return;

				int assetWordIdx = files[0].IndexOf( "Asset" );

				{
					string filePath = files[0].Substring( assetWordIdx );
					hint1.ClipSound = AssetDatabase.LoadAssetAtPath<AudioClip>( filePath );
				}
				{
					string filePath = files[1].Substring( assetWordIdx );
					hint2.ClipSound = AssetDatabase.LoadAssetAtPath<AudioClip>( filePath );
				}
				{
					string filePath = files[2].Substring( assetWordIdx );
					hint3.ClipSound = AssetDatabase.LoadAssetAtPath<AudioClip>( filePath );
				}
				
			}

		}


		//////////////////////////////////////////////////////////////////////////
		// FindHintName
		private string FindHintName( List<HintCommon> hints )
		{

			for ( int i = 0; i < 30; i++ )
			{
				Hint hint = hints.Find( h => h.Name == "Hint_" + i ) as Hint;
				if ( hint == null )
				{
					return "Hint_" + i;
				}
			}
			return "";
		}


		private GUIStyle textAreaWrapTextStyle = null;

		// Draw
		private void OnGUI() {

			if ( textAreaWrapTextStyle == null )
			{
				textAreaWrapTextStyle = new GUIStyle( EditorStyles.textArea );
				textAreaWrapTextStyle.wordWrap = true;
			}

			if ( m_ThisHintGroup.Childs == null ) return;

			if ( GUILayout.Button( "Create Hint" ) )
			{
				{	// HINT PRO
					string assetName = FindHintName( m_ThisHintGroup.Childs );
					if ( assetName.Length == 0 )
						return;
					string assetPathPro = ( m_IsCycleTransition ? EditorInfo.ResPathTransitions : EditorInfo.ResPathOthers ) + m_ThisHintGroup.name + "_" + assetName + ".asset";
					Hint hint = ScriptableObject.CreateInstance<Hint>();
					hint.Father = m_ThisHintGroup;
					hint.Name = assetName;
					hint.Path = assetPathPro;
					AssetDatabase.CreateAsset( hint, assetPathPro );
					EditorUtility.SetDirty( hint );
					m_ThisHintGroup.Childs.Add( hint );
				}
			}

			m_ScrollPosition = GUILayout.BeginScrollView( m_ScrollPosition );
			{
				GUILayout.BeginVertical();
				{
					GUILayout.Label ( "Father" );
					EditorGUILayout.ObjectField( m_ThisHintGroup.Father, typeof( HintCommon ), true );

					for ( int i = 0; i < m_ThisHintGroup.Childs.Count; i++ )
					{
						Hint hint = m_ThisHintGroup.Childs[ i ] as Hint;
						GUILayout.Label( "------------------------------------------------------------" );
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label ( hint.Name );
							hint.Text		= EditorGUILayout.TextArea( hint.Text, textAreaWrapTextStyle, GUILayout.MinWidth( 400 ), GUILayout.MaxWidth( 400 ), GUILayout.MinHeight( 200 ) );
							hint.ClipSound	= EditorGUILayout.ObjectField( hint.ClipSound, typeof( AudioClip ), true ) as AudioClip;

							GUILayout.BeginVertical();
							{
								if ( GUILayout.Button( "Clear" ) )
								{
									hint.Text		= "";
									hint.ClipSound	= null;
									EditorUtility.SetDirty( hint );
								}

								if ( GUILayout.Button( "Remove" ) )
								{
									string assetPath = hint.Path;
									m_ThisHintGroup.Childs.RemoveAt( i );
									AssetDatabase.DeleteAsset( assetPath );
									GUILayout.EndHorizontal();
									GUILayout.EndVertical();
									GUILayout.EndScrollView();
									return;
								}

								// PLAY BUTTON
								if ( hint.ClipSound != null && m_AudioSource.clip != hint.ClipSound && GUILayout.Button( "Play" ) ) {
									if ( m_AudioSource.clip != hint.ClipSound )
									{
										m_AudioSource.Stop();
										m_AudioSource.clip = hint.ClipSound;
										m_AudioSource.Play();
									}
								}

								// STOP BUTTON
								if ( m_AudioSource.isPlaying && m_AudioSource.clip == hint.ClipSound && GUILayout.Button( "Stop" ) )
								{
									m_AudioSource.Stop();
									m_AudioSource.clip = null;
								}

								if ( !m_AudioSource.isPlaying )
								{
									m_AudioSource.clip = null;
								}
							}
							GUILayout.EndVertical();
						}
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndScrollView();
		}

		// Save
		private void OnDestroy() {

			if ( m_SoundPlayer ) {

				if ( m_AudioSource ) {
					m_AudioSource.Stop();
					m_AudioSource = null;
				}

				DestroyImmediate( m_SoundPlayer );

			}

			if ( m_ThisHintGroup ) {

				EditorUtility.SetDirty( m_ThisHintGroup );
				AssetDatabase.SaveAssets();

			}

		}

	}



}

#endif