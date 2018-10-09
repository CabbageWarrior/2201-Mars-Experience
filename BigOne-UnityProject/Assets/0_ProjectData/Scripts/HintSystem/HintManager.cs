// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;
using HintSystem;

public interface IHintManager {

	RoomSection[]		RoomSections { get; }

	bool				IsEnabled { get; set; }

	Hint				HintRequest( int level );

	void				_SetAsSent	( HintCommon hintCommon );

}


public class HintManager : MonoBehaviour, IHintManager {

	public bool InDebug = false;

	private	static	IHintManager		m_Interface		= null;
	/// <summary> Hint Manager interface </summary>
	public	static	IHintManager	Interface {
		get {
			return m_Interface;
		}
	}

	private	RoomSection[]		m_RoomSections			= null;
	/// <summary> All the room sections </summary>
	public	RoomSection[]		RoomSections
	{
		get { return m_RoomSections; }
	}


	public	bool	IsEnabled
	{
		get;
		set;
	}


	private		VRTK_InteractGrab		m_RightGrabber	= null;
	private		VRTK_InteractGrab		m_LeftGrabber	= null;

	//////////////////////////////////////////////////////////////////////////
	// AWAKE
	private	void	Awake()
	{
        // STATIC REF
        m_Interface = this as IHintManager;

        // RETRIEVE ALL THE ROOM SECTIONS
        if ( m_RoomSections == null || m_RoomSections.Length == 0 )
			m_RoomSections = FindObjectsOfType<RoomSection>();

		// get controller interact component reference
		m_RightGrabber = PlayerCommon.Instance.RightController.GetComponent<VRTK_InteractGrab>();
		m_LeftGrabber  = PlayerCommon.Instance.LeftController. GetComponent<VRTK_InteractGrab>();

	}

	//////////////////////////////////////////////////////////////////////////
	// HintRequest
	/// <summary> level can be 0, 1, 2 </summary>
	public Hint	HintRequest( int level = -1 )
	{
		if ( InDebug ) print( "Hint requested" );
		if ( IsEnabled == false )
			return null;

		// check for object in hands ( right first then left )
		{   // RIGHT GRABBED OBJECT
			Hint hint = GetControllerHint( m_RightGrabber.GetGrabbedObject(), level );
			if ( hint != null )
			{
				return hint;
			}
		}
		{	// LEFT GRABBED OBJECT
			Hint hint = GetControllerHint( m_LeftGrabber.GetGrabbedObject(), level );
			if ( hint != null )
			{
				return hint;
			}
		}

		// check for puzzle in current section
		{	// SEARCH VALID HINT BY SECTION
			RoomSection currentSection = System.Array.Find( m_RoomSections, s => s.IsPlayerInside );
			if ( currentSection == null )
			{
				Debug.Log( "WARNING: HintManager::HintRequest(): section not found" );
				return null;
			}
				
			// for every cycle
			foreach( HintCycle cycle in currentSection.HintCycles )
			{
				if ( cycle.IsSent == false )
				{
					if ( InDebug ) print( "Iteration of Cycle " + cycle.Name );

					Hint hint = GetLastValidHint( cycle, level );
					if ( hint )
					{
						return hint;
					}
				}
			}
		}

		Debug.Log( "WARNING: HintManager::HintRequest(): no valid hint found !!" );
		return null;
	}

		
	//////////////////////////////////////////////////////////////////////////
	// _SetAsSent
	public void	_SetAsSent( HintCommon hintCommon )
	{
		hintCommon.SetAsSent();
	}
	

	//////////////////////////////////////////////////////////////////////////
	// GetLastValidHint
	private	Hint	GetLastValidHint( HintCycle cycle, int level = -1 )
	{
		if ( InDebug ) print( "GetLastValidHint()" );
		// for each puzzle
		foreach( HintPuzzle puzzle in cycle.Childs )
		{	if ( InDebug ) print( "Iteration of Puzzle " + puzzle.Name );
			// if not already sent
			if ( puzzle.IsSent == false )
			{	// for each group
				foreach( HintGroup group in puzzle.Childs )
				{	if ( InDebug ) print( "Iteration of Group " + group.Name );
					// if not already sent
					if ( group.IsSent == false )
					{
						for ( int i = 0; i < group.Childs.Count; i++ )
						{
							if ( level != -1 )
							{
								return group.Childs[level] as Hint;
							}

							Hint hint = group.Childs[i] as Hint;
							if ( InDebug ) print( "Iteration of Hint " + hint.Name );
							// Always return the last hint
							if ( i == group.Childs.Count - 1 ) return hint;
							if ( hint.IsSent == true ) continue;
							hint.SetAsSent();
							return hint;
						}
					}
				}
			}
		}

		return null;

	}


	//////////////////////////////////////////////////////////////////////////
	// GetControllerHint
	private	Hint	GetControllerHint( GameObject grabbedObject, int level )
	{
		if ( grabbedObject != null )
		{
			PuzzleItem item = grabbedObject.GetComponent<PuzzleItem>();
			if ( item )
			{
				if ( item.hintCycle != null )
				{
					if( !item.hintCycle.IsSent )
						return GetLastValidHint( item.hintCycle, level );
				}
				else
				{
					Debug.Log( "WARNING: HintManager::HintRequest(): No hintCycle for item " + item.name );
				}
			}
		}
		return null;
	}

}