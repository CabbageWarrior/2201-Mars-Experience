using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheker : MonoBehaviour {

	private bool isVisible = false;
	public  GameObject menu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider coll){
		Debug.Log ("Sei entrato");
		if (Input.GetKey (KeyCode.Space)) {
			if (!isVisible) {
				menu.SetActive (true);
				Debug.Log ("Attiva");
				isVisible = true;
			}
		}
	}

	void OnTriggerExit(Collider coll){
		Debug.Log ("Sei Uscito");
		 if (isVisible)
				menu.SetActive (false);
				Debug.Log ("Menu disattivato");
				isVisible = false;
	}
	// Mi serve per capire se i bottoni funzionano.. :p
	void Notify(GameObject gameob){
		Debug.Log ("Hai premuto " + gameob);
	}
}
