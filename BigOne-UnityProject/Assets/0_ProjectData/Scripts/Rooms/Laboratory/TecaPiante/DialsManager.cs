using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;
using VRTK.Examples;

public class DialsManager : Puzzle
{
    public GameEvent CardGrabbed;

    public Transform spriteOx;
    public Transform spriteTemp;
    Animator anim;

    [Header("Body Elements")]
    public GameObject cylinder;
    public GameObject card;
	public Transform dialOX;
	public Transform dialTemp;
	//public GameObject cube;
    [Header("Check Routine")]
    public GameObject[] barCheck;
    public float checkTickDelay = .5f;

	[Header("Check Values")]
	public float oxigenMinScaleValue;
	public float oxigenMaxScaleValue;
	public float temperatureMinScaleValue;
	public float temperatureMaxScaleValue;

    private int countCheck = 0;
	private	VRTK_InteractableObject cardInteractable;

    private void Start()
    {
        anim = GetComponent<Animator>();
		dialOX = transform.Find("manopola_SX");
		dialTemp = transform.Find("manopola_DX");
		if ( card != null )
		{
			cardInteractable = card.GetComponent<VRTK_InteractableObject>();
			cardInteractable.InteractableObjectGrabbed += CardInteractable_InteractableObjectGrabbed;
		}
    }

	private void CardInteractable_InteractableObjectGrabbed( object sender, InteractableObjectEventArgs e )
	{
		CardGrabbed.Invoke();
		cardInteractable.InteractableObjectGrabbed -= CardInteractable_InteractableObjectGrabbed;
	}

    public IEnumerator CountDown()
    {
        yield return new WaitForSeconds(checkTickDelay);
        for (int i = 0; i < barCheck.Length; i++)
        {
            barCheck[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(checkTickDelay);
            countCheck++;

            if (countCheck >= barCheck.Length)
            {
                countCheck = barCheck.Length;
                CheckValues();
            } 
        }
    }

    public void RestartCountDown()
    {
        countCheck = 0;

        for (int i = 0; i < barCheck.Length; i++)
        {
            if (barCheck[i].activeInHierarchy)
            {
                barCheck[i].SetActive(false);
            }
        }
    }

    public void OpenCase()
    {
        anim.Play("OpenCase");
		dialOX.GetComponent<VRTK.Controllables.PhysicsBased.VRTK_PhysicsRotator>().isLocked = true;
		dialTemp.GetComponent<VRTK.Controllables.PhysicsBased.VRTK_PhysicsRotator>().isLocked = true;
        if (this.gameObject.name == "Teca5")
        {
            //cube.SetActive(false);
            card.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
            card.GetComponent<GrabbableAtDistance>().IsAllowed = true;
        }
    }

    void CheckValues()
    {
        if (spriteOx.localScale.x > oxigenMinScaleValue && spriteOx.localScale.x < oxigenMaxScaleValue && spriteTemp.localScale.x > temperatureMinScaleValue && spriteTemp.localScale.x < temperatureMaxScaleValue)
        {
            OnCompletion();
        }
        else
        {
            cylinder.GetComponent<MeshRenderer>().material.color = Color.red;
            //Debug.Log("Sbagliato! Capra! Capra! Capra!");
        }
    }
}
