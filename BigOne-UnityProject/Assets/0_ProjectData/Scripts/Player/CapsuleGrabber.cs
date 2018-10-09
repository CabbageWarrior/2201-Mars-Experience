// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Highlighters;



public class CapsuleGrabber : MonoBehaviour {

	public	VRTK_Pointer_Extension		pointer						= null;

	public	float						grabDistance				= 2f;

	[System.Serializable]
	public	class DistGrabbable
	{
		public GrabbableAtDistance		pObject						= null;
		public bool						bHighlithed					= false;

		public DistGrabbable( GrabbableAtDistance obj ) {
			pObject = obj;
		}
	}

	[SerializeField]
	private	List<DistGrabbable>			m_Grabbables				= null;

	private	DistGrabbable				m_BestObject				= null;
	public	GrabbableAtDistance			BestObject
	{
		get { return m_BestObject != null ? m_BestObject.pObject : null; }
	}

	private	bool						m_IsEnabled					= true;
	public	bool						IsEnabled
	{
		get { return m_IsEnabled; }
		set
		{
			m_IsEnabled = value;
			m_Collider.enabled = value;
			if ( value == false )
			{
				ResetGrabbablesList();
			}
		}
	}

	private	CapsuleCollider				m_Collider					= null;
	private InteractObjectHighlighterEventArgs emptyEvent = new InteractObjectHighlighterEventArgs();


	//////////////////////////////////////////////////////////////////////////
	// START
	private	void Awake ()
	{	
		name = "GrabberCapsule";

		m_Grabbables = new List<DistGrabbable>();
		  
		m_Collider = GetComponent<CapsuleCollider>();
        m_Collider.isTrigger = true;

        transform.localEulerAngles = new Vector3( 90f, 0f, 0f );

		// ignore self colliding
		gameObject.layer = 2;
		gameObject.tag = "HandController";

		Destroy( GetComponent<MeshRenderer>() );
	}


	//////////////////////////////////////////////////////////////////////////
	// ManualRemove
	public void ManualRemove( GrabbableAtDistance obj )
	{
		DistGrabbable objFound = m_Grabbables.Find ( g => g.pObject == obj );

		// no found
		if ( objFound == null ) return;

		// remove last reference
		if ( m_BestObject == objFound )
		{
			m_BestObject = null;
		}

		// remove object
		m_Grabbables.Remove( objFound );
	}

#region MATH

	private Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point){		
 
		//get vector from point on line to point in space
		Vector3 linePointToPoint = point - linePoint;
 
		float t = Vector3.Dot(linePointToPoint, lineVec);
 
		return linePoint + lineVec * t;
	}

	private int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point){
 
		Vector3 lineVec = linePoint2 - linePoint1;
		Vector3 pointVec = point - linePoint1;
 
		float dot = Vector3.Dot(pointVec, lineVec);
 
		//point is on side of linePoint2, compared to linePoint1
		if(dot > 0){
 
			//point is on the line segment
			if(pointVec.magnitude <= lineVec.magnitude){
 
				return 0;
			}
 
			//point is not on the line segment and it is on the side of linePoint2
			else{
 
				return 2;
			}
		}
 
		//Point is not on side of linePoint2, compared to linePoint1.
		//Point is not on the line segment and it is on the side of linePoint1.
		else{
 
			return 1;
		}
	}

	private Vector3 ProjectPointOnLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point){
 
		Vector3 vector = linePoint2 - linePoint1;
 
		Vector3 projectedPoint = ProjectPointOnLine(linePoint1, vector.normalized, point);
 
		int side = PointOnWhichSideOfLineSegment(linePoint1, linePoint2, projectedPoint);
 
		//The projected point is on the line segment
		if(side == 0){
 
			return projectedPoint;
		}
 
		if(side == 1){
 
			return linePoint1;
		}
 
		if(side == 2){
 
			return linePoint2;
		}
 
		//output is invalid
		return Vector3.zero;
	}

	private	float PointDistanceToLine( Vector3 linePoint1, Vector3 linePoint2, Vector3 pointVect )
	{
		Vector3 projectedPoint = ProjectPointOnLineSegment( linePoint1, linePoint2, pointVect );
		Vector3 vector = projectedPoint - pointVect;
		return vector.magnitude;

	}


