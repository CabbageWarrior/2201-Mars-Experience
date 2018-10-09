using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMonitorManager : MonoBehaviour {

    int imageNumber;
    public Image[] monitorImages;

	// Use this for initialization
	void Start () {
        imageNumber = 0;
        
	}
	
	public void NextImage()
    {
        monitorImages[imageNumber].gameObject.SetActive(false);
        if (imageNumber == (monitorImages.Length - 1)) 
        {
            imageNumber = 0;
            monitorImages[imageNumber].gameObject.SetActive(true);
        }
        else
        { 
            imageNumber++;
            monitorImages[imageNumber].gameObject.SetActive(true);
        }
        
    }
}
