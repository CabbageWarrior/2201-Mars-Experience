// Scripted by Roberto Leogrande

using System.Collections;
using TMPro;
using UnityEngine;


namespace Cutscene {

	using FMOD.Studio;
	using FMODUnity;

	public class CutsceneSequence : MonoBehaviour {

		public Sources						Source					= Sources.PIPBOY4K;

		public	GameEventArg1				m_OnSequnceFinished		= null;
//		public	GameEventArg1				m_OnSequnceInterrupted	= null;
		private	global::System.Action		m_EndSequenceCallback	= null;
		public	global::System.Action		EndSequenceCallback
		{
			get { return m_EndSequenceCallback; }
		}

		private	bool						m_IsCompleted			= false;
		public	bool						IsCompleted
		{
			get { return m_IsCompleted; }
		}

		[SerializeField]
		private	SequencePair[]				m_SequenceFrames		= null;


		private	SequencePair				m_CurrentFramePair		= null;
		private	int							m_CurrentItemIdx		= 0;
		private	StudioEventEmitter			m_CurrentAudioSource	= null;

        private	StudioEventEmitter			m_RuntimeAudioSource	= null;
		//////////////////////////////////////////////////////////////////////////
		// START
//		private void Start()
//		{
//		}


		//////////////////////////////////////////////////////////////////////////
		// Build
		public CutsceneSequence Build( SequencePair[] sequenceFrames, global::System.Action endSequenceCallback = null )
		{
			m_SequenceFrames = sequenceFrames;
			m_EndSequenceCallback = endSequenceCallback;
			return this;
		}


		//////////////////////////////////////////////////////////////////////////
		// NextValidFrame
		private	SequencePair NextValidFrame()
		{
			SequencePair sequncePair = m_SequenceFrames[ m_CurrentItemIdx ];

			// if m_CurrentFrame has null value, iterate for next valid frame
			while( sequncePair.SequenceFrame == null || sequncePair.SequenceFrame.AudioRef == null || sequncePair.SequenceFrame.AudioRef.Length == 0 )
			{
				m_CurrentItemIdx ++;
				sequncePair = m_SequenceFrames[ m_CurrentItemIdx ];
			}

			return sequncePair;
		}


		//////////////////////////////////////////////////////////////////////////
		// SetEndCallback
		public CutsceneSequence	SetEndCallback( global::System.Action action )
		{
			m_EndSequenceCallback = action;
			return this;
		}


		//////////////////////////////////////////////////////////////////////////
		// Play
		public	void	Play()
		{
			// Sanity checks
			if ( ( m_SequenceFrames == null || m_SequenceFrames.Length == 0 )	// no items
			||	CutsceneManager.Instance.IsOK == false							// CutsceneManager is not avaiable
//			||	m_IsCompleted == true											// already executed
			)
			{
				if ( m_EndSequenceCallback != null )
					m_EndSequenceCallback.Invoke();
				return;
			}

			// If another sequence is playing, stop it
			if ( CutsceneManager.Instance.CurrentSequence != null )
				CutsceneManager.Instance.CurrentSequence.InterruptSequence();

			// set first frame as current in play
			m_CurrentItemIdx = 0;

			{	// Sanity check
				SequencePair nextSequencePair = NextValidFrame();
				if ( nextSequencePair == null )
				{
					Destroy( gameObject );
					return;
				}

				m_CurrentFramePair = nextSequencePair;
			}

			GameManager.instance.ChangeState( GAMESTATE.CUTSCENE );

			CutsceneManager.Instance.CurrentSequence = this;

			// Set text for speaker and message
			CutsceneManager.Instance.ShowInterface();
			CutsceneManager.Instance.InterfaceSpeaker.text	= m_CurrentFramePair.SequenceFrame.SpeakerName;

			// WRITE text in message text field
			StartCoroutine( WriteTextCO( CutsceneManager.Instance.InterfaceMessage, m_CurrentFramePair.SequenceFrame.Text ) );

			// set current audioRef for play
			if ( m_CurrentFramePair.SequenceFrame.AudioRef != null && m_CurrentFramePair.SequenceFrame.AudioRef.Length > 0 )
			{
				// set audioclip for audiosource
				m_CurrentAudioSource	= CutsceneManager.Instance.GetEventEmitter( Source );
                //m_CurrentAudioSource.Stop();
                //m_CurrentAudioSource.Event = m_CurrentFramePair.SequenceFrame.AudioRef;
                //m_CurrentAudioSource.Play();

                if (m_RuntimeAudioSource)
                { 
                    m_RuntimeAudioSource.Stop();
                    Destroy(m_RuntimeAudioSource);
                }
                m_RuntimeAudioSource = m_CurrentAudioSource.gameObject.AddComponent<StudioEventEmitter>();
                m_RuntimeAudioSource.Event = m_CurrentFramePair.SequenceFrame.AudioRef;
                m_RuntimeAudioSource.Play();
			}
		}


