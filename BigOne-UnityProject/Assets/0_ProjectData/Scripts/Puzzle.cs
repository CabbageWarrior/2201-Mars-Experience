// Scripted by Roberto Leogrande

using UnityEngine;
using HintSystem;


public interface IPuzzle_setter {

	/// <summary> CAUTION </summary>
	bool		IsCompleted { set; }

}

public interface IPuzzle {

	/// <summary> Return the current puzzle state </summary>
	bool		IsCompleted { get; }

	HintPuzzle	HintPuzzle { get; }

	/// <summary> Used to update internally puzzle logic and status check </summary>
	void		OnCompletion();
	void		OnError();

}

/// <summary> Mother of puzzle managers offer interface for Save, Load and gamestate switch for puzzles </summary>
public abstract class Puzzle : MonoBehaviour, IPuzzle, IPuzzle_setter {

	public	IPuzzle			Interface			= null;
	public	IPuzzle_setter	Internal			= null;

	[SerializeField]
	private	HintPuzzle		m_HintPuzzle		= null;
	public	HintPuzzle		HintPuzzle
	{
		get { return m_HintPuzzle; }
	}


	/// <summary> Used to update internally puzzle logic and status check </summary>
	[SerializeField]
	private	GameEvent		m_OnCompletion		= null;
	
		/// <summary> Used to update internally puzzle logic and status check </summary>
	[SerializeField]
	private	GameEvent		m_OnError			= null;

	private		bool		m_Completed			= false;
	/// <summary> Return the current puzzle state </summary>
	public		bool		IsCompleted
	{
		get { return m_Completed; }
	}
	// hidden setter
	bool IPuzzle_setter.IsCompleted
	{
		set { m_Completed = value; }
	}


	//////////////////////////////////////////////////////////////////////////
	// AWAKE
	protected virtual void Awake()
	{
		Interface	= this as IPuzzle;
		Internal	= this as IPuzzle_setter;

		// register game manager callbacks
		GameManager.SaveManagemnt.OnSave					+= OnSave;
		GameManager.SaveManagemnt.OnLoad					+= OnLoad;
		GameManager.instance.OnGameStateSwitchEvent += OnStateSwitch;
	}
	
	//////////////////////////////////////////////////////////////////////////
	/// <summary> Used to update internally puzzle logic and status check </summary>
	public void	OnCompletion()
	{
		if ( m_Completed == true )
			return;

		m_Completed = true;

		if ( m_OnCompletion != null )
		{
			m_OnCompletion.Invoke();
		}
	}
	public void	OnError()
	{
		if ( m_OnError != null )
		{
			m_OnError.Invoke();
		}
	}




	//////////////////////////////////////////////////////////////////////////

	protected virtual void OnSave( SaveLoadUtilities.PermanentGameData saveData )
	{
		// collect data from this puzzle and add it into puzzleInfo stream
		SaveLoadUtilities.PuzzleInfo puzzleInfo = new SaveLoadUtilities.PuzzleInfo( gameObject.name, m_Completed );
		saveData.SavePuzzleInfo( puzzleInfo );
	}

	protected virtual void OnLoad( SaveLoadUtilities.PermanentGameData loadData )
	{
		// get puzzle data
		SaveLoadUtilities.PuzzleInfo puzzleInfo = loadData.GetPuzzleInfo( gameObject.name );

		if ( puzzleInfo == null )
		{
			print( gameObject.name + " - OnLoad: Cannot retrieve puzzle data" );
			return;
		}
		
		// set puzzle data
		m_Completed = puzzleInfo.Completed;
	}

	protected virtual void OnStateSwitch( GAMESTATE currentState, GAMESTATE nextState )
	{

	}


}
