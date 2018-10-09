// Scripted by Roberto Leogrande

using UnityEngine;
using System.Collections;

public class ObjectRepositioner : MonoBehaviour {

	[Tooltip( "The minimum height at which script must make transition" )]
	public	float				m_MinHeight				= -20.0f;
	[Tooltip( "The trasform where this object must be positionated" )]
	public	Transform			m_Destination			= null;
	[Tooltip( "Check this if you want that also rotation must be set" )]
	public	bool				m_AlsoSetRotation		= false;
	[Tooltip( "The particle system reference, if null a search is done on this transform and children" )]
	public	ParticleSystem		m_ParticleSystem		= null;

	private	bool				m_IsInTransition		= false;
	public	bool				IsInTransition
	{
		get { return m_IsInTransition; }
	}

	private	bool				m_IsOK					= false;


	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start()
	{
		// Sanity checks
		if ( m_Destination == null )
		{
			print( "WARNING: RepositionerObject::Start: Destination not set !!" );
			return;
		}

		if ( m_Destination.position.y < m_MinHeight )
		{
			print( "WARNING: RepositionerObject::Start: Cannot destination transform is in ainvalid position !!" );
			return;
		}

		if ( m_ParticleSystem == null )
		{
			m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
		}

		if ( m_ParticleSystem == null )
		{
			print( "WARNING: RepositionerObject::Start: Cannot find a valid particle system in this object or children !!" );
			return;
		}

		// setup for partile system
		var particleMain = m_ParticleSystem.main;
		particleMain.playOnAwake = false;

		// script is ok
		m_IsOK = true;
	}


	//////////////////////////////////////////////////////////////////////////
	// SetObjectVisibility
	private	void	SetObjectVisibility( bool state )
	{
		// RENDERERS
		foreach ( Renderer renderer in GetComponentsInChildren<Renderer>() )
		{
			renderer.enabled = state;
		}

		// COLLIDERS
		foreach( Collider collider in GetComponentsInChildren<Collider>() )
		{
			collider.enabled = state;
		}

		// INTERACTIONS
		foreach( GrabbableAtDistance distGrab in GetComponentsInChildren<GrabbableAtDistance>() )
		{
			distGrab.IsAllowed = state;
			distGrab.InteractableObject.isGrabbable = state;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// MakeTransition ( Coroutine )
	private	IEnumerator	MakeTransition()
	{
		// lock script logic
		m_IsInTransition = true;

		// hide object and make it not interactable 
		SetObjectVisibility( false );

		// play the particle systems
		m_ParticleSystem.Play();

		// wait for end of particle system play
		while( m_ParticleSystem.time < m_ParticleSystem.main.duration )
		{
			yield return null;
		}

		// ensure only particle system stop
		m_ParticleSystem.Stop( false );
		
		// set velocity to Zero
		Rigidbody rb = GetComponent<Rigidbody>();
		if ( rb != null )
		{
			rb.WakeUp();
			rb.velocity = Vector3.zero;

			// enable object collision detection
			rb.detectCollisions = true;
		}

		// Set position
		transform.position = m_Destination.position;

		// Set rotation if needed
		if ( m_AlsoSetRotation )
		{
			transform.rotation = m_Destination.rotation;
		}

		// hide object and make it not interactable 
		SetObjectVisibility( true );

		// unlock script logic
		m_IsInTransition = false;
	}


	//////////////////////////////////////////////////////////////////////////
	// UNITY
	private void FixedUpdate()
	{
		if ( m_IsOK == false )
		{
			return;
		}

		// Check for valid condition
		if ( transform.position.y < m_MinHeight && m_IsInTransition == false )
		{
			StartCoroutine( MakeTransition() );
		}

	}
}
