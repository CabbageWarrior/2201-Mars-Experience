// Scripted by Roberto Leogrande

using UnityEngine;
using Cutscene;

using System;

[System.Serializable]
public class GameEvent      : UnityEngine.Events.UnityEvent { }
[System.Serializable]
public class GameEventArg1	: UnityEngine.Events.UnityEvent< UnityEngine.Object > { }
[System.Serializable]
public class GameEventArg2	: UnityEngine.Events.UnityEvent< UnityEngine.Object, UnityEngine.Object > { }
[System.Serializable]
public class GameEventArg3	: UnityEngine.Events.UnityEvent< UnityEngine.Object, UnityEngine.Object, UnityEngine.Object > { }
[System.Serializable]
public class GameEventArg4	: UnityEngine.Events.UnityEvent< UnityEngine.Object, UnityEngine.Object, UnityEngine.Object, UnityEngine.Object > { }




// TODO Currently game saves puzzle info into file "SaveFile.txt" as jason, need encryption



//	DELEGATES FOR EVENTS
public delegate void GameStateSwitchEvent( GAMESTATE currentState, GAMESTATE nextState );
public delegate void PauseEvent();

// INTERFACE
/// <summary> Clean interface of only GameManager class </summary>
public interface IGameManager {

	// PROPERTIES

	/// <summary> Store current game state </summary>
	GAMESTATE	CurrentGameState { get; }

	/// <summary> get or set ithe the game can be puased </summary>
	bool		CanPause { get; set; }
	
	/// <summary> Return if game is currently paused </summary>
	bool		IsPaused { get; }


	// EVENTS

	/// <summary> Event called when switching to a state </summary>
	event GameStateSwitchEvent	OnGameStateSwitchEvent;

	/// <summary> Events called when pause state is activated </summary>
	event PauseEvent			OnPauseEnable;


	// METHODS

	/// <summary> Evaluate switch authorizations and then, if possible, switch game state </summary>
	void	ChangeState( GAMESTATE nextGameState );

	/// <summary> Used to send pause request to Game Manager, return result as boolean </summary>
	bool	PauseRequest( bool forcedPause );



}


public enum GAMESTATE {
	// INTRO,
	MAINMENU,
	CUTSCENE,
	PAUSEMENU,
	INGAME
	// OUTRO
}

[RequireComponent( typeof( CutsceneManager ) ) ]
[RequireComponent( typeof( PuzzleManager ) ) ]
[RequireComponent( typeof( HintManager ) ) ]

// CLASS
/// <summary> Used to manage general aspects of the game </summary>
public partial class GameManager : MonoBehaviour, IGameManager {

    /// <summary> Use this to identity is executing in editor or in build </summary>
#if UNITY_EDITOR
    public const bool InEditor = true;
#else
	public	const	bool InEditor = false;
#endif

    /// <summary> store the only one instance of the game manager without MonoBehaviour properties and methods </summary>
    public	static IGameManager instance;
	/// <summary> interface use to save and load </summary>
	public	static IGameManager_SaveManagement SaveManagemnt
	{
		get { return instance as IGameManager_SaveManagement; }
	}

	/// <summary> Event called when switching to a state </summary>
    public event GameStateSwitchEvent	OnGameStateSwitchEvent		= null;

    /// <summary> Events called when pause state is activated </summary>
    public event PauseEvent				OnPauseEnable				= null;


	private	bool	m_PlayingHint			= false;
	private	float	m_GC_Timer				= 0f;

	//////////////////////////////////////////////////////////////////////////
	// AWAKE
    private void Awake()
	{
        // STATIC REF
        instance = this as IGameManager;
        DontDestroyOnLoad(this);

		// Check if save files exist
		CheckForSaves();

		CutsceneManager thisCutsceneManager		= GetComponent<CutsceneManager>();
		PuzzleManager thisPuzzleManager			= GetComponent<PuzzleManager>();
		HintManager thisHintManager				= GetComponent<HintManager>();

		if ( thisCutsceneManager == null )		gameObject.AddComponent<CutsceneManager>();

		if ( thisPuzzleManager == null )		gameObject.AddComponent<PuzzleManager>();

		if ( thisHintManager == null )			gameObject.AddComponent<HintManager>();

    }

    #region GAME STATE
	
    private GAMESTATE m_CurrentGameState = GAMESTATE.MAINMENU;
    /// <summary> Store current game state </summary>
    public GAMESTATE CurrentGameState
	{
        get { return m_CurrentGameState; }
    }


