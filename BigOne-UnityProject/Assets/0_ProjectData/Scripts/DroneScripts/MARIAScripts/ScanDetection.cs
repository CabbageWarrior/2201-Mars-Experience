using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanDetection : MonoBehaviour {
	[Range(0f, 360f)]
	public float viewAngle = 45f;

	[Space]
	public string actuallyHitting;

	[SerializeField]
	private GameObject currentHighlightedObject;

	public GameObject scanSelection;
	public Image scanImage;
	public Text scanText;

	[Space]
	public float activationDelay = 1.0f;

	[Space]
	public GameEvent OnScanSelectionActivation;

	private bool canUse = false;
	private Coroutine resetActivationCoroutine;

	private void Update()
	{
		FindScanTarget();
		DeactivateScanTarget();
	}

	private void FindScanTarget()
	{
		RaycastHit hit;

		Debug.DrawRay( transform.position, transform.forward * 5f, Color.green );

		if ( Physics.Raycast( transform.position, transform.forward, out hit, 5f ) )
		{
			actuallyHitting = ""; //hit.transform.name;

			Transform correctTransform = hit.transform;

			while ( !correctTransform.GetComponent<ScanTarget>() && correctTransform.parent )
				correctTransform = correctTransform.parent;

			actuallyHitting = correctTransform.name;

			//Debug.Log("Sto toccheggiando \"" + actuallyHitting + "\".");

			ScanTarget correctScanTarget = correctTransform.GetComponent<ScanTarget>();

			if ( correctScanTarget )
			{
				//Debug.Log("Sono dentro, ho trovato lo scan target.");


				if ( currentHighlightedObject && correctScanTarget.gameObject == currentHighlightedObject )
				{
					//Debug.Log("Sto toccando sempre lo stesso, che bravo.");
					return;
				}

				if ( currentHighlightedObject )
				{
					currentHighlightedObject.GetComponent<ScanTarget>().overlayCloneInstance.SetActive( false );
				}
				currentHighlightedObject = correctScanTarget.gameObject;
				correctScanTarget.overlayCloneInstance.SetActive( true );

				//Debug.Log("Ho acceso la cosa giusta? " + correctScanTarget.overlayCloneInstance.activeInHierarchy.ToString());
			}
		}
	}

	public void DeactivateScanTarget()
	{
		if ( currentHighlightedObject != null )
		{
			Vector3 dirToObject = ( currentHighlightedObject.transform.position - transform.position ).normalized;
			//float dstToObject = Vector3.Distance(transform.position, currentHighlightedObject.transform.position);

			if ( !( Vector3.Angle( transform.forward, dirToObject ) < viewAngle / 2 ) )
			{
				currentHighlightedObject.GetComponent<ScanTarget>().overlayCloneInstance.SetActive( false );
				currentHighlightedObject = null;
			}
		}
	}

	// Activates description mode when the object is highlighted and clicked on
	public void ScanSelection()
	{
		if ( !canUse )
			return;

		if ( currentHighlightedObject != null )
		{
			scanSelection.SetActive( true );
			scanImage.sprite = currentHighlightedObject.GetComponent<ScanTarget>().infoSprite;
			scanText.text = currentHighlightedObject.GetComponent<ScanTarget>().infoText;

			if ( OnScanSelectionActivation != null )
			{
				OnScanSelectionActivation.Invoke();
			}
		} else
		{
			scanSelection.SetActive( false );
		}

		ResetActivation();
	}

	public void ResetActivation()
	{
		if ( gameObject.activeInHierarchy )
		{
			if ( resetActivationCoroutine != null )
			{
				StopCoroutine( resetActivationCoroutine );
			}
			
			resetActivationCoroutine = StartCoroutine( ResetActivation_Coroutine() );
		}
	}

	private IEnumerator ResetActivation_Coroutine()
	{
		canUse = false;
		yield return new WaitForSeconds( activationDelay );
		canUse = true;
		yield return null;
	}
}
