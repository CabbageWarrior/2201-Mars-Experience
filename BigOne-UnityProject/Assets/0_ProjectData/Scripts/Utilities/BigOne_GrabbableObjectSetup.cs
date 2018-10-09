#if UNITY_EDITOR
using UnityEditor;

namespace BigOne {
	using UnityEngine;
	using UnityEditor;
	using VRTK;
	using VRTK.GrabAttachMechanics;
	using VRTK.SecondaryControllerGrabActions;
	using VRTK.Highlighters;

	public class BigOne_GrabbableObjectSetup : EditorWindow {
		[MenuItem( "Window/BigOne/Setup Grabbable Object" )]
		private static void Init()
		{
			Transform[] transforms = Selection.transforms;
			foreach ( Transform currentTransform in transforms )
			{

				//VRTK_InteractableObject interactableObject = SetupInteractableObject(currentTransform);
				VRTK_InteractableObject interactableObject = currentTransform.GetComponent<VRTK_InteractableObject>();
				if ( interactableObject == null )
				{
					interactableObject = currentTransform.gameObject.AddComponent<VRTK_InteractableObject>();
				}
				interactableObject.isGrabbable = true;
				interactableObject.holdButtonToGrab = true;
				interactableObject.isUsable = false;
				interactableObject.disableWhenIdle = true;
				interactableObject.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
				interactableObject.useOverrideButton = VRTK_ControllerEvents.ButtonAlias.Undefined;

				//SetupPrimaryGrab(interactableObject);
				VRTK_BaseGrabAttach grab = interactableObject.GetComponentInChildren<VRTK_BaseGrabAttach>();
				if ( grab != null )
				{
					DestroyImmediate( grab );
				}
				interactableObject.grabAttachMechanicScript = interactableObject.gameObject.AddComponent<VRTK_ChildOfControllerGrabAttach>();

				Transform handleLeft = currentTransform.Find("HandleLeft");
				if ( handleLeft == null )
				{
					handleLeft = Instantiate(new GameObject("HandleLeft"), currentTransform).transform;
					handleLeft.localPosition = new Vector3(-.1f, 0, 0);
					handleLeft.localRotation = Quaternion.Euler(0, 0, 90);
				}
				interactableObject.grabAttachMechanicScript.leftSnapHandle = handleLeft;

				Transform handleRight = currentTransform.Find("HandleRight");
				if ( handleRight == null )
				{
					handleRight = Instantiate(new GameObject("HandleRight"), currentTransform).transform;
					handleRight.localPosition = new Vector3(.1f, 0, 0);
					handleRight.localRotation = Quaternion.Euler(0, 0, -90);
				}
				interactableObject.grabAttachMechanicScript.rightSnapHandle = handleRight;
			
				//SetupSecondaryGrab(interactableObject);
				VRTK_BaseGrabAction secGrab = interactableObject.GetComponentInChildren<VRTK_BaseGrabAction>();
				if ( secGrab != null )
				{
					DestroyImmediate( secGrab );
				}
				interactableObject.secondaryGrabActionScript = interactableObject.gameObject.AddComponent<VRTK_SwapControllerGrabAction>();

				//SetupRigidbody(interactableObject);
				Rigidbody rb = interactableObject.GetComponent<Rigidbody>();
				if ( rb == null )
				{
					interactableObject.gameObject.AddComponent<Rigidbody>();
				}

				//SetupHaptics(interactableObject);
				VRTK_InteractHaptics haptics = interactableObject.GetComponentInChildren<VRTK_InteractHaptics>();
				if ( haptics == null )
				{
					interactableObject.gameObject.AddComponent<VRTK_InteractHaptics>();
				}

				//SetupHighlighter(interactableObject);
				VRTK_InteractObjectHighlighter highlighter = interactableObject.GetComponentInChildren<VRTK_InteractObjectHighlighter>();
				if ( highlighter == null )
				{
					highlighter = interactableObject.gameObject.AddComponent<VRTK_InteractObjectHighlighter>();
				}
				highlighter.touchHighlight = Color.yellow;
				
				// --> Outline
				VRTK_OutlineObjectCopyHighlighter outlineHighlighter = interactableObject.GetComponentInChildren<VRTK_OutlineObjectCopyHighlighter>();
				if ( outlineHighlighter == null )
				{
					outlineHighlighter = interactableObject.gameObject.AddComponent<VRTK_OutlineObjectCopyHighlighter>();
				}
				
				// --> Grabbable At Distance
				GrabbableAtDistance distance = interactableObject.GetComponentInChildren<GrabbableAtDistance>();
				if ( distance == null )
				{
					distance = interactableObject.gameObject.AddComponent<GrabbableAtDistance>();
				}
			}
		}
	}
}
#endif