using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Cutscene;


public class AudioLogOnline : MonoBehaviour {

	[SerializeField]
	private	GameEvent				m_OnGrabEvent				= null;

	[SerializeField]
	private	CutsceneSequence		m_LogSequence				= null;
	public	CutsceneSequence		LogSequence
	{
		get { return m_LogSequence; }
	}


	private	VRTK_InteractableObject m_Interactable				= null;


	
	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start ()
	{
		m_Interactable = GetComponent<VRTK_InteractableObject>();
		if ( m_Interactable == null || m_LogSequence == null )
		{
			print( "WARNING:AudioLogOnline::Start: object " + name + " does not have VRTK_InteractableObject component !!" );
			Destroy( gameObject );
			return;
		}

		m_Interactable.InteractableObjectGrabbed += InteractableObjectGrabbed;
        m_Interactable.isGrabbable = true;
        if (this.gameObject.name != "AudioLog2")
        {
            GetComponent<GrabbableAtDistance>().IsAllowed = true;
        }

		GameManager.SaveManagemnt.OnLoad += OnLoad;
	}


	//////////////////////////////////////////////////////////////////////////
	// InteractableObjectGrabbed
	private void InteractableObjectGrabbed( object sender, InteractableObjectEventArgs e )
	{
		// Add this log to player collection
		PlayerCommon.Instance.AddAudioLog( this );

		if ( m_OnGrabEvent != null && m_OnGrabEvent.GetPersistentEventCount() > 0 )
			m_OnGrabEvent.Invoke();
		
		GameObject go = m_Interactable.GetGrabbingObject();
		VRTK_InteractGrab grab = go.GetComponent<VRTK_InteractGrab>();
		grab.ForceRelease();

		// Stop interacting and disable interactions
		m_Interactable.ForceStopInteracting();
		m_Interactable.InteractableObjectGrabbed -= InteractableObjectGrabbed;

		// Start hiding coroutine
		HideObject();
	}

	                              
	//////////////////////////////////////////////////////////////////////////
	// HideObject ( Coroutine )
	private	void	HideObject()
	{
		foreach( Component c in GetComponents<Component>() )
		{
			if ( c is CutsceneSequence || c is Transform || c is AudioLogOnline )
				continue;
			Destroy( c );
		}
		
		// set object position to an hidden location
		transform.position = Vector3.down * 999f;

		// Play the sequence
		m_LogSequence.Play();

		Destroy( m_Interactable );
		Destroy( transform.GetChild( 0 ).gameObject );
		Destroy( this );
	}


	//////////////////////////////////////////////////////////////////////////
	// OnLoad
	private	void	OnLoad( SaveLoadUtilities.PermanentGameData loadData )
	{
		// Already added, so destroy this object
		AudioLogOnline audioLog = loadData.FindAudioLog( this );
		if ( audioLog != null )
		{
			HideObject();
		}
	}

	private void OnDestroy()
	{
		GameManager.SaveManagemnt.OnLoad -= OnLoad;
	}

	// Grab management of Audiolog2 by Giulia
	public void SetGrabbable(bool makeGrabbable)
	{
		m_Interactable.isGrabbable = makeGrabbable;
	}

}