#endregion


	//////////////////////////////////////////////////////////////////////////
	// ResetGrabbablesList
	private	void	ResetGrabbablesList()
	{
		foreach( DistGrabbable distGrabbable in m_Grabbables )
		{
			if ( distGrabbable.bHighlithed == true )
			{
				distGrabbable.pObject.InteractableObject.Unhighlight();
				distGrabbable.pObject.InteractObjectHighlighter.OnInteractObjectHighlighterUnhighlighted( emptyEvent );
			}
		}

		m_Grabbables.Clear();
	}


	//////////////////////////////////////////////////////////////////////////
	// IsVisibleFrom
	public bool IsVisibleFrom( Renderer renderer, Camera camera )
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes( camera );
		return GeometryUtility.TestPlanesAABB( planes, renderer.bounds );
	}


	//////////////////////////////////////////////////////////////////////////
	// CheckIfBestObj
	private bool	CheckIfBestObj( DistGrabbable obj )
	{
		if ( obj == null || obj.pObject == null ) return false;
		
		// already set as best object
		if ( obj == m_BestObject ) return false;

		// object "is not visible"
		Ray ray = new Ray( Camera.main.transform.position, ( obj.pObject.transform.position - Camera.main.transform.position ) );
		RaycastHit hit;
		bool rayCastResult = Physics.Raycast( ray, out hit );

		// has hit
		if ( rayCastResult )
		{
			// if is visible by camera
			Renderer renderer = obj.pObject.GetComponent<Renderer>();
			if ( renderer != null && IsVisibleFrom( renderer, Camera.main ) == false )
				return false;

			// if hittded obj is different from current object parsed return because this object is not visible
			if ( hit.transform != obj.pObject.transform )
				return false;
		}

		// Another obj should be the best, so unhighlight previous one
		if ( m_BestObject != null && m_BestObject.bHighlithed == true )
		{
			m_BestObject.pObject.InteractableObject.Unhighlight();
			obj.pObject.InteractObjectHighlighter.OnInteractObjectHighlighterUnhighlighted( emptyEvent );
			m_BestObject.bHighlithed = false;
		}

		// highlight current obj
		if ( obj.bHighlithed == false )
		{
			obj.pObject.InteractableObject.Highlight( obj.pObject.InteractObjectHighlighter.touchHighlight );
			obj.pObject.InteractObjectHighlighter.OnInteractObjectHighlighterHighlighted( emptyEvent );
			obj.bHighlithed = true;
		}

		// set as best object
		m_BestObject = obj;
		return true;

	}


	//////////////////////////////////////////////////////////////////////////
	// FixedUpdate
	private void FixedUpdate()
	{
		// skip if no elements in list
		if ( m_Grabbables.Count == 0 ) return;


		// if only one is in the list
		if ( m_Grabbables.Count == 1 )
		{	// set as the current object and
			// check if already set as current best object
			CheckIfBestObj( m_Grabbables[0] );
			return;
		}
		                               

		// set as best object the nearest  one whose is nearest to forward line of capsule
		float currentMinDistance = float.MaxValue;
		DistGrabbable currentObject = null;

		for ( int i = 0; i < m_Grabbables.Count; i++ )
		{
			DistGrabbable obj = m_Grabbables[i];

			if ( obj == null || obj.pObject ==  null )
			{
				m_Grabbables.RemoveAt(i);
				return;
			}


			float nextDistance = PointDistanceToLine
			( 
				linePoint1 : transform.parent.position,
				linePoint2 : transform.parent.position + transform.parent.eulerAngles.normalized * transform.localScale.y,
				pointVect : obj.pObject.transform.position
			);

			if ( nextDistance < currentMinDistance )
			{
				currentMinDistance = nextDistance;
				currentObject = obj;
			}
		}

        CheckIfBestObj( currentObject );

	}


    //////////////////////////////////////////////////////////////////////////
    // EvaluateDestinationPoint
    private void	EvaluateDestinationPoint( ref Collider other )
	{
		DestinationPoint	destPoint = other.GetComponent<DestinationPoint>();

        if (destPoint != null)
        {
            Vector3 rayStart = pointer.customOrigin.position;
            Vector3 direction = (( destPoint.transform.position + ( destPoint.Collider as CapsuleCollider).center ) - pointer.transform.position);
            RaycastHit hit;
            if ( Physics.Raycast( rayStart, direction, out hit, transform.localScale.y ) )
            {
                if ( hit.collider != destPoint.Collider )
                {
                    DestinationPoint.currentPointedDestinationPoint = null;
                    return;
                }
            }

        }


        if ( destPoint != null && pointer.CanDoTeleport == true && DestinationPoint.currentPlayerDestinationPoint != destPoint )
		{
			if ( DestinationPoint.currentPointedDestinationPoint != null )
			{
				DestinationPoint.currentPointedDestinationPoint.RestoreLocationIndicator();
			}
			
			if ( destPoint.TeleportEnabled == true )
			{
                DestinationPoint.currentPointedDestinationPoint = destPoint;
				DestinationPoint.currentPointedDestinationPoint.ShowLocationIndicatorGood();
                if (PlayerCommon.Instance.OnDestinationPointSnap != null && PlayerCommon.Instance.OnDestinationPointSnap.GetPersistentEventCount() > 0 )
                    PlayerCommon.Instance.OnDestinationPointSnap.Invoke();

            }
			else
			{
				if ( DestinationPoint.currentPointedDestinationPoint != null )
					DestinationPoint.currentPointedDestinationPoint.ShowLocationIndicatorBad();
			}
		}
	}


    //////////////////////////////////////////////////////////////////////////
    // OnTriggerEnter
    private void OnTriggerEnter( Collider other )
	{
		if ( m_IsEnabled == false )
			return;

		// Evaluate destination point
        if (DestinationPoint.currentPointedDestinationPoint == null)
            EvaluateDestinationPoint( ref other );
		

		// if is not a grabbale at distance item return
		GrabbableAtDistance DistGrabScript = other.GetComponent<GrabbableAtDistance>();
		if ( DistGrabScript == null || DistGrabScript.IsAllowed == false || DistGrabScript.InteractableObject.isGrabbable == false || DistGrabScript.InteractableObject.IsGrabbed() == true )
			return;

		// Check if is in grab range
		if ( ( DistGrabScript.transform.position - pointer.transform.position ).sqrMagnitude > grabDistance * grabDistance )
			return;

		// If object is already added into list of grabbables
		if ( m_Grabbables.Find( t => t.pObject == DistGrabScript ) != null )
			return;

		// add to outlined list
		m_Grabbables.Add( new DistGrabbable( DistGrabScript ) );

	}


    //////////////////////////////////////////////////////////////////////////
    // OnTriggerStay
    private void OnTriggerStay( Collider other )
	{
		if ( m_IsEnabled == false )
			return;
		
		// Evaluate destination point
        if (DestinationPoint.currentPointedDestinationPoint == null)
	    	EvaluateDestinationPoint( ref other );
	}


    //////////////////////////////////////////////////////////////////////////
    // OnTriggerExit
    private void OnTriggerExit( Collider other )
	{	
		// Destination point trigger
		DestinationPoint  destPoint = other.GetComponent<DestinationPoint>();
		if ( destPoint != null && destPoint.TeleportEnabled == true )
		{
			destPoint.RestoreLocationIndicator();
			DestinationPoint.currentPointedDestinationPoint = null;
		}

		// Skip for non distance grabbable objects
		var DistGrabScript = other.transform.GetComponent<GrabbableAtDistance>();
		if ( DistGrabScript == null )
			return;

		DistGrabbable obj = null;
		if ( ( obj = m_Grabbables.Find( t => t.pObject == DistGrabScript  ) ) == null )
			return;

		if ( obj.bHighlithed == true )
		{
			DistGrabScript.InteractableObject.Unhighlight();
			DistGrabScript.InteractObjectHighlighter.OnInteractObjectHighlighterUnhighlighted( emptyEvent );
		}

		// remove from found list
		ManualRemove( DistGrabScript );
	}
	

}
