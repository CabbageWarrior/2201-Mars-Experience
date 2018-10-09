// Scripted by Roberto Leogrande

using UnityEngine;

namespace HintSystem {

	public class Hint : HintCommon {

		// INTERNAL DATA
		[SerializeField]
		public	AudioClip			ClipSound	= null;

		[SerializeField, FMODUnity.EventRef]
		public	string				EventRef	= string.Empty;

		[SerializeField]
		public	string				Text		= string.Empty;

	}


}