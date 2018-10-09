using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : NavMeshMovement
{
    /// <summary>
    /// Range in which the AI discards patrol targets too close to the player.
    /// </summary>
    public float distanceFromPlayer;

    /// <summary>
    /// Time spent idle before selecting a new target and a new interaction.
    /// </summary>
    public int timeBeforeRandomization;
    public TargetTrigger patrolTarget;

    /// <summary>
    /// List of possible patrol targets.
    /// </summary>
    TargetTrigger[] patrolPossibleTargets;
    GameObject playerGameObject;
    TargetTrigger playerTargetTrigger;

    NavMeshPath path;

	protected void Start()
    {
        path = new NavMeshPath();
    }

    protected override void Update()
    {
		if (isMovementEnabled)
		{
			if (!playerTargetTrigger)
			{
				playerGameObject = GameObject.Find("DroneTarget");
				if (playerGameObject)
				{
					playerTargetTrigger = playerGameObject.GetComponent<TargetTrigger>();
				}
				return;
			}

			if (target == null && playerTargetTrigger != null)
			{
				patrolPossibleTargets = FindObjectsOfType<TargetTrigger>();

				patrolTarget = PatrolTargetRandomization(patrolPossibleTargets);
				if (patrolTarget != null &&
					patrolTarget != lastTarget &&
					patrolTarget != playerTargetTrigger &&
					Vector3.SqrMagnitude(playerTargetTrigger.transform.position - patrolTarget.transform.position) >= distanceFromPlayer * distanceFromPlayer && // Checks if the target is too close to the player
					NavMesh.CalculatePath(agent.transform.position, patrolTarget.transform.position, NavMesh.AllAreas, path)
				)
				{
				// Performance save
				//	Debug.Log(path.status.ToString());
					if (path.status == NavMeshPathStatus.PathComplete)
					{
						target = patrolTarget;
					}
				}
			}

			base.Update();
		}
    }

    /// <summary>
    /// Randomizes patrol target and sets it.
    /// </summary>
    public override void OnSelected()
    {
        base.OnSelected();
        animator.SetTrigger("BecomesSad");

        isMovementEnabled = true;
        isTargetInteractionStarted = false;

        playerTargetTrigger = GameObject.Find("DroneTarget").GetComponent<TargetTrigger>();
        patrolPossibleTargets = null;
        target = null;
    }

    /// <summary>
    /// Target randomization.
    /// </summary>
    /// <param name="myPatrolPossibleTargets">Array of possible targets.</param>
    /// <returns></returns>
    TargetTrigger PatrolTargetRandomization(TargetTrigger[] myPatrolPossibleTargets)
    {
        TargetTrigger myPatrolTarget = null;
        if (myPatrolPossibleTargets.Length > 0)
        {
            myPatrolTarget = myPatrolPossibleTargets[Random.Range(0, myPatrolPossibleTargets.Length)];
        }
        return myPatrolTarget;
    }

    public override IEnumerator ExecuteTargetAction(MARIAInteraction interaction)
    {
		isTargetInteractionStarted = true;
		yield return StartCoroutine(interaction.Execute());

		yield return new WaitForSeconds(timeBeforeRandomization);

		// Initialize for new target assignment
		target = null;
		interaction = null;
		isTargetInteractionStarted = false;
	}
}