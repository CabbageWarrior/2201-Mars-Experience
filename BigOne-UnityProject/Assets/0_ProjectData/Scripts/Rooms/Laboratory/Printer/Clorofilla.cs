using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Clorofilla : MonoBehaviour {

	private void Start()
	{
		GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += new SnapDropZoneEventHandler( ObjectSnappedToDropZone );
	}

	void ObjectSnappedToDropZone( object sender, SnapDropZoneEventArgs e )
	{
	
			Destroy(e.snappedObject.gameObject);
			Debug.Log( "Clorofilla Inserita" );
			this.gameObject.SetActive(false);
			
	}

}
