using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTargetExclamationMark : ScanTarget
{
 //   public GameObject overlayClonePrefab;
	//public Sprite infoSprite;
	//public string infoText;
	
	////[HideInInspector]
 //   [Space]
 //   public GameObject overlayCloneInstance;

	private void Awake()
	{
		overlayCloneInstance = Instantiate( overlayClonePrefab, transform );
		overlayCloneInstance.SetActive(false);
	}
}