	//////////////////////////////////////////////////////////////////////////
	// ChangeState ( Arg[0] = string )
	public	void	ChangeState( string nextStrateString )
	{
		try {
			GAMESTATE nextState = ( GAMESTATE ) System.Enum.Parse( typeof( GAMESTATE ), nextStrateString.ToUpper() );        
			if ( System.Enum.IsDefined( typeof( GAMESTATE ), nextState ) )
			{
				ChangeState( nextState );
				return;
			}			
		}
		catch ( Exception ) {}
	}


	//////////////////////////////////////////////////////////////////////////
	// ChangeState ( Arg[0] = enum GAMESTATE )
    /// <summary> Evaluate switch authorizations and then, if possible, switch game state </summary>
    public void ChangeState( GAMESTATE nextGameState )
	{
        // If there are callback call them
        OnGameStateSwitchEvent( m_CurrentGameState, nextGameState );

		m_CurrentGameState = nextGameState;
    }


    #endregion


    #region PAUSE

    // PAUSE
    private float m_BeforePauseTimeScale = 1.0f;
    private bool m_CanPause = true;
    /// <summary> get or set ithe the game can be puased </summary>
    public bool CanPause
	{
        get { return m_CanPause; }
		set { m_CanPause = value; }
    }
    private bool m_isPaused = false;
    /// <summary> Return if game is currently paused </summary>
    public bool IsPaused
	{
        get { return m_isPaused; }
    }


	//////////////////////////////////////////////////////////////////////////
	// PauseRequest
    /// <summary> Used to send pause request to Game Manager, return result as boolean </summary>
    public bool PauseRequest( bool forcedPause = false )
	{
        // return if cannot be paused
        if ( !m_CanPause && !forcedPause )
			return false;

        // Invoke callback when pused
        OnPauseEnable();

        if ( !m_isPaused )
		{
            // set internal pause state
            m_isPaused = true;

            // save current time scale
            m_BeforePauseTimeScale = Time.timeScale;

            // set current time scale to zero
            Time.timeScale = 0.0f;
        }

        return true;
    }

    #endregion




	#region HINT REQUEST

	public	void HintRequest( int level = -1 )
	{
		if ( m_PlayingHint == true )
			return;

		HintSystem.Hint hint = HintManager.Interface.HintRequest( level );
		if ( hint != null )
		{
			 print( "GameManager::HintRequest:: Hint Found, sending hint " + hint.Father.Name + "_" + hint.Name );

			// Create sequence frame
			SequenceFrame sequenceFrame = ScriptableObject.CreateInstance<SequenceFrame>();
			sequenceFrame.SpeakerName	= "M.A.R.I.A.";
			sequenceFrame.Text			= hint.Text;
			sequenceFrame.AudioRef		= hint.EventRef;
//			sequenceFrame.Audio			= hint.ClipSound;
			sequenceFrame.Duration		= Mathf.Infinity;

			// Create sequence frames
			SequencePair[] frames = new SequencePair[1]
			{
				new SequencePair() { SequenceFrame = sequenceFrame }
			};
			
			// Create sequence
			CutsceneSequence sequence = gameObject.AddComponent<CutsceneSequence>();
			sequence.Build( frames )
				.SetEndCallback( () => { m_PlayingHint = false; Destroy( sequenceFrame ); Destroy ( sequence ); } )
				.Play();
			m_PlayingHint = true;
			return;
		}

		print( "GameManager::HintRequest:: Hint not found !!" );
	}

	#endregion


	//////////////////////////////////////////////////////////////////////////
	// UNITY
	private void Update()
	{
		
		if ( Input.GetKeyDown( KeyCode.H ) )
		{
			HintRequest();
		}

        if ( Input.GetKey( KeyCode.LeftControl ) && Input.GetKeyDown( KeyCode.T ) )
		{
            print("Saving puzzle and sections data");
            Save("");
        }

		m_GC_Timer -= Time.deltaTime;
		if ( m_GC_Timer < 0f )
		{
			System.GC.Collect( 2 );
			m_GC_Timer = 5f;
		}

        if (Input.GetKeyDown(KeyCode.Escape))
		{
            if ( InEditor )
                // Commented for debugging stuff
                //UnityEditor.EditorApplication.isPlaying = false;

                Debug.Log("[[[ YOU PRESSED THE \"CLOSE APPLICATION\" BUTTON!!!!!!!! ]]]");
            else
#pragma warning disable CS0162 // È stato rilevato codice non raggiungibile
                Application.Quit();
#pragma warning restore CS0162 // È stato rilevato codice non raggiungibile
        }
    }


	private void OnApplicationPause( bool pause )
	{
		
		if ( pause == true )
		{
			
		}
		else
		{

		}

	}
}

