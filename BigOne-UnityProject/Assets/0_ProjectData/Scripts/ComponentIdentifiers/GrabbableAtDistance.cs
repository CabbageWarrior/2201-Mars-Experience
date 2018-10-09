// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;
using VRTK.Highlighters;

public class GrabbableAtDistance : MonoBehaviour {

	[ SerializeField ]
	private	bool								m_IsAllowed		= true;
	public	bool								IsAllowed
	{
		get { return m_IsAllowed; }
		set
		{
			OnAllowedSet( value );
		}
	}

	public	float								TouchDistance	= 0.0f;

	private VRTK_InteractableObject				m_InteractableObject = null;
	public VRTK_InteractableObject				InteractableObject
	{
		get{ return m_InteractableObject; }
	}


	private VRTK_InteractObjectHighlighter		m_InteractObjectHighlighter = null;
	public VRTK_InteractObjectHighlighter		InteractObjectHighlighter
	{
		get{ return m_InteractObjectHighlighter; }
	}

	private VRTK_OutlineObjectCopyHighlighter	m_OutlineObjectCopyHighlighter = null;
	public VRTK_OutlineObjectCopyHighlighter	OutlineObjectCopyHighlighter
	{
		get{ return m_OutlineObjectCopyHighlighter; }
	}

	//////////////////////////////////////////////////////////////////////////
	// AWAKE
	private	void	Awake()
	{
		// Get VRTK components
		m_InteractableObject			= GetComponent<VRTK_InteractableObject>();
		m_InteractObjectHighlighter		= GetComponent<VRTK_InteractObjectHighlighter>();
		m_OutlineObjectCopyHighlighter	= GetComponent<VRTK_OutlineObjectCopyHighlighter>();

		// add a mesh collider to improve distance grab
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

		meshCollider.convex = true;
		meshCollider.isTrigger = true;
        meshCollider.inflateMesh = true;
		meshCollider.skinWidth = 0.001f;

        // Useful for Chips only.
        if (GetComponent<Chips>() != null)
        {
		    if ( meshCollider.sharedMesh == null )
		    {
			    MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
			    if ( meshFilter != null )
				    meshCollider.sharedMesh = meshFilter.mesh;
			    else
			    {
				    print( "WARNING: This object need a mesh filter on himself or at last on one child !!!" );
			    }
		    }
        }

        // set as grabbable at dista only if is grabble normally
        VRTK_InteractableObject interactableScript = GetComponent<VRTK_InteractableObject>();

		m_IsAllowed = interactableScript.isGrabbable;

		m_InteractObjectHighlighter.touchHighlight = Color.yellow;
	}

	//////////////////////////////////////////////////////////////////////////
	// OnAllowedSet
	private	void	OnAllowedSet( bool value )
	{
		m_IsAllowed = value;
		m_InteractableObject.isGrabbable = value;
//		m_OutlineObjectCopyHighlighter.active = value;
	}


	//////////////////////////////////////////////////////////////////////////
	// UnSnapIfSnapped
    public	void	UnSnapIfSnapped()
    {
        if ( m_InteractableObject.IsInSnapDropZone() )
        {
            Transform parent = transform.parent;
			if ( parent == null )
				return;

            VRTK_SnapDropZone snapZone = parent.GetComponent<VRTK_SnapDropZone>();
			if ( snapZone == null )
				return;

            snapZone.ForceUnsnap();
        }
    }

}