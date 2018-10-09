using UnityEngine;
using VRTK;

public class PanelManager : MonoBehaviour {
	public Animator anim;
	// public GameObject passMorse;
	public GameObject letter;

	[Space]
	public  GameEvent m_OnTesseraSnapped = null;

	void Start()
	{
		GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += new SnapDropZoneEventHandler( ObjectSnappedToDropZone );
		GetComponent<Renderer>().enabled = true;
	}

	void OnTriggerEnter( Collider col )
	{
		if ( col.gameObject.name == "IdentityCard" && !col.gameObject.GetComponent<VRTK_InteractableObject>().IsGrabbed() )
		{
			ExecuteCallback( col.gameObject );
		}
	}

	void ObjectSnappedToDropZone( object sender, SnapDropZoneEventArgs e )
	{
		if ( e.snappedObject.name == "IdentityCard" )
		{
			ExecuteCallback( e.snappedObject.gameObject );
			//passMorse.SetActive(true);
		}
	}

	void ExecuteCallback( GameObject gobj )
	{
		//Destroy( gobj );
		gobj.SetActive(false);

		//Debug.Log( "Tessera Inserita" );
		letter.SetActive( true );
		anim.Play( "SlidingPanel" );
		if ( m_OnTesseraSnapped != null )
		{
			m_OnTesseraSnapped.Invoke();
		}
	}
}