using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeriodicTableElement : MonoBehaviour
{
    [Header("Chemical Element Properties")]
    public ChemicalElementScriptableObjectClass chemicalElementScriptableObject;

    [Space]
    [Header("Component Properties")]
    public MoleculePuzzleManager moleculePuzzleManager;

    public void SelectElement()
    {
        if (!moleculePuzzleManager)
        {
            Debug.LogError(gameObject.name + " - Molecule Puzzle Manager Reference Missing!");
            return;
        }

        moleculePuzzleManager.SelectElement(this);
    }
    
    public void FillDataInScene()
    {
        string formattedAtomicNumber = "000" + chemicalElementScriptableObject.atomicNumber.ToString();
        formattedAtomicNumber = formattedAtomicNumber.Substring(formattedAtomicNumber.Length - 3);

        // Aggiorno il nome dell'elemento
        this.gameObject.name = "Element_" + formattedAtomicNumber + "_" + chemicalElementScriptableObject.elementSymbol;
        this.transform.Find("Text").GetComponent<Text>().text = chemicalElementScriptableObject.elementSymbol;

        // Metto il manager se non c'è
        if (!this.moleculePuzzleManager)
        {
            try
            {
                this.moleculePuzzleManager = GameObject.Find("MoleculePuzzle").GetComponent<MoleculePuzzleManager>();
            }
            catch (Exception e)
            {
                Debug.LogError("Non esiste nessun oggetto \"MoleculePuzzle\"!!!");
            }
        }
    }
}
