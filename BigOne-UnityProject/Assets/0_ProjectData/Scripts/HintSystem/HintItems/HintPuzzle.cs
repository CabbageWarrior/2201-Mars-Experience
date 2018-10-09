// Scripted by Roberto Leogrande

using UnityEngine;
using System.Linq;

namespace HintSystem {

	using System.Collections.Generic;

	public class HintPuzzle : HintCommon {

		//////////////////////////////////////////////////////////////////////////
		// EQUALS ( Overrider )
		public override bool Equals( object other )
		{
			HintCommon puzzle = other as HintCommon;
			if ( puzzle == null ) return false;

			return puzzle.Path == this.Path;

		}

		//////////////////////////////////////////////////////////////////////////
		// GETHASHCODE ( Overrider )
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}


		//////////////////////////////////////////////////////////////////////////
		// Set
		public	void	Set( HintPuzzle loadedPuzzle )
		{
			foreach( HintGroup onlineGroup in loadedPuzzle.Childs )
			{
				HintGroup loadedGroup = loadedPuzzle.Childs.Find( g => g.Path == this.Path ) as HintGroup;
				if ( loadedGroup == null )
				{
					Debug.Log( "WARNING: Hint Group " + name + " - Set: Cannot retrieve hint group data with path " + this.Path );
					continue;
				}
				onlineGroup.Set( loadedGroup );
			}

			if ( loadedPuzzle.IsSent )
				this.SetAsSent();
		}

	}


}