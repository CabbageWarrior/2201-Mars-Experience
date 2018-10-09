using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRTK.Examples;
using FMODUnity;
using DG.Tweening;

public class SliderManager : Puzzle
{
    Sliders[] sliders;
   // public Text checkText;
    public Light[] panelLights; //Red leds of generator

	[Space]
	//[FMODUnity.EventRef]     //Declare FMOD event (can also declare snapshots and parameters)
	public GameObject accensioneStazioneGO;
	private StudioEventEmitter accensioneStazioneEmitter;

   /* public IEnumerator RotateObject()
    {
        leva.transform.DORotate(new Vector3(leva.transform.eulerAngles.x, leva.transform.eulerAngles.y, -102),0.5f);
        yield return new WaitForSeconds(1);
        coroutineStarted = false;
    }
    */
    protected void Start()
    {
        sliders = GameObject.FindObjectsOfType<Sliders>();
        //Debug.Log(sliders.Length);
		accensioneStazioneEmitter = accensioneStazioneGO.GetComponent<StudioEventEmitter> ();
    }

    public bool CheckSliders()
    {
        if (IsCompleted) return false;

        int correctCount = 0;
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i].isCorrect)
            {
                correctCount++;
                //Debug.Log(correctCount);
            }
        }

        if (correctCount == sliders.Length)
        {
           // checkText.text = "Correct";
           // ChangeColor();
            OnCompletion();
            for (int i = 0; i < sliders.Length; i++)
            {
                Destroy(sliders[i].GetComponent<BoxCollider>());
            }
            return true;
        }
        else 
        {
            //Debug.Log("Wrong");
            // checkText.text = "Wrong";
            OnError();
            return false;
            
        }
    }

    public void ChangeColor()
    {
        for (int i = 0; i < panelLights.Length; i++)
        {
            panelLights[i].color = Color.green;
        }
    }

	public void accensioneStazioneTriggerCue()
	{
		accensioneStazioneEmitter.EventInstance.triggerCue ();
	}

   
}