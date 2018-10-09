// Scripted by Roberto Leogrande

using UnityEngine;
using System.Linq;

namespace HintSystem {

	using System.Collections.Generic;

	public class HintGroup : HintCommon {
		

		//////////////////////////////////////////////////////////////////////////
		// Set
		public	void	Set( HintGroup loadedGroup )
		{
			foreach( Hint onlineHint in loadedGroup.Childs )
			{
				Hint loadedHint = loadedGroup.Childs.Find( h => h.Path == this.Path ) as Hint;
				if ( loadedGroup == null )
				{
					Debug.Log( "WARNING: Hint " + name + " - Set: Cannot retrieve hint data with path " + this.Path );
					continue;
				}
				if ( loadedHint.IsSent )
					onlineHint.SetAsSent();
			}

			if ( loadedGroup.IsSent )
				this.SetAsSent();
		}

	}


}