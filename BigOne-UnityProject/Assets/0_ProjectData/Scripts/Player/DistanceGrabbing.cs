// Scripted by Roberto Leogrande

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Highlighters;


public class DistanceGrabbing : MonoBehaviour {

	[Range( 1f, 5f )][SerializeField]
	private float				m_TriggerDistance			= 4.0f;

	[Range( 1f, 5f )][SerializeField]
	private float				m_GrabDistance				= 3.0f;

	[Range( 0.2f, 2f )][SerializeField]
	private float				m_GrabArea					= 1.0f;

	[Range( 0.2f, 15f )][SerializeField]
	private float				m_GrabTime					= 0.4f;


	private	GrabbableAtDistance	m_GrabbingObject			= null;
	public	GrabbableAtDistance	GrabbingObject {
		get { return m_GrabbingObject; }
	}

	private	CapsuleGrabber		m_CapsuleCasterScript		= null;
	public	bool	IsEnabled
	{
		get { return m_CapsuleCasterScript.IsEnabled; }
		set { m_CapsuleCasterScript.IsEnabled = value; }
	}


	private	GameObject			m_CapsuleCaster				= null;
	private	DistanceGrabbing	m_OtherGrabber				= null;
	private	VRTK_InteractTouch	m_InteractTouchScript		= null;
	private	VRTK_InteractGrab	m_InteractGrabScript		= null;


	//////////////////////////////////////////////////////////////////////////
	// START
	private IEnumerator Start()
	{
		m_InteractGrabScript  = GetComponent<VRTK_InteractGrab>();
		m_InteractTouchScript = GetComponent<VRTK_InteractTouch>();

		{	// auto assign the other DistanceGrabbing controller script
			if ( name == "RightController" )
			{
				m_OtherGrabber = PlayerCommon.Instance.LeftDistanceGrab;
			}
			else
			{
				m_OtherGrabber = PlayerCommon.Instance.RightDistanceGrab;
			}
		}

		yield return new WaitForSecondsRealtime( 1f );

		// Controller events
		m_InteractGrabScript.GrabButtonPressed  += new ControllerInteractionEventHandler( TryDistanceGrab );
		m_InteractGrabScript.GrabButtonReleased += new ControllerInteractionEventHandler( AbortGrab );
		
		// create sphere for grabber
		m_CapsuleCaster = GameObject.CreatePrimitive( PrimitiveType.Capsule );
		m_CapsuleCasterScript = m_CapsuleCaster.AddComponent<CapsuleGrabber>();

		m_CapsuleCaster.transform.SetParent( transform.parent );
		m_CapsuleCaster.transform.localScale	= new Vector3( m_GrabArea, m_TriggerDistance, m_GrabArea  );
		m_CapsuleCaster.transform.localPosition = new Vector3( 0f, 0f, m_GrabDistance );
		m_CapsuleCaster.transform.localRotation = Quaternion.Euler( 90f, 0f, 0f );

		m_CapsuleCasterScript.pointer = GetComponent<VRTK_Pointer_Extension>();
		m_CapsuleCasterScript.grabDistance = m_GrabDistance;


		yield return null;
	}


	//////////////////////////////////////////////////////////////////////////
	// OnValidate
	private void OnValidate()
	{
		if ( m_TriggerDistance < m_GrabDistance )
			m_GrabDistance = m_TriggerDistance;
	}


	//////////////////////////////////////////////////////////////////////////
	// TryDistanceGrab
	private	void	TryDistanceGrab( object o, ControllerInteractionEventArgs e )
	{
		if (	m_InteractGrabScript.GetGrabbedObject() != null
			||  !m_CapsuleCasterScript.enabled 
			||  ( m_GrabbingObject = m_CapsuleCasterScript.BestObject ) == null ) return;

		// skip if current selected object is already in distance grabb action
		if ( m_OtherGrabber.GrabbingObject == m_GrabbingObject ) return;

		StartCoroutine( GrabCoroutine() );
	}


	//////////////////////////////////////////////////////////////////////////
	// SetPhisicActive
	private	void	SetPhisicActive( Transform obj, bool state )
	{
		obj.GetComponent<Rigidbody>().useGravity = state;
		obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
		foreach( var c in obj.GetComponents<Collider>() )
		{
			if ( c.isTrigger == false )
				c.enabled = state;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// AbortGrab
	private	void	AbortGrab( object o, ControllerInteractionEventArgs e )
	{	
		// Stop grab coroutine
		StopAllCoroutines();

		// so if was grabbing object
		if ( m_GrabbingObject != null )
		{
			// manually remove from objects into capsule
//			m_CapsuleCasterScript.ManualRemove( m_GrabbingObject );

			// force hand to stop touching everything
			m_InteractTouchScript.ForceStopTouching();
			m_InteractGrabScript.ForceRelease();

			// re-enalbe object distance grab
			m_GrabbingObject.IsAllowed = true;

			// enalbe object physic
			SetPhisicActive( m_GrabbingObject.transform, true );
		}
		
		m_GrabbingObject = null;
	}


	//////////////////////////////////////////////////////////////////////////
	// GrabCoroutine
	private IEnumerator GrabCoroutine()
	{
		// Unsnap object if snapped
        m_GrabbingObject.UnSnapIfSnapped();

		// Disable phisic
		SetPhisicActive( m_GrabbingObject.transform, false );
		
		// disable distance grab ( avoid conflict with the other conotrller )
		m_GrabbingObject.IsAllowed = false;

		// move object towards the controller
		float currentTime = 0;
		float interpolant = 0f;
		Vector3 startObjPosition = m_GrabbingObject.transform.position;

        while ( interpolant < 1f )
        {
			if ( m_GrabbingObject == null )
				yield break;

			currentTime += Time.unscaledDeltaTime;
			interpolant += currentTime / m_GrabTime;
            m_GrabbingObject.transform.position = Vector3.Lerp( startObjPosition, transform.position, interpolant );

            yield return null;
        }

		// ???
//		m_CapsuleCasterScript.ManualRemove( m_GrabbingObject );

        // Enable phisic
		SetPhisicActive( m_GrabbingObject.transform, true );

		// force object unhighlight
//		m_GrabbingObject.OutlineObjectCopyHighlighter.Unhighlight();

		// OBJECT GRAB
		{
			m_GrabbingObject.IsAllowed = true;

			// set object position to hand's one
			m_GrabbingObject.transform.position = transform.position;

			// force hand to stop touching everything
			m_InteractTouchScript.ForceStopTouching();

			// force the touch on the near object
			m_InteractTouchScript.ForceTouch( m_GrabbingObject.gameObject );

			// try to grab the object
			m_InteractGrabScript.AttemptGrab();
		}

		// force object unhighlight
		m_GrabbingObject.InteractableObject.Unhighlight();

        m_GrabbingObject = null;

    }

	
	//////////////////////////////////////////////////////////////////////////
	// UNITY
	private void FixedUpdate ()
	{
		if ( m_CapsuleCasterScript != null && m_InteractGrabScript != null )
			m_CapsuleCasterScript.enabled = ( m_InteractGrabScript.GetGrabbedObject() == null );
	}
	
	

}
