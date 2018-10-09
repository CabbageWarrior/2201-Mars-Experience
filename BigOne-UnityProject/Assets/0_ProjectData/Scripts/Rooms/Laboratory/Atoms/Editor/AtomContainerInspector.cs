using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AtomContainer))]
public class SequenceFrameCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AtomContainer atomContainer = (AtomContainer)target;

        if (GUILayout.Button("Generate"))
        {
            atomContainer.GenerateAtoms(atomContainer.elementName, atomContainer.nAtoms);
        }

        if (GUILayout.Button("Discard"))
        {
            atomContainer.DiscardAtoms();
        }
    }

}