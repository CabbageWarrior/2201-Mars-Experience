using System.Collections;
using UnityEngine;
using VRTK;

public class OpenClozeManager : Puzzle
{
    Slots[] slots;
    Animator anim;
    public int correctChips;
	public GameObject[] slides;
	public GameObject slot;
	public GameObject chips;
    public GameObject fogliettoN4;
	

    void Start()
    {
        slots = GameObject.FindObjectsOfType<Slots>();
        anim = GetComponent<Animator>();
        fogliettoN4.GetComponent<GrabbableAtDistance>().IsAllowed = false;
    }


	public IEnumerator Correct()
	{
		
		yield return new WaitForSeconds(0.5f);
		OnCompletion();
	}

	public void UnlockDoor()
	{
	

		for (int i = 0; i < slides.Length; i++)
		{
			slides[i].GetComponent<GrabbableAtDistance>().IsAllowed = true;
		}

		chips.SetActive(false);
		slot.SetActive(false);
        anim.SetTrigger("IsOpen");
        fogliettoN4.GetComponent<GrabbableAtDistance>().IsAllowed = true;
        
    }

	public void CheckPuzzle()
	{
		if (correctChips == 4)
		{
			StartCoroutine(Correct());
		}
	}
}