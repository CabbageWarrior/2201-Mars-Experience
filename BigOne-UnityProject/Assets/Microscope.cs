using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microscope : MonoBehaviour
{
    public GameEvent SlideOnMicroscope;

    private bool isAlreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isAlreadyTriggered && other.tag == "Slides")
        {
            if (other.gameObject.name == "Slide5")
            {
                isAlreadyTriggered = true;

                if (SlideOnMicroscope != null)
                {
                    SlideOnMicroscope.Invoke();
                }
            }
        }
    }
}