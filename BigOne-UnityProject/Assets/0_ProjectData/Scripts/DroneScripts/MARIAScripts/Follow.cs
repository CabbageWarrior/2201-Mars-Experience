using System.Collections;
using UnityEngine;

public class Follow : NavMeshMovement
{
    protected override void Update()
    {
        if (!target)
        {
            GameObject playerGameObject = GameObject.Find("DroneTarget");

            if (playerGameObject)
            {
                target = playerGameObject.GetComponent<TargetTrigger>();
            }
            return;
        }

        base.Update();
    }

    /// <summary>
    /// Sets the player as target.
    /// </summary>
    public override void OnSelected()
    {
        base.OnSelected();
        mariaBoredom.enabled = true;
        animator.SetTrigger("BecomesHappy");

        StartCoroutine(OnSelected_Coroutine());
    }

    public override void StopMovement()
    {
        mariaBoredom.enabled = false;
        base.StopMovement();
    }

    IEnumerator OnSelected_Coroutine()
    {
        while (!target)
        {
            yield return null;
        }

        isMovementEnabled = true;
        isTargetInteractionStarted = false;
        yield return null;
    }

    public override IEnumerator ExecuteTargetAction(MARIAInteraction interaction)
    {
        isTargetInteractionStarted = true;
        StartCoroutine(interaction.Execute());

        yield return null;
    }
}