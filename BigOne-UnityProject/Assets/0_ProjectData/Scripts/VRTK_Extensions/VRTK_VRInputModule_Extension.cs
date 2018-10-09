// Scripted by Roberto Leogrande

using System.Collections.Generic;
using UnityEngine;
using VRTK;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRTK_VRInputModule_Extension : VRTK_VRInputModule {

	private	void	ResetObject( VRTK_UIPointer pointer, GameObject obj )
	{
		if ( obj == null )
			return;

		// get button
		Image img	= obj.GetComponent<Image>();
		Button btn	= obj.GetComponent<Button>();
		if ( btn != null )
		{
			pointer.hoverDurationTimer = 0f;
			pointer.canClickOnHover = false;
//			Selectable selectable = btn as Selectable;
//			if ( selectable != null )
//			{
//				ColorBlock colorBlock = selectable.colors;
//				colorBlock.fadeDuration = 0f;
//				selectable.colors = colorBlock;
//			}
		}

		if ( btn != null && img != null && img.type == Image.Type.Filled )
		{
			// FILL AMOUNT BUTTON
			img.fillAmount = 0f;
		}

//		ExecuteEvents.Execute( obj,  pointer.pointerEventData, ExecuteEvents.endDragHandler );
		ExecuteEvents.Execute( obj,  pointer.pointerEventData, ExecuteEvents.pointerUpHandler );
		ExecuteEvents.Execute( obj,  pointer.pointerEventData, ExecuteEvents.pointerExitHandler);
//		ExecuteEvents.ExecuteHierarchy( obj, pointer.pointerEventData, ExecuteEvents.dropHandler );
	}

	public static	void	ResetItems( VRTK_UIPointer pointer, List<RaycastResult> results )
	{
		VRTK_UIPointer_Extension pointerExtended = pointer as VRTK_UIPointer_Extension;

		Image  img = null;
		Button btn = null;
		/*
		if ( pointer.pointerEventData.dragging )
		{
			if ( results != null )
			{
				for (int i = 0; i < results.Count; i++)
				{
					ExecuteEvents.ExecuteHierarchy( results[i].gameObject, pointer.pointerEventData, ExecuteEvents.dropHandler );
				}
			}
			pointer.pointerEventData.dragging = false;
		}
		
		if ( pointer.pointerEventData.pointerDrag != null )
		{
			ExecuteEvents.Execute( pointer.pointerEventData.pointerDrag, pointer.pointerEventData, ExecuteEvents.dragHandler );
			ExecuteEvents.Execute( pointer.pointerEventData.pointerDrag, pointer.pointerEventData, ExecuteEvents.endDragHandler );
			pointer.pointerEventData.pointerDrag = null;
			pointer.pointerEventData.dragging = false;
		}
		*/
		if ( pointer.pointerEventData.pointerPress != null )
		{
			img = pointer.pointerEventData.pointerEnter.GetComponent<Image>();
			btn = pointer.pointerEventData.pointerEnter.GetComponent<Button>();
			ExecuteEvents.Execute( pointer.pointerEventData.pointerPress,  pointer.pointerEventData, ExecuteEvents.pointerUpHandler );
			pointer.pointerEventData.pointerPress = null;
		}
				
		if ( pointer.pointerEventData.pointerEnter != null )
		{
			img = pointer.pointerEventData.pointerEnter.GetComponent<Image>();
			btn = pointer.pointerEventData.pointerEnter.GetComponent<Button>();
			ExecuteEvents.Execute( pointer.pointerEventData.pointerEnter,  pointer.pointerEventData, ExecuteEvents.pointerExitHandler );
			pointer.pointerEventData.hovered.Remove( pointer.pointerEventData.pointerEnter );
			pointer.pointerEventData.pointerEnter = null;
		}

		if ( btn != null )
		{
			pointer.hoverDurationTimer = 0f;
			pointer.canClickOnHover = false;
//			Selectable selectable = btn as Selectable;
//			if ( selectable != null )
//			{
//				ColorBlock colorBlock = selectable.colors;
//				colorBlock.fadeDuration = 0f;
//				selectable.colors = colorBlock;
//			}
		}

		if ( btn != null && img != null && img.type == Image.Type.Filled )
		{
			// FILL AMOUNT BUTTON
			img.fillAmount = 0f;
		}
		
		pointerExtended.hasIteraction = false;
		pointer.hoveringElement = null;

//		pointer.canClickOnHover = true;
//		pointer.hoverDurationTimer = 0f;
	}

//	public List<RaycastResult> prevRaycasts = new List<RaycastResult>();
	private List< List< RaycastResult > > prevRaycasts = new List<List<RaycastResult>>() { new List<RaycastResult>(), new List<RaycastResult>() };
	public override void Process()
	{
		for (int i = 0; i < pointers.Count; i++)
		{
			VRTK_UIPointer pointer = pointers[i];

			if ( pointer != null && pointer.gameObject.activeInHierarchy && pointer.enabled && pointer.PointerActive() )
			{
				List<RaycastResult> results = CheckRaycasts( pointer );
				{
					RaycastResult? draggablePanel = results.Find( s => s.gameObject.name == "VRTK_UICANVAS_DRAGGABLE_PANEL" );
					if ( draggablePanel.HasValue )
						results.Remove( draggablePanel.Value );
				}

				// reset previous objects raycasted if no more pointed
				foreach ( var prevRaycast in prevRaycasts[i] )
				{
					if ( results.Find( currRaycast => currRaycast.gameObject == prevRaycast.gameObject ).gameObject == null )
					{
						ResetObject( pointer, prevRaycast.gameObject );
					}
				}
				prevRaycasts[i] = results;

				//Process events
				Hover	(	pointer, results	);
				Click	(	pointer, results	);
				Drag	(	pointer, results	);
				Scroll	(	pointer, results	);
				
			}
		}
	}


	protected override void CheckPointerHoverClick( VRTK_UIPointer pointer, List<RaycastResult> results )
	{
		if (pointer.hoverDurationTimer > 0f)
		{
			pointer.hoverDurationTimer -= Time.deltaTime;

			// get button
			Image img = pointer.pointerEventData.pointerEnter.GetComponent<Image>();
			Button btn = pointer.pointerEventData.pointerEnter.GetComponent<Button>();
			if ( btn != null && img != null && btn.interactable == true && img.type == Image.Type.Filled )
			{
				// FILL AMOUNT BUTTON
				float currentValue = pointer.hoverDurationTimer;
				float finalValue = pointer.clickAfterHoverDuration;
				img.fillAmount = 1f - ( currentValue / finalValue );
			}
		}

		if (pointer.canClickOnHover && pointer.hoverDurationTimer <= 0f)
		{
			pointer.canClickOnHover = false;
			ClickOnDown(pointer, results, true);
		}
		
	}


	protected override void Hover( VRTK_UIPointer pointer, List<RaycastResult> results )
    {
		VRTK_UIPointer_Extension pointerExtended = pointer as VRTK_UIPointer_Extension;

		if ( pointerExtended.pointerEventData.pointerEnter != null )
		{
			// TOO DISTANT, RESET
			if ( pointer.pointerEventData.pointerCurrentRaycast.distance > pointerExtended.InteractionMinDistance )
			{
				ResetItems( pointer, results );
//				print( "My Reset" );
				return;
			}

			// button and toggle box click
			CheckPointerHoverClick( pointerExtended, results );
			if ( pointerExtended.pointerEventData.pointerEnter == null || ValidElement( pointerExtended.pointerEventData.pointerEnter ) == false )
			{
				ResetItems( pointer, results );
//				print( " Invalid element " );
				return;
			}
		}
		else
		{
			for (int i = 0; i < results.Count; i++)
			{
				RaycastResult result = results[i];
				if ( result.gameObject == null || ValidElement(result.gameObject) == false )
				{
					continue;
				}

				Selectable selectable = result.gameObject.GetComponentInParent<Selectable>();
				if ( selectable != null )
				{
					ColorBlock colorBlock = selectable.colors;
					pointer.clickAfterHoverDuration = colorBlock.fadeDuration;
//					selectable.colors = colorBlock;
				}

				if ( result.distance > pointerExtended.InteractionMinDistance )
				{
					ResetItems( pointer, results );
//					print( "hoveringElement = null   FOR" );
					break;
				}

				GameObject target = ExecuteEvents.ExecuteHierarchy( result.gameObject, pointerExtended.pointerEventData, ExecuteEvents.pointerEnterHandler );
				if ( target != null )
				{
//					pointerExtended.OnUIPointerElementEnter( pointerExtended.SetUIPointerEvent( result, target, pointerExtended.hoveringElement ) );
					pointer.canClickOnHover = true;
					pointer.hoverDurationTimer = pointer.clickAfterHoverDuration;
					pointerExtended.hoveringElement = target;
					pointerExtended.pointerEventData.pointerCurrentRaycast = result;
					pointerExtended.pointerEventData.pointerEnter = target;
					pointerExtended.pointerEventData.hovered.Add( target );
					pointerExtended.hasIteraction = true;
//					print( "hoveringElement = obj   TARGET" );
					break;
				}
				else
				{
					if (result.gameObject != pointerExtended.hoveringElement)
					{
						ResetItems( pointer, results );
//						pointerExtended.OnUIPointerElementEnter( pointerExtended.SetUIPointerEvent( result, result.gameObject, pointerExtended.hoveringElement ) );
						pointer.canClickOnHover = true;
						pointer.hoverDurationTimer = pointer.clickAfterHoverDuration;
					}
					pointerExtended.hoveringElement = result.gameObject;
//					print( "OBJ DIFFERENT" );
				}
			}

			if ( pointerExtended.hoveringElement && results.Count == 0 )
			{
				pointerExtended.OnUIPointerElementExit( pointerExtended.SetUIPointerEvent( new RaycastResult(), null, pointerExtended.hoveringElement ) );
				ResetItems( pointer, null );
//				print( "results.Count = 0" );
			}
		}
    }
	
	
    protected override void Click(VRTK_UIPointer pointer, List<RaycastResult> results)
    {
		if ( pointer.hoveringElement == null || ExecuteEvents.CanHandleEvent<IPointerClickHandler>( pointer.hoveringElement ) == false )
		{
			return;
		}

		switch (pointer.clickMethod)
        {
            case VRTK_UIPointer.ClickMethods.ClickOnButtonUp:
                ClickOnUp(pointer, results);
                break;
            case VRTK_UIPointer.ClickMethods.ClickOnButtonDown:
                ClickOnDown(pointer, results);
                break;
        }
    }
	


	protected override void ClickOnUp(VRTK_UIPointer pointer, List<RaycastResult> results)
	{
		pointer.pointerEventData.eligibleForClick = pointer.ValidClick(false);

		if ( AttemptClick( pointer ) == false )
		{
			IsEligibleClick( pointer, results );
		}
	}

	protected override void ClickOnDown(VRTK_UIPointer pointer, List<RaycastResult> results, bool forceClick = false)
	{
		pointer.pointerEventData.eligibleForClick = (forceClick ? true : pointer.ValidClick(true));

		if ( IsEligibleClick( pointer, results ) )
		{
			pointer.pointerEventData.eligibleForClick = false;
			AttemptClick( pointer );
		}

	}

	protected override bool AttemptClick(VRTK_UIPointer pointer)
	{
		if ( pointer.hoveringElement == null )
			return false;

		if (pointer.pointerEventData.pointerPress)
		{
			if ( pointer.pointerEventData.pointerPress == null || !ValidElement(pointer.pointerEventData.pointerPress) )
			{
				pointer.pointerEventData.pointerPress = null;
				return true;
			}

			if (pointer.pointerEventData.eligibleForClick)
			{
				if ( IsHovering( pointer ) == false )
				{
					ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerPress, pointer.pointerEventData, ExecuteEvents.pointerUpHandler);
					pointer.pointerEventData.pointerPress = null;
				}
			}
			else
			{
				pointer.OnUIPointerElementClick(pointer.SetUIPointerEvent(pointer.pointerEventData.pointerPressRaycast, pointer.pointerEventData.pointerPress));
				ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerPress, pointer.pointerEventData, ExecuteEvents.pointerClickHandler);
				ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerPress, pointer.pointerEventData, ExecuteEvents.pointerUpHandler);
				pointer.pointerEventData.pointerPress = null;
			}
			return true;
		}
		return false;
	}

	
    protected override void Drag( VRTK_UIPointer pointer, List<RaycastResult> results )
    {
		pointer.pointerEventData.dragging = pointer.pointerEventData.delta != Vector2.zero;
		if ( pointer.hoveringElement != null && ExecuteEvents.CanHandleEvent<IDragHandler>( pointer.hoveringElement ) == false )
		{
			if ( pointer.pointerEventData.pointerDrag != null )
			{
//				ExecuteEvents.Execute( pointer.pointerEventData.pointerDrag, pointer.pointerEventData, ExecuteEvents.dragHandler );
				ExecuteEvents.Execute( pointer.pointerEventData.pointerDrag, pointer.pointerEventData, ExecuteEvents.endDragHandler );
			}

			for (int i = 0; i < results.Count; i++)
			{
				ExecuteEvents.ExecuteHierarchy( results[i].gameObject, pointer.pointerEventData, ExecuteEvents.dropHandler );
			}

			pointer.pointerEventData.pointerDrag = null;
			pointer.pointerEventData.dragging = false;
			return;
		}

		if ( pointer.hoveringElement == null )
		{
			if ( pointer.pointerEventData.pointerDrag != null )
			{
				ResetItems( pointer, results );
			}
			return;
		}

		if ( pointer.pointerEventData.dragging )
        {
            for ( int i = 0; i < results.Count; i++ )
            {
                RaycastResult result = results[i];
                if ( ValidElement(result.gameObject) == false )
                {
                    continue;
                }

                ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.initializePotentialDrag);
                ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.beginDragHandler);
                GameObject target = ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.dragHandler);
                if ( target != null )
                {
                    pointer.pointerEventData.pointerDrag = target;
                    break;
                }
            }
        }

