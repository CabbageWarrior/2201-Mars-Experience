// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;

public class VRTK_Pointer_Extension : VRTK_Pointer {

	[Space()]
	[Header("Extension properties")]

	[Tooltip("Time to wait until teleport is ready to do action")][Range( 0.01f, 2f )]
	public	float						m_TimeToCharge					= 1f;

	[Tooltip("Angle of touchpad at which teleport will activate")][Range(0.01f, 1f)]
	// normalized value that decide for teleport activation depending on touchpad angle
	public	float						m_ActivationStep					= 0.4f;

	[Tooltip("Angle of touchpad at which teleport will teleport")][Range(0.01f, 1f)]
	public	float						m_DeactivationStep				= 0.4f;

	// Direction reminder reference
	[SerializeField]
	private DirectionReminder			m_DirectionReminder				= null;
	
	[SerializeField]
	private	bool						m_TeleportEnabled				= false;
	public	bool						TeleportEnabled
	{
		get { return m_TeleportEnabled; }
		set { m_TeleportEnabled = value; }
	}

	[SerializeField]
	private	bool						m_CanCreateCurve				= false;
	public	bool						CanCreateCurve
	{
		get { return m_CanCreateCurve; }
		set { m_CanCreateCurve = value; }
	}

    [SerializeField]
    private GameEvent                   m_OnTeleport                    = null;


	// boolean value for aborting action
	private	bool						isAborting						= false;
	public	bool	IsAborting {
		set { isAborting = value; }
	}

	private	bool						m_CanDoTeleport						= false;
	public	bool						CanDoTeleport
	{
		get { return m_CanDoTeleport; }
	}


//	[SerializeField]
	private	VRTK_Pointer_Extension		m_OtherPointer						= null;
	private	VRTK_PointerDirectionIndicator_Extension	m_PointerDirection	= null;
	private VRTK_ControllerEvents		m_ControllerEvents					= null;
	private bool						m_CurrentlyActive					= false;
	private	bool						m_InvalidTeleport					= false;
	
	private	float						m_CurveInterpolant					= 0f;
	private	float						m_CurrentCurveTime					= 0f;


	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start()
	{
		activationButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
		selectionButton  = VRTK_ControllerEvents.ButtonAlias.Undefined;

		pointerRenderer = GetComponent<VRTK_BasePointerRenderer>();

		{	// auto assign the other teleport pointer script
			if ( name == "RightController" )
			{
				m_OtherPointer = PlayerCommon.Instance.LeftTeleportPointer;
				PlayerCommon.Instance.HeadsetControllerAwareRight.ControllerObscured	+= OnControllerObscured;
				PlayerCommon.Instance.HeadsetControllerAwareRight.ControllerUnobscured	+= OnControllerUnobscured;
			}
			else
			{
				m_OtherPointer = PlayerCommon.Instance.RightTeleportPointer;
				PlayerCommon.Instance.HeadsetControllerAwareLeft.ControllerObscured		+= OnControllerObscured;
				PlayerCommon.Instance.HeadsetControllerAwareLeft.ControllerUnobscured	+= OnControllerUnobscured;
			}
		}

		PlayerCommon.Instance.HeadsetCollision.HeadsetCollisionDetect		+= OnHeadsetCollisionDetect;
		PlayerCommon.Instance.HeadsetCollision.HeadsetCollisionEnded		+= OnHeadsetCollisionEnded;
	}


	//////////////////////////////////////////////////////////////////////////
	// OnControllerObscured
	private void	OnControllerObscured( object sender, HeadsetControllerAwareEventArgs e )
	{
		isAborting = true;
		ResetPointer();
		m_TeleportEnabled = false;
	}


	//////////////////////////////////////////////////////////////////////////
	// OnControllerUnobscured
	private void	OnControllerUnobscured( object sender, HeadsetControllerAwareEventArgs e )
	{
		if ( m_CanCreateCurve == false )
			return;

		isAborting = false;
		m_TeleportEnabled = true;
	}


	//////////////////////////////////////////////////////////////////////////
	// OnHeadsetCollisionDetect
	private void	OnHeadsetCollisionDetect( object sender, HeadsetCollisionEventArgs e )
	{
		isAborting = true;
		ResetPointer();
		m_TeleportEnabled = false;
	}


	//////////////////////////////////////////////////////////////////////////
	// OnHeadsetCollisionEnded
	private void	OnHeadsetCollisionEnded( object sender, HeadsetCollisionEventArgs e )
	{
		if ( m_CanCreateCurve == false )
			return;

		isAborting = false;
		m_TeleportEnabled = true;
	}




	//////////////////////////////////////////////////////////////////////////
	// OnTouchpadPressed
	private void	OnTouchpadPressed ( object o, ControllerInteractionEventArgs e )
	{
		ResetPointer();
		isAborting = true;
	}


	//////////////////////////////////////////////////////////////////////////
	// SetTeleportActive
	public	void	SetTeleportActive( bool setActive )
	{
		if ( m_CanCreateCurve == false )
			return;

		m_TeleportEnabled = setActive;
	}


