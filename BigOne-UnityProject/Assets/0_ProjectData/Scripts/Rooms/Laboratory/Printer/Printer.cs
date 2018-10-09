using UnityEngine;
using VRTK;
using UnityEngine.UI;
using System.Collections;

public class Printer : MonoBehaviour {
	// public GameObject printerText;  // Text associated to the 3D printer

    public GameEvent CardSnapped = null;
    public GameEvent ChlorophyllPrinted = null;


    public	bool isPrinterOn = false;       // Checks whether the printer is on
	public	bool isReadyToPrint = false;
	public	bool is3DModelPrinted = false;  // Checks whether the 3D model has already been printed
	public	float waitTimeBeforePrint = 0;	// Wait time before the molecule is activated
	public	GameObject	printButton;
	MeshRenderer meshRenderer;
	public GameObject clorofilla;
	public Animator anim;


	private void Start()
	{
        meshRenderer = GetComponent<MeshRenderer>();
        isPrinterOn = true;

		// printerText.SetActive( false );
		//GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += ObjectSnappedToDropZone;
	}
	

//	void ObjectSnappedToDropZone( object sender, SnapDropZoneEventArgs e )
//	{
//		if ( e.snappedObject.name == "PrinterCard" )
//		{
//			isPrinterOn = true;
//            CardSnapped.Invoke();
//			UpdateState();
//		}
//	}

	
	public void ActivatePrintMode()
	{
		isReadyToPrint = true;
		UpdateState();
	}
	

	public void Print()
	{
		if ( is3DModelPrinted == false)
		{
			is3DModelPrinted = true;
            anim.Play( "Print" );
			StartCoroutine(WaitBeforePrint());
            
		}
	}

	public void UpdateState()
	{
		if ( isPrinterOn )
		{
			if ( isReadyToPrint )
			{
				printButton.SetActive( true );
			}
			// printerText.SetActive( true );
		}
	}

	IEnumerator WaitBeforePrint()
	{
		yield return new WaitForSeconds(waitTimeBeforePrint);
		 clorofilla.SetActive(true);
		 clorofilla.GetComponent<GrabbableAtDistance>().IsAllowed = true;
            if (ChlorophyllPrinted != null)
            {
                ChlorophyllPrinted.Invoke();
            }
	}

}