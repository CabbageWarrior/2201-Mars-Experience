// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;

public class VRTK_EventSystem_Extension : VRTK_EventSystem {

	protected override void OnEnable()
	{
		base.OnEnable();

		Destroy( GetComponent<VRTK_VRInputModule>() );

		vrInputModule = gameObject.AddComponent<VRTK_VRInputModule_Extension>();
	}
	
}
