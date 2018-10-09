using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using VRTK;
using VRTK.GrabAttachMechanics;

[RequireComponent( typeof( NavMeshAgent ) )]
public class MARIAGrabAttach : VRTK_FixedJointGrabAttach {
    
    [Header("MARIA Grab Attach Only")]
    // Public
    public FMODUnity.StudioEventEmitter levitationAudioEmitter;
    [Space]
    public GameObject rightSnapTooltipGameObject;
    public GameObject leftSnapTooltipGameObject;

    [Space]
    public GameEvent OnGrab;
    public GameEvent OnUngrab;
    [Space]
    public GameEvent OnHighlight;
    public GameEvent OnUnhighlight;

    // Private
    float xRepositioningSpeed = 2f;
	float yRepositioningSpeed = 0.5f;
	float zRepositioningSpeed = 2f;

	float xRerotationingSpeed = 1f;
	float zRerotationingSpeed = 1f;

	NavMeshAgent navMeshAgent;
	Rigidbody myRigidbody;
	MARIAMenu mariaMenu;
    VRTK_InteractObjectHighlighter highlighterComponent;
    // Children
	Animator animator;

    float startYPosition;
	bool isGrabbed = false;
	public bool IsGrabbed
	{
		get {
			return isGrabbed;
		}
	}

	//myRigidbody.isKinematic = false;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		myRigidbody = GetComponent<Rigidbody>();
		mariaMenu = GetComponent<MARIAMenu>();
        highlighterComponent = GetComponent<VRTK_InteractObjectHighlighter>();

        highlighterComponent.InteractObjectHighlighterHighlighted += OnHighlightEventCaller;
        highlighterComponent.InteractObjectHighlighterUnhighlighted += OnUnhighlightEventCaller;

        animator = GetComponentInChildren<Animator>();

		Invoke( "SetStartYPosition", 1f );
	}

	private void FixedUpdate()
	{
		if ( navMeshAgent.enabled )
		{
			myRigidbody.angularVelocity = Vector3.zero;
			myRigidbody.velocity = navMeshAgent.velocity;
		}
	}

	void SetStartYPosition()
	{
		startYPosition = transform.position.y;
	}

	public override bool StartGrab( GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint )
	{
		isGrabbed = true;

		//myRigidbody.isKinematic = false;

		animator.SetTrigger( "BecomesGrabbed" );
		myRigidbody.useGravity = true;

        rightSnapTooltipGameObject.SetActive(false);
        leftSnapTooltipGameObject.SetActive(false);

        navMeshAgent.enabled = false;
        levitationAudioEmitter.SetParameter( "Standby", 1f );

        DoOnGrab();

		if ( !base.StartGrab( grabbingObject, givenGrabbedObject, givenControllerAttachPoint ) )
		{
			navMeshAgent.enabled = true;
            levitationAudioEmitter.SetParameter( "Standby", 0f );

            DoOnUngrab();

            return false;
		}
        return true;
	}

	public override void StopGrab( bool applyGrabbingObjectVelocity )
	{
		isGrabbed = false;
		animator.SetTrigger( "BecomesConfused" );

		if ( mariaMenu )
		{
			mariaMenu.SwitchMenuOff();
		}

        DoOnUngrab();

		base.StopGrab( applyGrabbingObjectVelocity );
		StartCoroutine( StopGrab_Coroutine() );
	}
	IEnumerator StopGrab_Coroutine()
	{
		yield return new WaitForSeconds( 2f );

		if ( isGrabbed )
			yield break;

		float startXPosition = transform.position.x;
		float startZPosition = transform.position.z;

		float currentXPosition = transform.position.x;
		float currentYPosition = transform.position.y;
		float currentZPosition = transform.position.z;

		float currentXRotation = transform.rotation.x;
		float currentZRotation = transform.rotation.z;

		float xPositionDirection = Mathf.Sign( startXPosition - currentXPosition );
		float yPositionDirection = Mathf.Sign( startYPosition - currentYPosition );
		float zPositionDirection = Mathf.Sign( startZPosition - currentZPosition );

		float xRotationDirection = Mathf.Sign( 0f - currentXRotation );
		float zRotationDirection = Mathf.Sign( 0f - currentZRotation );

		myRigidbody.useGravity = false;
		myRigidbody.angularVelocity = Vector3.zero;
		myRigidbody.velocity = Vector3.zero;

		while ( currentXPosition != startXPosition
			|| currentYPosition != startYPosition
			|| currentZPosition != startZPosition
			|| currentXRotation != 0
			|| currentZRotation != 0 )
		{
			if ( isGrabbed )
				yield break;

			if ( currentXPosition != startXPosition
			|| currentYPosition != startYPosition
			|| currentZPosition != startZPosition )
			{
				if ( currentXPosition != startXPosition )
				{
					currentXPosition += Time.deltaTime * xPositionDirection * xRepositioningSpeed;

					if ( currentXPosition * xPositionDirection > startXPosition * xPositionDirection )
					{
						currentXPosition = startXPosition;
					}
				}

				if ( currentYPosition != startYPosition )
				{
					currentYPosition += Time.deltaTime * yPositionDirection * yRepositioningSpeed;

					if ( currentYPosition * yPositionDirection > startYPosition * yPositionDirection )
					{
						currentYPosition = startYPosition;
					}
				}

				if ( currentZPosition != startZPosition )
				{
					currentZPosition += Time.deltaTime * zPositionDirection * zRepositioningSpeed;

					if ( currentZPosition * zPositionDirection > startZPosition * zPositionDirection )
					{
						currentZPosition = startZPosition;
					}
				}

				transform.position = new Vector3( currentXPosition, currentYPosition, currentZPosition );
			}

			if ( currentXRotation != 0 || currentZRotation != 0 )
			{
				if ( currentXRotation != 0 )
				{
					currentXRotation += Time.deltaTime * xRotationDirection * xRerotationingSpeed;

					if ( currentXRotation * xRotationDirection > 0f )
					{
						currentXRotation = 0f;
					}
				}

				if ( currentZRotation != 0 )
				{
					currentZRotation += Time.deltaTime * zRotationDirection * zRerotationingSpeed;

					if ( currentZRotation * zRotationDirection > 0f )
					{
						currentZRotation = 0f;
					}
				}

				transform.rotation = new Quaternion( currentXRotation, transform.rotation.y, currentZRotation, transform.rotation.w );
			}

			yield return null;
		}

		myRigidbody.useGravity = false;
		myRigidbody.angularVelocity = Vector3.zero;
		myRigidbody.velocity = Vector3.zero;

		navMeshAgent.enabled = true;
		animator.SetTrigger( "BecomesShaken" );
        levitationAudioEmitter.SetParameter( "Standby", 0f );
        yield return null;
	}

    private void DoOnGrab()
    {
        if (OnGrab != null)
        {
            OnGrab.Invoke();
        }
    }
    private void DoOnUngrab()
    {
        if (OnUngrab != null)
        {
            OnUngrab.Invoke();
        }
    }

    void OnHighlightEventCaller(object sender, InteractObjectHighlighterEventArgs e)
    {
        rightSnapTooltipGameObject.SetActive(true);
        leftSnapTooltipGameObject.SetActive(true);

        if (OnHighlight != null)
        {
            OnHighlight.Invoke();
        }
    }

    void OnUnhighlightEventCaller(object sender, InteractObjectHighlighterEventArgs e)
    {
        rightSnapTooltipGameObject.SetActive(false);
        leftSnapTooltipGameObject.SetActive(false);

        if (OnUnhighlight != null)
        {
            OnUnhighlight.Invoke();
        }
    }
}