	//////////////////////////////////////////////////////////////////////////
	// ResetPointer
	private void	ResetPointer()
	{
		if ( m_CurrentlyActive == false )
			return;
		
		customOrigin.localRotation = Quaternion.identity;
		
		// Hide teleport spline
		m_PointerDirection.gameObject.SetActive( false );
		pointerRenderer.enabled = false;
		this.Toggle( false );
		
		// reset internal vars
		m_CurrentlyActive = false; 
		m_CurrentCurveTime = m_CurveInterpolant = 0.0f;
		
		m_PointerDirection.transform.rotation = Quaternion.identity;
		m_PointerDirection.LockedRotation = false;

		m_InvalidTeleport = false;
		m_CanDoTeleport = false;
		DestinationPoint.ResetAll();
		DestinationPoint.currentPointedDestinationPoint = null;
	}


	//////////////////////////////////////////////////////////////////////////
	// ValidInput
	private bool	ValidInput()
	{
		return m_ControllerEvents.GetTouchpadAxis().sqrMagnitude >= m_ActivationStep;
	}


	//////////////////////////////////////////////////////////////////////////
	// ValidRelease
	private bool	ValidRelease()
	{
		return m_ControllerEvents.GetTouchpadAxis().sqrMagnitude < m_DeactivationStep;
	}


	//////////////////////////////////////////////////////////////////////////
	// CheckTeleport
	private	bool	NoObstacleOnTeleport()
	{
		Vector3 direction = ( transform.position - Camera.main.transform.position );
		Ray ray = new Ray( Camera.main.transform.position, direction );
		return Physics.Raycast( ray, direction.magnitude, LayerMask.NameToLayer("Obstacles") ) == false;
	}


#region EXTERNAL OVERRIDE

	//////////////////////////////////////////////////////////////////////////
	// ExecuteSelectionButtonAction ( Overrider )
	protected override void ExecuteSelectionButtonAction()
	{
		if ( DestinationPoint.currentPointedDestinationPoint != null )
		{
			DestinationPoint destPointExtended = DestinationPoint.currentPointedDestinationPoint;

			Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();
			playArea.position = destPointExtended.transform.position;

			if ( destPointExtended.ValidAngle > 0f || destPointExtended.ForcedDirection == true )
			{
				// set rotation to destination rotation
				playArea.rotation = destPointExtended.transform.rotation;
			}
			else
			{
				playArea.rotation = m_PointerDirection.GetRotation();
			}

			// if teleport is executed on a snap teleport point with rotation without headset rotation
			if ( destPointExtended.SnapToRotation == VRTK_DestinationPoint.RotationTypes.RotateWithNoHeadsetOffset )
			{
				// direction reminder get destination point rotation
				m_DirectionReminder.transform.rotation = destPointExtended.transform.rotation;
			}

			// hide teleport snap point that is selected
			if ( destPointExtended != null )
			{
				DestinationPoint.ResetAll();
				m_CanDoTeleport = false;
				DestinationPoint.currentPointedDestinationPoint = null;
			}
		}
		else
		{
			base.ExecuteSelectionButtonAction();
		}

		// set current indicator rotation to reminder
		m_DirectionReminder.transform.rotation = m_PointerDirection.GetRotation();
	}

	#endregion

