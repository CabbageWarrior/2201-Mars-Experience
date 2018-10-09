using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cutscene {

	[CustomEditor( typeof( SequenceFrame ) )]
	public class SequenceFrameCustomInspector : Editor {

		
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
        
			SequenceFrame sequenceFrame = (SequenceFrame)target;
			if( GUILayout.Button( "Edit Frame" ) )
			{
				SequenceFrameEditor.Init( sequenceFrame );
			}
		}

	}

}
