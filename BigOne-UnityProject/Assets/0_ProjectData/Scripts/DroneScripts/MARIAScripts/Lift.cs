using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Lift : MARIAInteraction
{
    public GameObject targetObject;                     // pairs the target trigger with the "true" game object target
    public NavMeshMovement navMeshMovement;
	//public float timeBeforeRandomization;
	public float lookAtDuration;

    MARIAManager mariaManager;

	private void Awake()
    {
        navMeshMovement = GameObject.FindObjectOfType<NavMeshMovement>();
        mariaManager = GameObject.FindObjectOfType<MARIAManager>();
    }

    public override IEnumerator Execute()
    {
        if (targetObject)
        {
            navMeshMovement.transform.DOLookAt(targetObject.transform.position, lookAtDuration, AxisConstraint.Y);
            yield return new WaitForSeconds(lookAtDuration);

            mariaManager.scanTriangles.SetActive(true);

            targetObject.transform.DOLocalMoveY((targetObject.transform.position.y + 1), 1f).SetDelay(1f).OnComplete(OnLiftEnd);
		}
        else
        {
            Debug.LogWarning("Lift (MARIAInteraction): Nessun target impostato!");
            navMeshMovement.target = null;
        }

        yield return null;
    }

    void OnLiftEnd()
    {
		targetObject.transform.DOLocalMoveY((targetObject.transform.position.y - 1), 1f).SetDelay(1f).OnComplete(OnDescentEnd);
    }

    void OnDescentEnd()
    {
		mariaManager.scanTriangles.SetActive(false);
        StartCoroutine(WaitBeforeRandomization());
    }

    IEnumerator WaitBeforeRandomization()
    {
		navMeshMovement.target = null;
        yield return null;
    }
}