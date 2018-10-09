using UnityEngine;
using VRTK;
using System.Collections;

public class MARIAManager : MonoBehaviour
{
	public NavMeshMovement currentMovement;            // active movement type
	public NavMeshMovement Follow;
	public NavMeshMovement Patrol;
	public VRTK_InteractableObject interactableObject;
	private bool isSwitchEnabled = false;                // disables Maria's switch mode in the tutorial
	private bool isPatrolEnabled = false;


    [Space]
    public GameObject scanTriangles;

	[Header("Audio")]
	public AudioSource movementSFX;

	public bool IsPatrolEnabled
	{
		get
		{
			return isPatrolEnabled;
		}

		set
		{
			isPatrolEnabled = value;
		}
	}

	public bool IsSwitchEnabled
	{
		get
		{
			return isSwitchEnabled;
		}

		set
		{
			isSwitchEnabled = value;
		}
	}

	private void Start()
	{
		if (currentMovement)
		{
			currentMovement.OnSelected();
		}
	}

	//private void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.E))
	//	{
	//		if (currentMovement == Patrol)
	//		{
	//			Debug.Log("Setta a follow");
	//			SetMovement(Follow);
	//			Debug.Log(currentMovement);
	//		}
	//		else if (currentMovement == Follow)
	//		{
	//			Debug.Log("Setta a patrol");
	//			SetMovement(Patrol);
	//		}
	//	}
	//}

	public void SetGrabbable(bool makeGrabbable)
    {
        interactableObject.isGrabbable = makeGrabbable;
    }

    #region Switch Manager
    /// <summary>
    /// Resets currentMovement and the target.
    /// </summary>
    /// <param name="newMovement">Movement logic to activate.</param>
    void SetMovement(NavMeshMovement newMovement)
    {
		if (IsSwitchEnabled)
		{
			if (currentMovement)
			{
				currentMovement.StopMovement();
			}
			currentMovement = newMovement;
			currentMovement.OnSelected();
		}
	}

    public void SetMovement(string newMovement)
    {
		//if (isActivated)
		//{
			switch (newMovement.ToUpper())
			{
				case "FOLLOW":
					SetMovement(Follow);
					break;
				case "PATROL":
					SetMovement(Patrol);
					break;
				default:
					Debug.LogError("Che movimento è \"" + newMovement + "\"?");
					break;
			}
		//}
		//else
		//{
		//	// Set tutorial movement
		//}
    }

    public void SwitchMovementMode()
    {
		//if (isPatrolEnabled)
		//{
			if (currentMovement && currentMovement.GetType() == typeof(Follow))
			{
				SetMovement(Patrol);
			}
			else
			{
				SetMovement(Follow);
			}
		//}
		//else
		//{
		//	SetMovement(Follow);
		//}
	}
    #endregion
}