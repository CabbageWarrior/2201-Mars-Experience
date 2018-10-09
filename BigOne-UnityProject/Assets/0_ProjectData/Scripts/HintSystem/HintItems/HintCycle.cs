// Scripted by Roberto Leogrande

using UnityEngine;
using System.Linq;

namespace HintSystem {

	using System.Collections.Generic;
	
	public class HintCycle : HintCommon {

		// INTERNAL DATA		

		[SerializeField]
		public	bool				IsTransition	= false;


		//////////////////////////////////////////////////////////////////////////
		// Set
		public void	Set( HintCycle loadedCycle )
		{
			foreach( HintPuzzle onlinePuzzle in Childs )
			{
				HintPuzzle loadedPuzzle = loadedCycle.Childs.Find( p => p.Path == this.Path ) as HintPuzzle;
				if ( loadedCycle == null )
				{
					Debug.Log( "WARNING: Hint Puzzle " + name + " - Set: Cannot retrieve hint puzzle data with path " + this.Path );
					continue;
				}
				onlinePuzzle.Set( loadedPuzzle );
			}

			if ( loadedCycle.IsSent )
				this.SetAsSent();
		}

	}


}