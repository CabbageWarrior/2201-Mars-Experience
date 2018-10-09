using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch: MonoBehaviour {

	public Texture2D isNight0Dir;
	public Texture2D isNight0Light;
	//
	[Space]
	public Texture2D isDay0Dir;
	public Texture2D isDay0Light;
	//
	[Space]
	public bool energyActive;

	private LightmapData[] lightMapsDark = new LightmapData[1];
	private LightmapData[] lightMapsLight = new LightmapData[1];



	// Use this for initialization
	void Start () {
		lightMapsDark[0] = new LightmapData();
		lightMapsDark [0].lightmapDir =isDay0Dir;
		lightMapsDark [0].lightmapColor =isNight0Light;
		//
		//
		// The end of darkness
		//
		lightMapsLight[0] = new LightmapData();
		lightMapsLight [0].lightmapDir = isDay0Dir;
		lightMapsLight [0].lightmapColor = isDay0Light;
		//
		//
		// The end of the light
		//
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Switch ();
		}
	}

	void Switch(){
		energyActive = !energyActive;
		Debug.Log (energyActive);

		if (!energyActive) {
			LightmapSettings.lightmaps = lightMapsDark;
			Debug.Log ("Is Dark");
		}
			
		if (energyActive) {
			LightmapSettings.lightmaps = lightMapsLight;
			Debug.Log ("There's light ");
		}
	}
}
