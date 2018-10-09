using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessing : MonoBehaviour
{
	private Camera playerCamera;
	public float postProcessingOverlapDistance = 0.5f;
	public Camera[] cameras;
	public List<MeshRenderer> objectsWithMesh;
	public PostProcessingProfile postProcessing;
	public PostProcessingProfile defaultPostProcessing;

    public FMODUnity.StudioEventEmitter headsetCollisionEventEmitter;

    [Space]
    public GameEvent OnDroneEnter;
    public GameEvent OnDroneExit;

    private PostProcessingBehaviour postProcessingBehaviour;

	private void Update()
	{
		if (playerCamera == null)
		{
			playerCamera = Camera.main;
			return;
		}

		//OverlapCheck();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PlayerHead")
		{
			SwitchPostProcessingProfile(postProcessing);
			SwitchMeshesOnOff(true);
            headsetCollisionEventEmitter.Play();

            DoOnDroneEnter();
        }
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "PlayerHead")
		{
			SwitchPostProcessingProfile(defaultPostProcessing);
			SwitchMeshesOnOff(false);
            headsetCollisionEventEmitter.Stop();

            DoOnDroneExit();
        }
	}

	/*private void OverlapCheck()
	{
		if (Vector3.SqrMagnitude(transform.position - playerCamera.transform.position) <= postProcessingOverlapDistance * postProcessingOverlapDistance)
		{
			
		}
		else
		{
			
		}
	}*/

	private void SwitchPostProcessingProfile(PostProcessingProfile newProfile)
	{
		foreach (Camera camera in cameras)
		{
			if (camera.isActiveAndEnabled)
			{
				postProcessingBehaviour = camera.GetComponent<PostProcessingBehaviour>();
				postProcessingBehaviour.profile = newProfile;
			}
		}
	}

	private void SwitchMeshesOnOff(bool isMeshActive)
	{
		foreach (MeshRenderer objectWithMesh in objectsWithMesh)
		{
			if (isMeshActive)
			{
				objectWithMesh.enabled = false;
			}
			else
			{
				objectWithMesh.enabled = true;
			}
		}
    }

    private void DoOnDroneEnter()
    {
        if (OnDroneEnter != null)
        {
            OnDroneEnter.Invoke();
        }
    }
    private void DoOnDroneExit()
    {
        if (OnDroneExit != null)
        {
            OnDroneExit.Invoke();
        }
    }
}