		//////////////////////////////////////////////////////////////////////////
		// NextItem
		private	void	NextFrame()
		{	
			// stop all coroutines
			StopAllCoroutines();

			if (  m_RuntimeAudioSource.IsPlaying() )
				 m_RuntimeAudioSource.Stop();
			
			// set current frame as executed
			if ( m_SequenceFrames[ m_CurrentItemIdx ].OnFrameEnd != null )
				m_SequenceFrames[ m_CurrentItemIdx ].OnFrameEnd.Invoke( m_CurrentAudioSource.transform.parent.gameObject );

			m_SequenceFrames[ m_CurrentItemIdx ].SequenceFrame.IsExecuted = true;

			// increase index
			m_CurrentItemIdx ++;

			// sequence is finished
			if ( m_CurrentItemIdx == m_SequenceFrames.Length )
			{
				// hide interface
				CutsceneManager.Instance.HideInterface();

				// set this sequence as finished
				m_IsCompleted = true;

				// Set current game state to INGAME state
				GameManager.instance.ChangeState( GAMESTATE.INGAME );

				// remove reference to this sequence
				CutsceneManager.Instance.CurrentSequence = null;

				// On Sequance Finished callback
				if ( m_OnSequnceFinished != null )
					m_OnSequnceFinished.Invoke( m_CurrentAudioSource.transform.parent.gameObject );
				return;
			}

			{	// Sanity check
				SequencePair nextSequencePair = NextValidFrame();
				if ( nextSequencePair == null )
					return;

				// set next frame as current in play
				m_CurrentFramePair = nextSequencePair;
			}

			// set current frame speaker and text
			CutsceneManager.Instance.InterfaceSpeaker.text	= m_CurrentFramePair.SequenceFrame.SpeakerName;
			StartCoroutine( WriteTextCO( CutsceneManager.Instance.InterfaceMessage, m_CurrentFramePair.SequenceFrame.Text ) );

			// set current audioRef for play
			if ( m_CurrentFramePair.SequenceFrame.AudioRef != null && m_CurrentFramePair.SequenceFrame.AudioRef.Length > 0 )
			{
				// set audioclip for audiosource
				m_CurrentAudioSource	= CutsceneManager.Instance.GetEventEmitter( Source );
                //m_CurrentAudioSource.Stop();
                //m_CurrentAudioSource.Event = m_CurrentFramePair.SequenceFrame.AudioRef;
                //m_CurrentAudioSource.Play();

                if (m_RuntimeAudioSource)
                {
                    m_RuntimeAudioSource.Stop();
                    Destroy(m_RuntimeAudioSource);
                }
                m_RuntimeAudioSource = m_CurrentAudioSource.gameObject.AddComponent<StudioEventEmitter>();
                m_RuntimeAudioSource.Event = m_CurrentFramePair.SequenceFrame.AudioRef;
                m_RuntimeAudioSource.Play();
            }
		}


		//////////////////////////////////////////////////////////////////////////
		// ParseString
		private	float	ParseString( string text, ref int currentPosition, int maxLength )
		{
			int endValue = text.IndexOf( ']' );

			// contains at last "w[n]"
			if ( text.Length > 3 && text[0] == 'w' && text[1] == '[' && endValue > 0 )
			{
				// set cursor position to new position
				currentPosition += endValue;
				if ( currentPosition < maxLength )
					currentPosition += 1;

				string waitValue = text.Substring( 2, endValue - 2 );
				return float.Parse( waitValue );
			}

			return 0.0f;
		}


		//////////////////////////////////////////////////////////////////////////
		// WriteTextCO ( Coroutine )
		private	IEnumerator	WriteTextCO( TextMeshPro container, string text )
		{
			// set start cursor position
			int currentPosition = 0;

			// empty text container
			container.text = "";

			// save current game time
//			float frameCurrentTime = Time.time;

			// 'Write' the text
			while ( currentPosition < text.Length )
			{
				// search for wait commands
				float timeToWait = ParseString( text.Substring( currentPosition ), ref currentPosition, text.Length );
				if ( timeToWait > 0.0f )
				{
					yield return new  WaitForSeconds ( timeToWait );
				}

				if ( currentPosition < text.Length )

					// add char to text string
					container.text += text[ currentPosition ];

				//increase cursor position
				currentPosition ++;

				// wait for next char
				yield return new  WaitForSeconds ( CutsceneManager.Instance.CharsDelay );
			}

			while(  m_RuntimeAudioSource.IsPlaying() )
				yield return null;

			/*
			// time decided by by sequence frame
			float delay = m_CurrentFramePair.SequenceFrame.Duration > 0 ? m_CurrentFramePair.SequenceFrame.Duration : 0f;

			// time elapsed from the start of the frame
			frameCurrentTime = Time.time - frameCurrentTime;

			// remaining time to wait
			delay -= frameCurrentTime;
			if ( delay  > 0 )
				yield return new  WaitForSeconds ( delay );
				*/
			// next sequence frame
			this.NextFrame();
		}


		//////////////////////////////////////////////////////////////////////////
		// SkipFrame
		public void SkipFrame()
		{
			// Call for next frame or sequence end
			NextFrame();
		}


		//////////////////////////////////////////////////////////////////////////
		// InterruptSequence
		public	void	InterruptSequence()
		{
			if ( ( m_SequenceFrames == null || m_SequenceFrames.Length == 0 ) )
				return;

			// stop all coroutines
			StopAllCoroutines();

			// hide interface
			CutsceneManager.Instance.HideInterface();

            //m_CurrentAudioSource.Stop();
            m_RuntimeAudioSource.Stop();
            Destroy(m_RuntimeAudioSource);

			// set this sequence as finished
			m_IsCompleted = true;
			
			// execute all remaining actions
			for ( int i = m_CurrentItemIdx+1; i < m_SequenceFrames.Length; i++ )
			{
				if ( m_SequenceFrames[ i ].OnFrameEnd != null )
					m_SequenceFrames[ i ].OnFrameEnd.Invoke( m_CurrentAudioSource.transform.parent.gameObject );
			}
			
			// On Seqquence Interruption callback
//			if ( m_OnSequnceInterrupted != null )
//				m_OnSequnceInterrupted.Invoke( m_CurrentAudioSource.transform.parent.gameObject );
			
			// Set current game state to INGAME state
			GameManager.instance.ChangeState( GAMESTATE.INGAME );

			// remove reference to this sequence
			CutsceneManager.Instance.CurrentSequence = null;
		}

	}

}
