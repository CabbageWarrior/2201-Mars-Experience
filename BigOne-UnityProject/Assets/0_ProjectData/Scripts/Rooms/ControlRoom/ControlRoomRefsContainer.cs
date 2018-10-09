using System.Collections;
using UnityEngine;
using VRTK;

public class ControlRoomRefsContainer : MonoBehaviour
{
    //public GameObject storageRoomDoor;
    //public GameObject generator;
	public	LightSwitcherHUB lightSwitcher;

    public GameObject[] tetraedri;
    public GameObject morseCodePuzzleCanvas;
    public GameObject[] environmentLights;
    public Animator animGenerator;

	public void ActivateGeneratorPanel()
    {
        morseCodePuzzleCanvas.SetActive(false);
        //generator.SetActive(true);
        animGenerator.Play("IsGenOpening");
    }

    public void GeneratorActivated()
	{
		lightSwitcher.TurnOnLights();
    }

    public void CardBoxCompleted()
    {
        for (int i = 0; i < tetraedri.Length; i++)
        {
            tetraedri[i].GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        }
    }

}