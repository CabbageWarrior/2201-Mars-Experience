using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TooltipController : MonoBehaviour
{
	//private bool			isTooltipControlActive	= true;
	//private bool			isBraceletTooltipActive = false;
	//private bool			isButtonTooltipActive	= false;

	//private bool			isTargetVisible			= false;
	//private float			viewAngle				= 110f;
	//private Camera			playerCam;

	public	GameObject		braceletPlaceholder;
	//public	GameObject		openDoorButton;
	public	GameObject		grabAnimation;
	public	float			tooltipWaitTime;

	//	public	float			braceletCounter;
	//	public	float			buttonCounter;

	public void CallWaitCoroutine()
	{
		StartCoroutine("WaitBeforeActivation");
	}

	IEnumerator WaitBeforeActivation()
	{
		yield return new WaitForSeconds(tooltipWaitTime);
		grabAnimation.SetActive(true);
		braceletPlaceholder.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
		braceletPlaceholder.GetComponent<GrabbableAtDistance>().IsAllowed = true;
	}

	//	public bool IsTooltipControlActive
	//	{
	//		get
	//		{
	//			return isTooltipControlActive;
	//		}

	//		set
	//		{
	//			isTooltipControlActive = value;
	//		}
	//	}

	//	public bool IsBraceletTooltipActive
	//	{
	//		get
	//		{
	//			return isBraceletTooltipActive;
	//		}

	//		set
	//		{
	//			isBraceletTooltipActive = value;
	//		}
	//	}

	//	public bool IsButtonTooltipActive
	//	{
	//		get
	//		{
	//			return isButtonTooltipActive;
	//		}

	//		set
	//		{
	//			isButtonTooltipActive = value;
	//		}
	//	}

	//	private void Update()
	//	{
	//		if (playerCam == null)
	//		{
	//			playerCam = Camera.main;
	//			return;
	//		}

	//		if (IsTooltipControlActive)
	//		{
	//			if (IsBraceletTooltipActive)
	//			{
	//				DetectObjectPosition(braceletPlaceholder);
	//				if (isTargetVisible)
	//				{
	//					//Debug.Log("Vedo il bracciale");
	//					//Parte il countdown
	//				}
	//			}
	//			if (IsButtonTooltipActive)
	//			{
	//				DetectObjectPosition(openDoorButton);
	//				if (isTargetVisible)
	//				{
	//					//Parte il countdown
	//				}
	//			}
	//		}
	//	}

	//	public void DetectObjectPosition(GameObject tooltipTarget)
	//	{
	//		Vector3 dirToObject = (tooltipTarget.transform.position - playerCam.transform.position).normalized;
	//		float dstToObject = Vector3.Distance(playerCam.transform.position, tooltipTarget.transform.position);

	//		if (Vector3.Angle(playerCam.transform.forward, dirToObject) < viewAngle / 2)
	//		{
	//			if (Physics.Raycast(playerCam.transform.position, dirToObject, dstToObject))
	//			{
	//				isTargetVisible = true;
	//			}
	//		}
	//		else
	//		{
	//			isTargetVisible = false;
	//		}
	//	}

}
