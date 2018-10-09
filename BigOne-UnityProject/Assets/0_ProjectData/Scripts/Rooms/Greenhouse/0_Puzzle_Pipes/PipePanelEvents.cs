// Scripted by Roberto Leogrande

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;
using VRTK.UnityEventHelper;
using VRTK.Controllables;
using FMODUnity;
#pragma warning disable CS0618 // Il tipo o il membro è obsoleto


public class PipePanelEvents : Puzzle {

	// Events
	public GameEvent				m_OnLitOpenend			= null;
	public GameEvent				m_OnLitClosed			= null;
    public GameEvent                m_OnPipeConnected       = null;


    // puzzle logic elements
    private	int						m_currentLinkedPipes	= 0;
	private	Transform				m_Button				= null;
	private	Animator				m_PanelAnimator			= null;
	private	Animator				m_ButtonAnimator		= null;
	private	bool					m_PanelOpened			= false;
	private	bool					m_PanelInTransition		= false;
	public	bool					PanelInTransition
	{
		set { m_PanelInTransition = value; }
	}

    [Space]
    public StudioEventEmitter LidButtonEmitter;

	//////////////////////////////////////////////////////////////////////////
	// START
	protected void Start()
	{
		m_Button	= transform.Find( "button" );
		m_PanelAnimator	= GetComponent<Animator>();
		m_ButtonAnimator = m_Button.GetComponent<Animator>();

    }


	//////////////////////////////////////////////////////////////////////////
	// PANEL EVENTS
	public	void	OnLitOpenend()	    {	m_PanelInTransition = false; 	m_OnLitOpenend.Invoke(); }
	public	void	OnLitClosed()	    {	m_PanelInTransition = false;	CheckForCompletion();	m_OnLitClosed.Invoke();  }
	public	void    OnPipeConnected()
    {
        if (m_OnPipeConnected != null)
            m_OnPipeConnected.Invoke();
    }
	public	void	OnButtonPressed()
	{
		if ( m_PanelInTransition )
			return;

		m_PanelOpened = !m_PanelOpened;
		m_ButtonAnimator.Play( "OnPressed" );
		m_PanelAnimator.Play( m_PanelOpened ? "Open" : "Close" );
		m_PanelInTransition = true;
        LidButtonEmitter.Play();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnPipeAdd
	public	void	OnPipeAdd( Pipe pipe )
	{
		m_currentLinkedPipes++;

        // Chiamo l'evento di snap del pipe.
        OnPipeConnected();

        // se tutti i tubi sono stati collegati invoca OnPipeSet
        if ( m_currentLinkedPipes == 4 )
		{
			// OnCompletion callback
			OnCompletion();
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// CheckForCompletion
	private void CheckForCompletion()
	{
		if ( IsCompleted )
		{
			m_Button.GetComponent<VR_UI_Interactable_Button>().IsEnabled = false;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnStateSwitch ( Overrider )
	protected override void OnStateSwitch( GAMESTATE currentState, GAMESTATE nextState )
	{

		base.OnStateSwitch( currentState, nextState );

		// insert here your customization
	}


	//////////////////////////////////////////////////////////////////////////
	// OnSave ( Overrider )
	protected override void OnSave( SaveLoadUtilities.PermanentGameData data )
	{

		// insert here your customization

		base.OnSave( data );

	}


	//////////////////////////////////////////////////////////////////////////
	// OnLoad ( Overrider )
	protected override void OnLoad( SaveLoadUtilities.PermanentGameData data )
	{
		base.OnLoad( data );

		if ( IsCompleted )
		{
			m_Button.GetComponent<VR_UI_Interactable_Button>().IsEnabled = false;

			var pipes = FindObjectsOfType<Pipe>();
			foreach( Pipe pipeObject in pipes )
			{
				Destroy( pipeObject );
			}

			OnCompletion();

			// do things to emulate puzzle completion
		}

		// insert here your customization
	}
	

}

#pragma warning restore CS0618 // Il tipo o il membro è obsoleto