	//////////////////////////////////////////////////////////////////////////
	// UNITY
	protected override void Update()
	{
		// GET COMPONENTES
		if ( m_PointerDirection == null )
		{
			m_PointerDirection =  pointerRenderer.directionIndicator as VRTK_PointerDirectionIndicator_Extension;
			return;
		}

		if ( m_ControllerEvents == null )
		{
			m_ControllerEvents = GetComponent<VRTK_ControllerEvents>();
			m_ControllerEvents.TouchpadPressed  += OnTouchpadPressed;
			return;
		}
		
		// sanity check
		if ( m_DeactivationStep > m_ActivationStep )
			m_DeactivationStep = m_ActivationStep - 0.001f;

		if ( m_TeleportEnabled == false || m_CanCreateCurve == false )
			return;

		bool validInput = ValidInput();

		DestinationPoint destPointExtended = DestinationPoint.currentPointedDestinationPoint;

#region ABORTING
		
		// if abort request and analog in valid position and currently active
		if ( isAborting && m_CurrentlyActive )
		{
			// reset
			ResetPointer();
			DestinationPoint.ResetAll();
			return;
		}

		// if abort request and no more in valid position abort request is deleted
		if ( isAborting && !validInput )
		{
			isAborting = false;
			return;
		}
		
#endregion

#region ACTIVATION

		// activate teleport
		if ( m_CurrentlyActive == false && m_ControllerEvents.touchpadTouched && m_ControllerEvents.GetTouchpadAxisAngle() != 0f && validInput && isAborting == false )
		{
			// hide other pointer and reset this one
			DestinationPoint.ResetAll();
			m_OtherPointer.isAborting = true;

			// Show valid pointer
			pointerRenderer.enabled = true;
			m_PointerDirection.gameObject.SetActive( false );
			m_PointerDirection.LockedRotation = false;
			this.Toggle( true );

			m_PointerDirection.gameObject.SetActive( false );
			m_PointerDirection.isActive = false;

			m_CurrentlyActive = true;
			m_CurrentCurveTime = m_CurveInterpolant = 0.0f;

		}
		  
		#endregion

#region DURING
		// hold command, loading teleport
		if ( m_CurrentlyActive && ValidRelease() == false )
		{
			m_InvalidTeleport = false;

			// CURVE LOADING
			{
				if ( m_CurveInterpolant < 1.0f )
				{
					m_CurrentCurveTime += Time.deltaTime;
					m_CurveInterpolant = Mathf.Clamp01( m_CurrentCurveTime / m_TimeToCharge );

					GameObject container = pointerRenderer.GetPointerObjects()[0];
					int toActivate = ( int )( container.transform.childCount * m_CurveInterpolant );
					for ( int i = 0; i < container.transform.childCount; i++ )
					{
						Renderer ItemRenderer = container.transform.GetChild( i ).GetComponent<Renderer>();
						ItemRenderer.enabled = i < toActivate;
					}
				}

				m_CanDoTeleport = ( m_CurveInterpolant > 0.9f );
//				pointerRendererExtended.ShowCursor = m_CanDoTeleport;

				if ( m_PointerDirection.gameObject.activeSelf != m_CanDoTeleport )
				{
					m_PointerDirection.gameObject.SetActive( m_CanDoTeleport );
					m_PointerDirection.isActive = m_CanDoTeleport;
				}
			}

			// update indicator headset offset
			m_OtherPointer.m_PointerDirection.headsetOffset = m_PointerDirection.headsetOffset = transform.parent.eulerAngles.y;

	#region CORRECT DIRECTION  SNAP POINT

			// if pointer is hitting a destination point
			if ( destPointExtended != null && destPointExtended.ValidAngle > 0 )
			{
				float indicatorRotation = m_PointerDirection.GetRotation().eulerAngles.y;
				float destPointAngle = destPointExtended.transform.eulerAngles.y;

				indicatorRotation = ( ( indicatorRotation > 180 ) ? indicatorRotation -= 360 : indicatorRotation );
				
				// If angle of indicator is not acceptable teleport is not allowed and show invalid marker
				if ( Mathf.Abs( Mathf.DeltaAngle( destPointAngle, indicatorRotation ) ) > destPointExtended.ValidAngle )
				{
					destPointExtended.ShowLocationIndicatorBad();
				}
				// else show valid marker
				else
				{
					destPointExtended.ShowLocationIndicatorGood();
				}

				m_InvalidTeleport = !destPointExtended.HoverCursorObject.activeSelf;
			}
	#endregion
		}

#endregion

#region TELEPORT

		// if teleport is currently active and analog is not hold
		if ( m_CurrentlyActive && ValidRelease() )
		{
			// if teleport has completly load execute teleport
			if ( m_CanDoTeleport && m_InvalidTeleport == false && IsStateValid() == true )
			{
				ExecuteSelectionButtonAction();

                if ( m_OnTeleport != null && m_OnTeleport.GetPersistentEventCount() > 0 )
                    m_OnTeleport.Invoke();

                // EVENTS
                {
					// from one destination point to another
					if ( destPointExtended != null && DestinationPoint.currentPlayerDestinationPoint != null && DestinationPoint.currentPlayerDestinationPoint != destPointExtended )
					{
						DestinationPoint.currentPlayerDestinationPoint.OnExit();
					}
					// To a non destination point
					if ( destPointExtended == null && DestinationPoint.currentPlayerDestinationPoint != null )
					{
						DestinationPoint.currentPlayerDestinationPoint.OnExit();
						DestinationPoint.currentPlayerDestinationPoint = null;
					}

					// assign new destination point as current point
					if ( destPointExtended != null )
					{
						DestinationPoint.currentPlayerDestinationPoint = destPointExtended;
						destPointExtended.OnEnter();
					}
				}
			}
			
			// hide teleport snap point that is selected
			if ( destPointExtended != null )
			{
				DestinationPoint.ResetAll();
				m_CanDoTeleport = false;
			}
			
			// reset teleport conditions
			ResetPointer();
			destPointExtended = null;

		}

		#endregion
		    
#region SNAP POINT

		// set as rotation locked
		m_PointerDirection.LockedRotation = false;

		// if is targeting a teleport snap point
		if ( destPointExtended != null )
		{
			// make bazier continue pointing to snap point position
			customOrigin.LookAt ( destPointExtended.transform.position );

			if ( destPointExtended.ForcedDirection == true )
			{
				// set rotation of directionindicator no more locked
				m_PointerDirection.LockedRotation = true;
				
				// set right rotation to directionindicator
				m_PointerDirection.transform.position = destPointExtended.transform.position;
				m_PointerDirection.transform.rotation = destPointExtended.transform.rotation;
			}
		}
		else
		{
			// pointer origin rotation will be reset
			customOrigin.localRotation = Quaternion.identity;
		}
		
#endregion

		// base class update
		base.Update();
	}
	
}
