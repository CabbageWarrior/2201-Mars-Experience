// Scripted by Roberto Leogrande

using UnityEngine;
using HintSystem;

[RequireComponent( typeof( BoxCollider ) ) ]
/// <summary> Class used to manage area events as when player enter, exit or complete all puzzle inside it </summary>
public class RoomSection : MonoBehaviour {

	private static RoomSection	m_PreviousRoomSection = null;

	[SerializeField]
	private bool				m_ShowSections		= false;

	[SerializeField]
	private	GameEvent			m_OnEnter			= null;

	[SerializeField]
	private	GameEvent			m_OnExit			= null;

	[SerializeField]
	private	HintCycle[]			m_HintCycles		= null;
	public	HintCycle[]			HintCycles
	{
		get { return m_HintCycles; }
	}

	private	bool				m_IsPlayerInside	= false;
	public	bool				IsPlayerInside
	{
		get { return m_IsPlayerInside; }
	}


	//////////////////////////////////////////////////////////////////////////
	// AWAKE
	private void Awake()
	{
		if ( m_HintCycles.Length == 0 ) {
			print( "WARNING: Room Section \"" + name + "\" have not hint cycle scriptable objects attached !!" );
			return;
		}

		GameManager.SaveManagemnt.OnSave += OnSave;
		GameManager.SaveManagemnt.OnLoad += OnLoad;

		// Set its collider as trigger
		GetComponent<Collider>().isTrigger = true;

		// Ensure scriptsbles reset
		foreach ( HintCycle cycle in m_HintCycles )
		{
			cycle.Reset();
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnSave
	private	void OnSave( SaveLoadUtilities.PermanentGameData saveData )
	{
		saveData.SaveRoomSection( m_HintCycles );
	}


	//////////////////////////////////////////////////////////////////////////
	// OnLoad
	private	void OnLoad( SaveLoadUtilities.PermanentGameData loadData )
	{
		foreach( HintCycle onlineCycle in m_HintCycles )
		{
			HintCycle loadedCycle = loadData.GetHintCycle( onlineCycle.Path );
			if ( loadedCycle == null )
			{
				print( "WARNING: Room Section " + gameObject.name + " - OnLoad: Cannot retrieve hint cycle data with path " + onlineCycle.Path );
				continue;
			}

			onlineCycle.Set( loadedCycle );
		}
	}


	/////////////////////////////////////////////////////////////////////////////////////////////////
	// UNITY
	private void OnTriggerEnter( Collider other )
	{
		if ( other.name == "PlayerTracker" )
		{
			if ( m_PreviousRoomSection != null )
			{
				// player is in the same "room"
				if ( m_PreviousRoomSection == this )
					return;

				// coming from different room
				if ( m_PreviousRoomSection != this )
				{
					m_PreviousRoomSection.m_OnExit.Invoke();
					m_PreviousRoomSection.m_IsPlayerInside = false;
				}
			}

			// Player enter a new room
			this.m_IsPlayerInside = true;
			this.m_OnEnter.Invoke();
			m_PreviousRoomSection = this;
		}
	}

	private void OnDrawGizmos()
	{
		if ( m_ShowSections == false )
			return;

		foreach ( BoxCollider c in GetComponentsInChildren<BoxCollider>() )
		{
			if ( c.enabled )
			{
				Gizmos.matrix = c.transform.localToWorldMatrix;
				Gizmos.DrawCube( c.center, c.size );
			}
		}
	}

	private void OnApplicationQuit()
	{
		foreach ( HintCycle cycle in m_HintCycles )
		{
			cycle.Reset();
		}
	}

}
