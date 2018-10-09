using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

[CustomEditor(typeof(MoleculePuzzleManager))]
public class MoleculePuzzleManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MoleculePuzzleManager moleculePuzzleManager = (MoleculePuzzleManager)target;

        if (GUILayout.Button("Update All Elements In Scene"))
        {
            PeriodicTableElement[] periodicTableElements;
            List<GameObject> groups = moleculePuzzleManager.groups;

            foreach (GameObject group in groups)
            {
                group.SetActive(true);
            }

            periodicTableElements = GameObject.FindObjectsOfType<PeriodicTableElement>();

            foreach (PeriodicTableElement pde in periodicTableElements)
            {
                try {
                    pde.FillDataInScene();
                }
                catch (Exception e)
                {
                    pde.name = "ElementToFix";
                    Debug.LogError(pde.name + " - " + e.Message);
                }
            }

            foreach (GameObject group in groups)
            {
                group.SetActive(false);
            }
        }
    }
}
