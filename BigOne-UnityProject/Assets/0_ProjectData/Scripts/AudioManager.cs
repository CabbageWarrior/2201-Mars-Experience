using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public Slider volumeReferenceSlider;

    private Bus busVolume;

    // Use this for initialization
    void Start()
    {
        busVolume = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    //// Update is called once per frame
    //void Update()
    //{
    //
    //}

    public void SetVolume()
    {
        if (volumeReferenceSlider)
            SetVolume(volumeReferenceSlider.value);
    }

    public void SetVolume(float volume)
    {
        FMOD.RESULT busVolumeResult = busVolume.setVolume(volume);

        //Debug.Log(busVolumeResult);
    }
}
