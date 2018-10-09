// Scripted by Roberto Leogrande


using UnityEngine;
using VRTK;
using System.Linq;

public class Pipe: MonoBehaviour {

	[SerializeField]
	private GameObject					m_UpperConnector	= null;
	[SerializeField]
	private GameObject					m_LowerConnector	= null;

	[SerializeField]
	private	Vector3						m_LinkedPosition	= Vector3.zero;
	[SerializeField]
	private	Vector3						m_LinkedRotation	= Vector3.zero;

	private bool						m_Linked			= false;
	public	bool						Linked
	{
		get { return m_Linked; }
	}

	// Internal
	private Vector3						m_StartScale		= Vector3.zero;
	private PipeTail					m_UpperPipeTail		= null;
	private PipeTail					m_LowerPipeTail		= null;

	// Components
	private Rigidbody					m_RigidBody			= null;
	private VRTK_InteractableObject		m_Interactable		= null;
	private	GrabbableAtDistance			m_DistanceGrab		= null;

	private	PipePanelEvents				m_PipesPanel		= null;
	private	Collider[]					m_Colliders			= null;


	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start()
	{

		// get components
		m_RigidBody		= GetComponent<Rigidbody>();
		m_DistanceGrab	= GetComponent<GrabbableAtDistance>();
		m_Colliders		= GetComponents<Collider>() ;
		m_Interactable	= m_DistanceGrab.InteractableObject;
		m_PipesPanel	= FindObjectOfType<PipePanelEvents>();

		// Setup and get tails
		m_UpperPipeTail = transform.Find( "UpperTail" ).GetComponent<PipeTail>().Setup( m_UpperConnector );
		m_LowerPipeTail = transform.Find( "LowerTail" ).GetComponent<PipeTail>().Setup( m_LowerConnector );

		// save start scale
		m_StartScale = transform.lossyScale;

		// register grab and ungrab callback
		m_Interactable.InteractableObjectGrabbed	+= new InteractableObjectEventHandler( OnGrab );
		m_Interactable.InteractableObjectUngrabbed	+= new InteractableObjectEventHandler( OnUnGrab );

	}


	//////////////////////////////////////////////////////////////////////////
	// OnGrab
	public void OnGrab( object sender, InteractableObjectEventArgs e )
	{
		// disable all collider
		CollisionState( false );

		m_DistanceGrab.IsAllowed = false;

		// reset scale
		transform.SetGlobalScale( m_StartScale );
	}


	//////////////////////////////////////////////////////////////////////////
	// OnUnGrab
	public void OnUnGrab( object sender, InteractableObjectEventArgs e )
	{
		// enabled all colliders
		CollisionState( true );

		m_DistanceGrab.IsAllowed = true;

		// if correctly positioned the pipe is dropped and postion set
		if ( m_UpperPipeTail.CorrectLink && m_LowerPipeTail.CorrectLink )
		{
			// force object release from hand
			m_Interactable.ForceStopInteracting();
			m_Interactable.ForceStopSecondaryGrabInteraction();

			// Set predefined fixed position
			transform.SetParent ( m_PipesPanel.transform );
//			transform.parent			= m_PipesPanel.transform.parent;
			transform.localPosition		= m_LinkedPosition;
			transform.localEulerAngles	= m_LinkedRotation;

			// Set rigid body as kinematic to physically lock position and rotation
			m_RigidBody.isKinematic = true;
			m_RigidBody.useGravity = false;

			// Set as linked
			m_Linked = true;

			// Make pipe no more grabbable
			this.Disable();

			// comunicate with panel
			m_PipesPanel.OnPipeAdd( this );
		}

		// reset scale
		transform.SetGlobalScale( m_StartScale );

	}


	//////////////////////////////////////////////////////////////////////////
	// Disable
	public	void	Disable()
	{
		// Disable grab
		m_DistanceGrab.IsAllowed = m_Interactable.isGrabbable = false;

		// no more highlights pipe on touch
		m_DistanceGrab.OutlineObjectCopyHighlighter.active = false;

		this.CollisionState( false );
	}


	//////////////////////////////////////////////////////////////////////////
	// CollisionState
	public	void	CollisionState( bool active )
	{
		foreach( var c in m_Colliders )
		{
			if ( c.isTrigger == false )
			{
				c.enabled = active;
			}
		}

		m_RigidBody.detectCollisions = active;
	}

}
