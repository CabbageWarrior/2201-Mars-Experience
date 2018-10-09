// Scripted by Roberto Leogrande

using UnityEngine;

#if UNITY_EDITOR

namespace Cutscene {

	using UnityEditor;
	using FMODUnity;

	public class SequenceFrameEditor : EditorWindow {

		public static	SequenceFrameEditor		pWindow			= null;

		private	SequenceFrame			m_CurrentSequenceFrame	= null;
		private Vector2					m_ScrollPosition		= Vector2.zero;

		private	GUIStyle				m_CenteredStyle			= null;

		private	string					m_Name					= string.Empty;
		private	string					m_Text					= string.Empty;

		public static void	Init( SequenceFrame sequenceFrame )
		{

			if ( pWindow != null ) {
				pWindow.Close();
				pWindow = null;
			}

			if ( pWindow == null ) {
				pWindow = EditorWindow.GetWindow<SequenceFrameEditor>( true, "Sequence Frame Editor" );
				pWindow.minSize = new Vector2( 800f, 400f );
			}

			pWindow.m_CurrentSequenceFrame = sequenceFrame;
			pWindow.m_CenteredStyle = new GUIStyle {
				alignment = TextAnchor.MiddleCenter
			};

			pWindow.m_Name	= pWindow.m_CurrentSequenceFrame.SpeakerName;
			pWindow.m_Text	= pWindow.m_CurrentSequenceFrame.Text;

			pWindow.Show();
			pWindow.Focus();

		}

		private GUIStyle textAreaWrapTextStyle = null;

		private void OnGUI()
		{

			if ( textAreaWrapTextStyle == null )
			{
				textAreaWrapTextStyle = new GUIStyle( EditorStyles.textArea ) {
					wordWrap = true
				};
			}

			m_ScrollPosition = GUILayout.BeginScrollView( m_ScrollPosition );
			{
				GUILayout.BeginVertical();
				{
					{	// NAME
						GUILayout.Label( "NAME", m_CenteredStyle );
						m_CurrentSequenceFrame.SpeakerName = GUILayout.TextField( m_CurrentSequenceFrame.SpeakerName );
					}

					{   // TEXT
						GUILayout.Label( "TEXT", m_CenteredStyle );
						m_CurrentSequenceFrame.Text = EditorGUILayout.TextArea( m_CurrentSequenceFrame.Text, textAreaWrapTextStyle, GUILayout.MinHeight( 150f ) );
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndScrollView();

		}


		private void OnDestroy()
		{	
			if ( m_CurrentSequenceFrame.SpeakerName != m_Name || m_CurrentSequenceFrame.Text != m_Text )
			{
				EditorUtility.SetDirty( m_CurrentSequenceFrame );
				AssetDatabase.SaveAssets();
			}
		}

	}

}

#endif
