// Scripted by Roberto Leogrande

using TMPro;
using UnityEngine;
using UnityEngine.UI;



/*
 * - If into text is found w[n], writing is suspended for n seconds
 * - Frame duration is an override value for audio duration
 * - If duration is elapsed, but text is writing, manager will wait for text writing completion
*/


namespace Cutscene {

	using FMODUnity;

	public enum Sources {

		PIPBOY4K, MARIA

	}
	

	public class CutsceneManager : MonoBehaviour {

		public	static	CutsceneManager Instance			= null;

		[SerializeField][Range( 0.001f, 1f ) ]
		private	float				m_CharsDelay			= 0.1f;
		public	float				CharsDelay
		{
			get { return m_CharsDelay; }
		}

		private	CutsceneSequence		m_CurrentSequence	= null;
		public	CutsceneSequence		CurrentSequence
		{
			get { return m_CurrentSequence; }
			set
			{
				if ( value == null && m_CurrentSequence != null && m_CurrentSequence.EndSequenceCallback != null )
				{
					m_CurrentSequence.EndSequenceCallback();
				}
				m_CurrentSequence = value;
			}
		}

		[SerializeField]
		private	StudioEventEmitter[]	m_EventEmitters		= null;

		[SerializeField]
		private	GameObject				m_CutsceneInterface	= null;
		public	GameObject				CutsceneInterface
		{
			get { return m_CutsceneInterface; }
		}

		[SerializeField]
		private	Text					m_InterfaceSpeaker	= null;
		public	Text					InterfaceSpeaker
		{
			get { return m_InterfaceSpeaker; }
		}

		[SerializeField]
		private	TextMeshPro				m_InterfaceMessage	= null;
		public	TextMeshPro				InterfaceMessage
		{
			get { return m_InterfaceMessage; }
		}

		private	bool					m_IsOK				= false;
		public	bool					IsOK
		{
			get { return m_IsOK; }
		}


		//////////////////////////////////////////////////////////////////////////
		// AWAKE
		private void Awake()
		{
            // STATIC REF
            Instance = this;

            m_CutsceneInterface.gameObject.SetActive( false );

			// Sanity checks
			if (m_EventEmitters == null
			||	m_EventEmitters.Length == 0
			||	m_CutsceneInterface == null
			||	m_InterfaceSpeaker == null
			||	m_InterfaceMessage == null
			)	return;

			m_IsOK = true;
		}


		//////////////////////////////////////////////////////////////////////////
		// StopCutscene
		public	void	ShowInterface()
		{
			if ( m_IsOK == false )
				return;

			m_CutsceneInterface.SetActive( true );
		}


		//////////////////////////////////////////////////////////////////////////
		// StopCutscene
		public	void	HideInterface()
		{
			if ( m_IsOK == false )
				return;

			m_CutsceneInterface.SetActive( false );
		}


		//////////////////////////////////////////////////////////////////////////
		// StopCutscene
		public	StudioEventEmitter	GetEventEmitter( Sources source )
		{
			if ( m_IsOK == false )
				return null;

			return m_EventEmitters[ (int) source ];
		}


		//////////////////////////////////////////////////////////////////////////
		// InterruptCurrentSequence
		public	void	InterruptCurrentSequence()
		{
			if( m_IsOK && m_CurrentSequence != null )
			{
				m_CurrentSequence.InterruptSequence();
			}
		}


		//////////////////////////////////////////////////////////////////////////
		// InterruptCurrentSequence
		public	void	SkipCurrentSequenceFrame()
		{
			if( m_IsOK && m_CurrentSequence != null )
			{
				m_CurrentSequence.SkipFrame();
			}
		}

	}

}
