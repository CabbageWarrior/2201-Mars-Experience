using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "New Chemical Element", menuName = "2201 Mars Experience/Scriptable Objects/Chemical Element", order = 1)]
public class ChemicalElementScriptableObjectClass : ScriptableObject
{
    public int atomicNumber;
    public string elementSymbol;
    public string elementName;

    public void UpdateData()
    {
        string formattedAtomicNumber = "000" + this.atomicNumber.ToString();
        formattedAtomicNumber = formattedAtomicNumber.Substring(formattedAtomicNumber.Length - 3);

        // Aggiorno il nome dell'elemento
        string newName = "Element_" + formattedAtomicNumber + "_" + this.elementSymbol;


        //this.name = "Element_" + formattedAtomicNumber + "_" + this.elementSymbol;

#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, newName);
        AssetDatabase.SaveAssets();
#endif
    }
}