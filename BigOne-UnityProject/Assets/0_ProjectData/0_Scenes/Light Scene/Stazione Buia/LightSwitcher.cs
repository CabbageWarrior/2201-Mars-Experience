using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour {

	public Texture2D isNight0Dir;
	public Texture2D isNight0Light;
	//

	public Texture2D isNight1Dir;
	public Texture2D isNight1Light;
	//
	public Texture2D isNight2Dir;
	public Texture2D isNight2Light;
	//
	public Texture2D isNight3Dir;
	public Texture2D isNight3Light;
	//
	public Texture2D isNight4Dir;
	public Texture2D isNight4Light;
	//
	public Texture2D isNight5Dir;
	public Texture2D isNight5Light;
	[Space]
	public Texture2D isDay0Dir;
	public Texture2D isDay0Light;
	//
	public Texture2D isDay1Dir;
	public Texture2D isDay1Light;
	//
	public Texture2D isDay2Dir;
	public Texture2D isDay2Light;
	//
	public Texture2D isDay3Dir;
	public Texture2D isDay3Clo;
	//
	public Texture2D isDay4Dir;
	public Texture2D isDay4Light;
	//
	public Texture2D isDay5Dir;
	public Texture2D isDay5Light;
	[Space]
	public bool energyActive;

	private LightmapData[] lightMapsDark = new LightmapData[6];
	private LightmapData[] lightMapsLight = new LightmapData[6];



	// Use this for initialization
	void Start () {
		lightMapsDark[0] = new LightmapData();
		lightMapsDark [0].lightmapDir =isDay0Dir;
		lightMapsDark [0].lightmapColor =isNight0Light;
		//
		lightMapsDark[1] = new LightmapData();
		lightMapsDark [1].lightmapDir = isNight1Dir;
		lightMapsDark [1].lightmapColor = isNight1Light;
		//
		lightMapsDark[2] = new LightmapData();
		lightMapsDark [2].lightmapDir = isNight2Dir;
		lightMapsDark [2].lightmapColor = isNight2Light;
		//
		lightMapsDark[3] = new LightmapData();
		lightMapsDark [3].lightmapDir = isNight3Dir;
		lightMapsDark [3].lightmapColor = isNight3Light;
		//
		lightMapsDark[4] = new LightmapData();
		lightMapsDark [4].lightmapDir = isNight4Dir;
		lightMapsDark [4].lightmapColor = isNight4Light;
		//
		lightMapsDark[5] = new LightmapData();
		lightMapsDark [5].lightmapDir = isNight5Dir;
		lightMapsDark [5].lightmapColor = isNight5Light;
		//
		// The end of darkness
		//
		lightMapsLight[0] = new LightmapData();
		lightMapsLight [0].lightmapDir = isDay0Dir;
		lightMapsLight [0].lightmapColor = isDay0Light;
		//
		lightMapsLight[1] = new LightmapData();
		lightMapsLight [1].lightmapDir = isDay1Dir;
		lightMapsLight [1].lightmapColor = isDay1Light;
		//
		lightMapsLight[2] = new LightmapData();
		lightMapsLight [2].lightmapDir = isDay2Dir;
		lightMapsLight [2].lightmapColor = isDay2Light;
		//
		lightMapsLight[3] = new LightmapData();
		lightMapsLight [3].lightmapDir = isDay3Dir;
		lightMapsLight [3].lightmapColor = isDay3Clo;
		//
		lightMapsLight[4] = new LightmapData();
		lightMapsLight [4].lightmapDir = isDay4Dir;
		lightMapsLight [4].lightmapColor = isDay4Light;
		//
		lightMapsLight[5] = new LightmapData();
		lightMapsLight [5].lightmapDir = isDay4Dir;
		lightMapsLight [5].lightmapColor = isDay4Light;
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
