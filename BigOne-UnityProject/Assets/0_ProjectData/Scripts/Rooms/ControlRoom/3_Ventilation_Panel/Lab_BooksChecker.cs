using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab_BooksChecker : MonoBehaviour {

	public	GameEvent OnBookExit = null;

	private void OnTriggerExit( Collider other )
	{
		PropItem prop = other.GetComponent<PropItem>();
		if ( prop != null && prop.Type == PropItem.PropType.BOOK )
		{
			if ( OnBookExit != null )
				OnBookExit.Invoke();

			Destroy( gameObject );
		}
	}

}