//		base.Drag( pointer, results );
    }
	
	/*
	protected override void Drag( VRTK_UIPointer pointer, List<RaycastResult> results )
	{
        pointer.pointerEventData.dragging = pointer.pointerEventData.delta != Vector2.zero;

        if (pointer.pointerEventData.pointerDrag)
        {
            if (!ValidElement(pointer.pointerEventData.pointerDrag))
            {
                pointer.pointerEventData.pointerDrag = null;
                return;
            }

            if (pointer.pointerEventData.dragging)
            {
                if (IsHovering(pointer))
                {
                    ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerDrag, pointer.pointerEventData, ExecuteEvents.dragHandler);
                }
            }
            else
            {
                ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerDrag, pointer.pointerEventData, ExecuteEvents.dragHandler);
                ExecuteEvents.ExecuteHierarchy(pointer.pointerEventData.pointerDrag, pointer.pointerEventData, ExecuteEvents.endDragHandler);
                for (int i = 0; i < results.Count; i++)
                {
                    ExecuteEvents.ExecuteHierarchy(results[i].gameObject, pointer.pointerEventData, ExecuteEvents.dropHandler);
                }
                pointer.pointerEventData.pointerDrag = null;
            }
        }
        else if (pointer.pointerEventData.dragging)
        {
            for (int i = 0; i < results.Count; i++)
            {
                RaycastResult result = results[i];
                if (!ValidElement(result.gameObject))
                {
                    continue;
                }

                ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.initializePotentialDrag);
                ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.beginDragHandler);
                GameObject target = ExecuteEvents.ExecuteHierarchy(result.gameObject, pointer.pointerEventData, ExecuteEvents.dragHandler);
                if (target != null)
                {
                    pointer.pointerEventData.pointerDrag = target;
                    break;
                }
            }
        }
	}
	*/
	protected override void Scroll(VRTK_UIPointer pointer, List<RaycastResult> results)
    {
        pointer.pointerEventData.scrollDelta = (pointer.controller != null ? pointer.controller.GetTouchpadAxis() : Vector2.zero);
        bool scrollWheelVisible = false;
        for (int i = 0; i < results.Count; i++)
        {
            if (pointer.pointerEventData.scrollDelta != Vector2.zero)
            {
                GameObject target = ExecuteEvents.ExecuteHierarchy(results[i].gameObject, pointer.pointerEventData, ExecuteEvents.scrollHandler);
                if (target != null)
                {
                    scrollWheelVisible = true;
                }
            }
        }

        if (pointer.controllerRenderModel != null)
        {
            VRTK_SDK_Bridge.SetControllerRenderModelWheel(pointer.controllerRenderModel, scrollWheelVisible);
        }
    }

}
