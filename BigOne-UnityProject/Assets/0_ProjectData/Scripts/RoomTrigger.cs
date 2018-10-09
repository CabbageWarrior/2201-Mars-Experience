// Scripted by Roberto Leogrande

using UnityEngine;

public class RoomTrigger : MonoBehaviour {

    //	[SerializeField][FMODUnity.EventRef]
    //	private	string	m_EventName = "";
    [SerializeField]
    private Cutscene.CutsceneSequence m_Sequence = null;

	[SerializeField]
	private	GameEvent	m_OnEnter		= null;

	[SerializeField]
	private	GameEvent	m_OnExit		= null;


//	private	FMOD.Studio.EventInstance		m_LogInstance;
	private bool		m_IsAlreadyTriggered = false;


	private	bool	_IsOk = false;


	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start()
	{
		if ( m_Sequence == null  )
		{
			enabled = false;
			Debug.LogError( "Attenzione: serve abbinare la sequenza da riprodurre nella stanza " + name );
			return;
		}
		_IsOk = true;
//		m_LogInstance = FMODUnity.RuntimeManager.CreateInstance( m_EventName );
	}


	//////////////////////////////////////////////////////////////////////////
	// OnDestroy
	private void OnDestroy()
	{
//		m_LogInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
//		m_LogInstance.release();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnTriggerEnter
	private void OnTriggerEnter( Collider other )
	{
		if ( _IsOk == false )
			return;

		if ( other.name == "PlayerTracker" )
		{
			if ( m_IsAlreadyTriggered == false )
			{
				m_IsAlreadyTriggered = true;
                m_Sequence.Play();
			}
			if ( m_OnEnter != null && m_OnEnter.GetPersistentEventCount() > 0 )
				m_OnEnter.Invoke();
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnTriggerExit
	private void OnTriggerExit( Collider other )
	{
		if ( _IsOk == false )
			return;

		if ( other.name == "PlayerTracker" )
		{
			if ( m_OnExit != null && m_OnExit.GetPersistentEventCount() > 0 )
				m_OnExit.Invoke();
		}
	}

}
