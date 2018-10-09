using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

[CustomEditor(typeof(ChemicalElementScriptableObjectClass))]
public class ChemicalElementScriptableObjectClassInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChemicalElementScriptableObjectClass elem = (ChemicalElementScriptableObjectClass)target;

        if (GUILayout.Button("Update"))
        {
            elem.UpdateData();
        }
    }
}
