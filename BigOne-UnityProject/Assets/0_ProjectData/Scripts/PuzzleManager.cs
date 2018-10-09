// Scripted by Roberto Leogrande

using UnityEngine;

// INTERFACE
public interface  IStateManager {

	int			PuzzleCount { get; }

	/// <summary> Used to set a s completed a puzzle </summary>
	void		__SetAsCompleted( Puzzle p );

}

/// <summary> Class that manage the structure of all areas and relative puzzles </summary>
public class PuzzleManager : MonoBehaviour, IStateManager {

	private	static	IStateManager m_Interface	= null;
	public	static	IStateManager Interface
	{
		get { return m_Interface; }
	}

	public	int			PuzzleCount
	{
		get { return  ( vPuzzles != null ) ? vPuzzles.Length : 0; }
	}

//	[SerializeField]
	private	Puzzle[]		vPuzzles			= null;


	//////////////////////////////////////////////////////////////////////////
	// AWAKE
	private void Awake()
	{
        // STATIC REF
        m_Interface = this;

		m_Interface = this;
		DontDestroyOnLoad( this );
	}


	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start()
	{
		vPuzzles = FindObjectsOfType<Puzzle>();
	}


	//////////////////////////////////////////////////////////////////////////
	// __SetAsCompleted
	/// <summary> Used to set a s completed a puzzle </summary>
	public	void	__SetAsCompleted( Puzzle p )
	{
		if ( p == null || p.IsCompleted  )
		{
			return;
		}

		// call OnCompletion callback
		p.Internal.IsCompleted = true;
		p.OnCompletion();
	}
		
}