// Scripted by Roberto Leogrande

using UnityEngine;

namespace Cutscene {

	[ CreateAssetMenu( menuName = "Cutscenes/Create Item", order = 1 ) ]
	public class SequenceFrame : ScriptableObject {

		public	string		SpeakerName		= "";

		[TextArea()]
		public	string		Text			= null;

		[FMODUnity.EventRef]
		public	string		AudioRef		= "";

		public	float		Duration		= 0.0f;

		[HideInInspector]
		public	bool		IsExecuted		= false;

	}

	[System.Serializable]
	public class SequencePair {

		public	SequenceFrame	SequenceFrame	= null;

		public	GameEventArg1	OnFrameEnd		= null;

	}

}