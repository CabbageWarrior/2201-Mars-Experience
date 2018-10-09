using UnityEngine;
using System.Collections.Generic;
using VRTK;

public interface IPlayerCommon {

	GameObject						LeftController				{ get; }
	GameObject						RightController				{ get; }

	DistanceGrabbing				LeftDistanceGrab			{ get; }
	DistanceGrabbing				RightDistanceGrab			{ get; }

	VRTK_Pointer_Extension			LeftTeleportPointer			{ get; }
	VRTK_Pointer_Extension			RightTeleportPointer		{ get; }

	VRTK_UIPointer					LeftUIPointer				{ get; }
	VRTK_UIPointer					RightUIPointer				{ get; }

	VRTK_HeadsetCollision			HeadsetCollision			{ get; }
	VRTK_HeadsetControllerAware		HeadsetControllerAwareLeft	{ get; }
	VRTK_HeadsetControllerAware		HeadsetControllerAwareRight	{ get; }

    GameEvent                       OnDestinationPointSnap         { get; }



    void							AddAudioLog( AudioLogOnline audioLog );

}

/// <summary> Class used as container for player hands components references </summary>
public class PlayerCommon : MonoBehaviour, IPlayerCommon {

	public static IPlayerCommon				Instance					= null;
	
	[SerializeField]
	private	GameObject						m_LeftController			= null;
	public	GameObject						LeftController
	{
		get { return m_LeftController; }
	}

	[SerializeField]
	private	GameObject						m_RightController			= null;
	public	GameObject						RightController
	{
		get { return m_RightController; }
	}

	[SerializeField]
	private	VRTK_HeadsetCollision			m_HeadsetCollision			= null;
	public	VRTK_HeadsetCollision			HeadsetCollision
	{
		get { return m_HeadsetCollision; }
	}

	[SerializeField]
	private	VRTK_HeadsetControllerAware		m_HeadsetControllerAwareLeft	= null;
	public	VRTK_HeadsetControllerAware		HeadsetControllerAwareLeft
	{
		get { return m_HeadsetControllerAwareLeft; }
	}
	[SerializeField]
	private	VRTK_HeadsetControllerAware		m_HeadsetControllerAwareRight	= null;
	public	VRTK_HeadsetControllerAware		HeadsetControllerAwareRight
	{
		get { return m_HeadsetControllerAwareRight; }
	}

    [SerializeField]
    private GameEvent                       m_OnDestinationPointSnap    = null;
    public  GameEvent                       OnDestinationPointSnap
    {
        get { return m_OnDestinationPointSnap; }
    }

    private	DistanceGrabbing				m_LeftDistanceGrab			= null;
	public	DistanceGrabbing				LeftDistanceGrab
	{
		get { return m_LeftDistanceGrab; }
	}

	private	DistanceGrabbing				m_RightDistanceGrab			= null;
	public	DistanceGrabbing				RightDistanceGrab
	{
		get { return m_RightDistanceGrab; }
	} 
	

	private	VRTK_Pointer_Extension			m_LeftTeleportPointer		= null;
	public	VRTK_Pointer_Extension			LeftTeleportPointer
	{
		get { return m_LeftTeleportPointer; }
	}

	private	VRTK_Pointer_Extension			m_RightTeleportPointer		= null;
	public	VRTK_Pointer_Extension			RightTeleportPointer
	{
		get { return m_RightTeleportPointer; }
	}

	private	VRTK_UIPointer					m_LeftUIPointer				= null;
	public	VRTK_UIPointer					LeftUIPointer
	{
		get { return m_LeftUIPointer; }
	}

	private	VRTK_UIPointer					m_RightUIPointer			= null;
	public	VRTK_UIPointer					RightUIPointer
	{
		get { return m_RightUIPointer; }
	}


	private	List<AudioLogOnline>			m_AudioLogs					= null;

	//////////////////////////////////////////////////////////////////////////
	// AWAKE
	private	void Awake()
	{
		// STATIC REF
		Instance = this as IPlayerCommon;

		m_LeftDistanceGrab		= LeftController.GetComponent<DistanceGrabbing>();
		m_RightDistanceGrab		= RightController.GetComponent<DistanceGrabbing>();

		m_LeftTeleportPointer	= LeftController.GetComponent<VRTK_Pointer_Extension>();
		m_RightTeleportPointer	= RightController.GetComponent<VRTK_Pointer_Extension>();

		m_LeftUIPointer			= LeftController.GetComponent<VRTK_UIPointer>();
		m_RightUIPointer		= RightController.GetComponent<VRTK_UIPointer>();

		GameManager.SaveManagemnt.OnSave += OnSave;
		GameManager.SaveManagemnt.OnLoad += OnLoad;

		m_AudioLogs = new List<AudioLogOnline>();
	}


	public	void	AddAudioLog( AudioLogOnline audioLog )
	{
		m_AudioLogs.Add ( audioLog );
	}


	//////////////////////////////////////////////////////////////////////////
	// OnSave
	private	void	OnSave( SaveLoadUtilities.PermanentGameData saveData )
	{
		saveData.SaveAudioLogs( m_AudioLogs.ToArray() );
	}

	//////////////////////////////////////////////////////////////////////////
	// OnLoad
	private	void	OnLoad( SaveLoadUtilities.PermanentGameData loadData )
	{
		m_AudioLogs = new List<AudioLogOnline>( loadData.GetAudioLogs() );
	}

}
