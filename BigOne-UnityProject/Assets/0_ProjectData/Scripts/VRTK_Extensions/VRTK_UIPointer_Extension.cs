// Scripted by Roberto Leogrande

using UnityEngine;
using UnityEngine.EventSystems;
using VRTK;


public class VRTK_UIPointer_Extension : VRTK_UIPointer {
	
	[Space()]
	[Header("Extension properties")]

	[Range( 0.001f, 1f )][SerializeField]
	private	float	m_InteractionMinDistance = 0.2f;
	public float	InteractionMinDistance
	{
		get { return m_InteractionMinDistance; }
	}

	[HideInInspector]
	public	bool	hasIteraction = false;
	
	protected override void OnEnable()
	{
		pointerOriginTransform = (originalPointerOriginTransform == null ? VRTK_SDK_Bridge.GenerateControllerPointerOrigin(gameObject) : originalPointerOriginTransform);

		controller = (controller != null ? controller : GetComponentInParent<VRTK_ControllerEvents>());
		ConfigureEventSystem();
		pointerClicked = false;
		lastPointerPressState = false;
		lastPointerClickState = false;
		beamEnabledState = false;
	}

	protected override void OnDisable()
	{
		if (cachedVRInputModule && cachedVRInputModule.pointers.Contains(this))
		{
			cachedVRInputModule.pointers.Remove(this);
		}
	}
	
	/*
	public override bool IsActivationButtonPressed()
	{
		return true;
	}
	
	/// <summary>
	/// The IsSelectionButtonPressed method is used to determine if the configured selection button is currently in the active state.
	/// </summary>
	/// <returns>Returns `true` if the selection button is active.</returns>
	*/
	public override bool IsSelectionButtonPressed()
	{

		if ( currentTarget != null && currentTarget.GetComponent<IDragHandler>() != null )
			return true;

		return (controller != null ? controller.IsButtonPressed(selectionButton) : false);
	}
	

	public override VRTK_VRInputModule SetEventSystem( EventSystem eventSystem )
	{
		if (!(eventSystem is VRTK_EventSystem_Extension))
		{
			GameObject go = eventSystem.gameObject;
			Destroy( eventSystem );
			eventSystem = go.AddComponent<VRTK_EventSystem_Extension>();
		}

		return eventSystem.GetComponent<VRTK_VRInputModule_Extension>();
	}


	protected override void ConfigureEventSystem()
	{
		if (cachedEventSystem == null)
		{
			cachedEventSystem = FindObjectOfType<VRTK_EventSystem_Extension>();
			if ( cachedEventSystem == null || cachedEventSystem.gameObject.activeSelf == false)
			{
				cachedEventSystem =	new GameObject( "EventSystem" ).AddComponent<VRTK_EventSystem_Extension>();
			}
		}

		if (cachedVRInputModule == null)
		{
			cachedVRInputModule = SetEventSystem(cachedEventSystem);
		}

		if (cachedEventSystem != null && cachedVRInputModule != null)
		{
			if (pointerEventData == null)
			{
				pointerEventData = new PointerEventData(cachedEventSystem);
			}

			if (!cachedVRInputModule.pointers.Contains(this))
			{
				cachedVRInputModule.pointers.Add(this);
			}
		}
	}


}
