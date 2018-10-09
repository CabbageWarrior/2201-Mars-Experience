using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

[CustomEditor(typeof(PeriodicTableElement))]
public class PeriodicTableElementInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PeriodicTableElement periodicTableElement = (PeriodicTableElement)target;

        if (GUILayout.Button("Update In Scene"))
        {
            periodicTableElement.FillDataInScene();
        }
    }
}
