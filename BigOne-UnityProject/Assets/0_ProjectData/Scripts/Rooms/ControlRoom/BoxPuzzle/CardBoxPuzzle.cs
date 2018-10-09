// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Highlighters;

 
public class CardBoxPuzzle : Puzzle {

	public	GameEvent			m_OnTetraSet			= null;
//	public	GameEvent			m_OnTourchSet			= null;

	[SerializeField]
	private VRTK_SnapDropZone	m_SnapZoneTetra1		= null;

	[SerializeField]
	private VRTK_SnapDropZone	m_SnapZoneTetra2		= null;

	//[SerializeField]
	//private VRTK_SnapDropZone	m_SnapZoneTourchBox		= null;

	[SerializeField]
	private	ControlledDrawer	m_ControlledDrawer		= null;


	private	Animator			m_Animator				= null;

                                                   
	//////////////////////////////////////////////////////////////////////////
	// START
	protected void Start()
	{
		m_Animator = GetComponent<Animator>();

		m_SnapZoneTetra1.ObjectSnappedToDropZone		+= new SnapDropZoneEventHandler ( OnTetra1Snapped );
		m_SnapZoneTetra2.ObjectSnappedToDropZone		+= new SnapDropZoneEventHandler ( OnTetra2Snapped );
		//m_SnapZoneTourchBox.ObjectSnappedToDropZone		+= new SnapDropZoneEventHandler ( OnCubeSnapped );

		//m_SnapZoneTourchBox.ObjectUnsnappedFromDropZone	+= new SnapDropZoneEventHandler ( OnCubeUnSnapped );

		//m_SnapZoneTourchBox.enabled = false;
	}


#region SNAP

	//////////////////////////////////////////////////////////////////////////
	// OnTetra1Snapped
	void OnTetra1Snapped( object o, SnapDropZoneEventArgs e )
	{
		GameObject snappedObject = e.snappedObject;
		var distGrab = snappedObject.GetComponent<GrabbableAtDistance>();

		distGrab.IsAllowed = distGrab.InteractableObject.isGrabbable = false;
		distGrab.InteractableObject.Unhighlight();
		distGrab.InteractObjectHighlighter.touchHighlight = Color.clear;
		distGrab.OutlineObjectCopyHighlighter.active = false;

		CheckPuzzleStatus ();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnTetra2Snapped
	void OnTetra2Snapped( object o, SnapDropZoneEventArgs e )
	{
		GameObject snappedObject = e.snappedObject;
        var distGrab = snappedObject.GetComponent<GrabbableAtDistance>();

		distGrab.IsAllowed = distGrab.InteractableObject.isGrabbable = false;
		distGrab.InteractableObject.Unhighlight();
		distGrab.InteractObjectHighlighter.touchHighlight = Color.clear;
		distGrab.OutlineObjectCopyHighlighter.active = false;

		CheckPuzzleStatus ();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnCubeSnapped
	void OnCubeSnapped( object o, SnapDropZoneEventArgs e )
	{
		GameObject snappedObject = e.snappedObject;
		var distGrab = snappedObject.GetComponent<GrabbableAtDistance>();
		if ( distGrab != null )
			distGrab.IsAllowed = false;

        if ( IsCompleted )
        {
            distGrab.IsAllowed = true;
            return;
        }

		CheckPuzzleStatus ();
	}

#endregion

////////////////////////////////////////////////////////////////////////////////////

#region UNSNAP

	//////////////////////////////////////////////////////////////////////////
	// OnCubeUnSnapped
	void OnCubeUnSnapped( object o, SnapDropZoneEventArgs e )
	{
		GameObject snappedObject = e.snappedObject;
		var distGrab = snappedObject.GetComponent<GrabbableAtDistance>();
		if ( distGrab != null )
			distGrab.IsAllowed = true;

		//m_SnapZoneTourchBox.ForceUnsnap();
		//m_SnapZoneTourchBox.enabled = false;
	}

#endregion


	//////////////////////////////////////////////////////////////////////////
	// CheckPuzzleStatus
	private	void	CheckPuzzleStatus ()
	{
		if ( IsCompleted )
			return;

		if ( m_SnapZoneTetra1.GetCurrentSnappedObject() != null && m_SnapZoneTetra2.GetCurrentSnappedObject() != null )
		{
			if ( m_OnTetraSet != null )
			{
				m_OnTetraSet.Invoke();
                m_ControlledDrawer.Activate();
                OnCompletion();
                //                m_SnapZoneTourchBox.enabled = true;
//                m_Animator.Play( "Animate" );
			}
		}

		//if ( m_SnapZoneTourchBox.GetCurrentSnappedObject() != null )
		//{
		//	if ( m_OnTourchSet != null )
		//	{
		//		GameObject tourchOBJ = m_SnapZoneTourchBox.GetCurrentSnappedObject();
		//		tourchOBJ.GetComponent<GrabbableAtDistance>().IsAllowed = true;
		//		m_OnTourchSet.Invoke();
				
		//	}
		//}
	}

}
