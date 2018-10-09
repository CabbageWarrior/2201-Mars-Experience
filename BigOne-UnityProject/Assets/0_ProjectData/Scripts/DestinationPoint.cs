// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;

public class DestinationPoint : MonoBehaviour {

	private	static DestinationPoint[] AllDestPoints					= null;

	public	static DestinationPoint	currentPointedDestinationPoint	= null;
	public	static DestinationPoint	currentPlayerDestinationPoint	= null;

	[SerializeField]
	private		GameObject		m_LockedCursorObject				= null;
	public		GameObject		LockedCursorObject
	{
		get { return m_LockedCursorObject; }
	}

	[SerializeField]
	private		GameObject		m_DefaultCursorObject				= null;
	public		GameObject		DefaultCursorObject
	{
		get { return m_DefaultCursorObject; }
	}

	[SerializeField]
	private		GameObject		m_HoverCursorObject					= null;
	public		GameObject		HoverCursorObject
	{
		get { return m_HoverCursorObject; }
	}

	[SerializeField]
	private		VRTK_DestinationPoint.RotationTypes m_SnapToRotation = VRTK_DestinationPoint.RotationTypes.NoRotation;
	public		VRTK_DestinationPoint.RotationTypes SnapToRotation
	{
		get { return m_SnapToRotation; }
	}


	[Tooltip(" define the max absolute angle of directionIndicator to satisfy teleport condition" )]
	[SerializeField][Range( -1, 180 )]
	private		int				m_ValidAngle						= 20;
	public		int				ValidAngle
	{
		get { return m_ValidAngle; }
	}

	[Tooltip(" define if this destination point must force proper rotation to directionIndicator ") ]
	[SerializeField]
	private		bool			m_ForcedDirection					= false;
	public		bool			ForcedDirection
	{
		get{ return m_ForcedDirection; }
	}

	[SerializeField]
	private		GameEventArg1	m_OnEnter							= null;
		
	[SerializeField]
	private		GameEventArg1	m_OnExit							= null;

	[SerializeField]
	private	bool				m_TeleportEnabled					= true;
	public	bool				TeleportEnabled
	{
		get { return m_TeleportEnabled; }
	}

	private		Collider		m_PointCollider						= null;
	public		Collider		Collider
	{
		get { return m_PointCollider; }
	}


//	private		int				m_TeleportAreaLayerIndex			= 0;
//	private		int				m_DefaultLayerIndex					= 0;


    //////////////////////////////////////////////////////////////////////////
    // Awake
    private void Awake()
    {
//        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerMask.NameToLayer("Default");
    }


    //////////////////////////////////////////////////////////////////////////
    // START
    private void Start()
	{
		m_PointCollider = GetComponentInChildren<Collider>();
        (m_PointCollider as CapsuleCollider).radius = 0.2f;


        RestoreLocationIndicator();

		if ( AllDestPoints == null )
		{
			AllDestPoints = FindObjectsOfType<DestinationPoint>();
		}

//		m_TeleportAreaLayerIndex	= LayerMask.NameToLayer( "TeleportArea" );
//		m_DefaultLayerIndex			= LayerMask.NameToLayer( "Default" );
	}


	//////////////////////////////////////////////////////////////////////////
	// Enable
	public void Enable()
	{
		m_TeleportEnabled = true;
	}


	//////////////////////////////////////////////////////////////////////////
	// Disable
	public void Disable()
	{
		m_TeleportEnabled = false;
	}


	//////////////////////////////////////////////////////////////////////////
	// OnEnter
	public void	OnEnter()
	{
		m_OnEnter.Invoke( gameObject );
		HideLocationIndicator();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnExit
	public void	OnExit()
	{
		m_OnExit.Invoke( gameObject );
		HideLocationIndicator();
	}
	


	//////////////////////////////////////////////////////////////////////////
	// ShowLocationIndicatorGood
	public	void ShowLocationIndicatorGood()
	{
		m_PointCollider.enabled = true;

		ToggleObject( m_LockedCursorObject,		false );
		ToggleObject( m_DefaultCursorObject,	false );
		ToggleObject( m_HoverCursorObject,		true  );
		
/*		if ( gameObject.layer != m_TeleportAreaLayerIndex )
		{
//			gameObject.layer = m_TeleportAreaLayerIndex;
		}
*/	}


	//////////////////////////////////////////////////////////////////////////
	// ShowLocationIndicatorBad
	public	void ShowLocationIndicatorBad()
	{
		m_PointCollider.enabled = true;

		ToggleObject( m_LockedCursorObject,		true  );
		ToggleObject( m_DefaultCursorObject,	false );
		ToggleObject( m_HoverCursorObject,		false );

/*		if ( gameObject.layer != m_DefaultLayerIndex )
		{
//			gameObject.layer = m_DefaultLayerIndex;
		}
*/	}


	//////////////////////////////////////////////////////////////////////////
	// RestoreLocationIndicator
	public void	RestoreLocationIndicator()
	{
        if (m_PointCollider == null)
            return;

		ToggleObject( m_HoverCursorObject, false );

		if ( m_TeleportEnabled )
		{
			m_PointCollider.enabled = true;

			ToggleObject( m_DefaultCursorObject, true );
			ToggleObject( m_LockedCursorObject,	 false );

/*			if ( gameObject.layer != m_TeleportAreaLayerIndex )
			{
//				gameObject.layer = m_TeleportAreaLayerIndex;
			}
*/		}
		else
		{
			m_PointCollider.enabled = false;

			m_PointCollider.enabled = false;
			ToggleObject( m_LockedCursorObject,  true);
			ToggleObject( m_DefaultCursorObject, false);

/*			if ( gameObject.layer != m_DefaultLayerIndex )
			{
//				gameObject.layer = m_DefaultLayerIndex;
			}
*/		}
	}
	

	//////////////////////////////////////////////////////////////////////////
	// HideLocationIndicator
    public  void HideLocationIndicator()
    {
		m_PointCollider.enabled = false;

        ToggleObject( m_LockedCursorObject,		false );
        ToggleObject( m_DefaultCursorObject,	false );
        ToggleObject( m_HoverCursorObject,		false );

/*		if ( gameObject.layer != m_DefaultLayerIndex )
		{
//			gameObject.layer = m_DefaultLayerIndex;
		}
 */   }


	//////////////////////////////////////////////////////////////////////////
	// ResetAll
	public static void ResetAll()
	{
		if ( AllDestPoints == null )
		{
			return;
		}

		foreach( DestinationPoint d in AllDestPoints )
		{
			d.RestoreLocationIndicator();
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// ToggleObject
	protected virtual void ToggleObject( GameObject givenObject, bool state )
	{
		if ( givenObject != null && givenObject.activeSelf != state )
		{
			givenObject.SetActive( state );
		}
	}

	/*
	//////////////////////////////////////////////////////////////////////////
	// UNITY
    private void OnTriggerEnter( Collider other )
	{
		if ( other.name == "TriggerCollider" )
		{
			if ( m_TeleportEnabled )
			{
				if ( currentDestinationPoint != null )
				{
					currentDestinationPoint.RestoreLocationIndicator();
				}
				currentDestinationPoint = this;
				ShowLocationIndicatorGood();
			}
			else
			{
				ShowLocationIndicatorBad();
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.name == "TriggerCollider" )
		{
			currentDestinationPoint = null;
			RestoreLocationIndicator();
		}
	}
	*/
}
