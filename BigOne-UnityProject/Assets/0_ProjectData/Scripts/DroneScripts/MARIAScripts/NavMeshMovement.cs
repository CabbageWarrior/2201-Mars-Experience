using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public abstract class NavMeshMovement : MARIAMovement
{
    /// <summary>
    /// Agent moving on the NavMesh.
    /// </summary>
    [Header("Navigation")]
    public NavMeshAgent agent;

    [HideInInspector]
    public TargetTrigger targetTrigger;

    /// <summary>
    /// M.A.R.I.A.'s movement target.
    /// </summary>
    public TargetTrigger target;
    public AudioSource movementAudioSource;

	public GameObject movementSFX_GameObject;
	private StudioEventEmitter movementSFX_Emitter;

	bool isPlayingMovementAudio = false;

    /// <summary>
    /// Checks if an interaction is running.
    /// </summary>
    protected bool isTargetInteractionStarted;

    /// <summary>
    /// M.A.R.I.A.'s last movement target.
    /// </summary>
    protected TargetTrigger lastTarget;

    /// <summary>
    /// Checks if M.A.R.I.A. is detected by the NavMesh.
    /// </summary>
    private bool isOnNavMesh;

    private MARIAInteraction currentInteraction;

    public abstract IEnumerator ExecuteTargetAction(MARIAInteraction interaction);

    private Rigidbody rb;
    private Vector3 lastPosition = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        targetTrigger = GameObject.FindObjectOfType<TargetTrigger>();
        rb = gameObject.GetComponent<Rigidbody>();

		movementSFX_Emitter = movementSFX_GameObject.GetComponent<StudioEventEmitter> ();
    }

    protected virtual void Update()
    {
        if (isMovementEnabled)
        {
            MoveOnNavMesh();

            if (target != null
                && (new Vector2(target.transform.position.x, target.transform.position.z) - new Vector2(agent.transform.position.x, agent.transform.position.z)).sqrMagnitude <= target.interactionRange * target.interactionRange)
            {
                OnTargetReached();
            }
        }

        UpdateMovementSound();
    }

    private void MoveOnNavMesh()
    {
        if (agent.enabled)
        {
            isOnNavMesh = agent.isOnNavMesh;

            if (!isOnNavMesh)
            {
                Debug.LogError("MARIA non è sulla NavMesh!!!");
            }

            if (target != null && isOnNavMesh)
            {
                // Sets agent's movement towards target (ToDo: check if target is on NavMesh)
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                // Stops agent's movement
                agent.isStopped = true;
            }
        }
    }

    private void OnTargetReached()
    {
        lastTarget = target;
        if (agent.enabled)
        {
            agent.isStopped = true;

            if (lastTarget.CheckForInteractions() && !isTargetInteractionStarted)
            {
                currentInteraction = lastTarget.GetRandomInteraction();

                // Executes chosen interaction
                StartCoroutine(ExecuteTargetAction(currentInteraction));

                isTargetInteractionStarted = true;
            }

        }
    }

    private void UpdateMovementSound()
    {
		if (movementSFX_Emitter != null)
        {
            if (agent.velocity.sqrMagnitude > 0)
            {
				if (!isPlayingMovementAudio)
                {
                    //movementAudioSource.Play();
					movementSFX_Emitter.SetParameter ("Movimento", 1f);
					isPlayingMovementAudio = true;
                }
            }
            else
            {
				if (isPlayingMovementAudio) {
					//movementAudioSource.Stop();
					movementSFX_Emitter.SetParameter ("Movimento", 0f);
					isPlayingMovementAudio = false;
				}
            }
        }
    }

    public override void StopMovement()
    {
        base.StopMovement();

        GetComponent<MARIAManager>().scanTriangles.SetActive(false);
    }
}