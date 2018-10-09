using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    public GameObject testa;

    void Awake() {
        this.transform.position = testa.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = testa.transform.position;
    }
}
