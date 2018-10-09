using BigOne.PuzzleComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleculePuzzleManager : Puzzle
{
    // Public
    public List<AtomContainer> atomPanels;
    public GameObject panelQuantity;
    [Header("Selected Atom Infos")]
    public Text atomNumber;
    public Text atomSymbol;
    public Text atomName;
    [Space]
    public KeypadManager keypadLab;
    public Text userQuantity;
    [Space]
    public GameObject errorGameObject;
    [Header("Correct Atoms Combination")]
    public List<string> correctAtomsAndQuantities;
    [Header("Periodic Table Groups Buttons")]
    public List<VR_UI_Interactable_Button> groupsButtons;
    [Header("Periodic Table Groups Panels")]
    public List<GameObject> groups;

    // Not public
    GameObject lastPanel;
    IEnumerator errorCoroutine;

    public void SelectElement(PeriodicTableElement periodicTableElement)
    {
        lastPanel = periodicTableElement.transform.parent.gameObject;
        lastPanel.SetActive(false);

        atomNumber.text = periodicTableElement.chemicalElementScriptableObject.atomicNumber.ToString();
        atomSymbol.text = periodicTableElement.chemicalElementScriptableObject.elementSymbol;
        atomName.text = periodicTableElement.chemicalElementScriptableObject.elementName;

        panelQuantity.SetActive(true);

        //userQuantity.text = "";
		keypadLab.EmptyValue();
        keypadLab.isPuzzleEnabled = true;
    }

    public void ReturnToLastPanel()
    {
        panelQuantity.SetActive(false);
        lastPanel.SetActive(true);
        lastPanel = null;
    }

    public void ConfirmAtomQuantity()
    {
        keypadLab.Internal.IsCompleted = false;

        int atomsToCreate = 0;
        if (userQuantity.text != "") atomsToCreate = int.Parse(userQuantity.text);

        if (atomsToCreate == 0) return;

        AtomContainer firstAvailableAtomContainer = atomPanels.Find(x => x.isSlotAvailable);

        if (!firstAvailableAtomContainer) return;

        firstAvailableAtomContainer.GenerateAtoms(atomSymbol.text, atomsToCreate);

        ReturnToLastPanel();
    }

	public void ResetButtonsColors()
	{
		foreach(VR_UI_Interactable_Button btn in groupsButtons)
		{
			btn.SetEnabled(true);
		}
	}

    public void ShowError()
    {
        if (errorCoroutine != null)
        {
            StopCoroutine(errorCoroutine);
        }
        errorCoroutine = ShowError_Coroutine();
        StartCoroutine(errorCoroutine);
    }

    public IEnumerator ShowError_Coroutine()
    {
        errorGameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        errorGameObject.SetActive(false);
    }

    #region Callbacks
    public IEnumerator Correct()
    {
        OnCompletion();
        yield return null;
    }

    public IEnumerator Wrong()
    {
        ShowError();
        yield return null;
    }
    #endregion

    public void CheckCode()
    {
        bool isCombinationCorrect = true;
        List<string> currentSelectedAtoms = new List<string>();

        foreach (var ac in atomPanels)
        {
            if (ac.isSlotAvailable)
            {
                isCombinationCorrect = false;
                break;
            }

            currentSelectedAtoms.Add(ac.elementName + ac.nAtoms.ToString());
        }

        if (isCombinationCorrect)
        {
            foreach (var el in correctAtomsAndQuantities)
            {
                if (!currentSelectedAtoms.Exists(x => x == el))
                {
                    isCombinationCorrect = false;
                    break;
                }
            }
        }

        if (isCombinationCorrect)
        {
            StartCoroutine(Correct());
        }
        else
        {
            StartCoroutine(Wrong());
        }
    }
}
