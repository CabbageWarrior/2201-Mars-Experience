using UnityEngine;
using System.Collections;
using VRTK;
using VRTK.Highlighters;


// unique implementation for M.A.R.I.A.

public class VRTK_OutlineObjectCopyHighlighter_Extension : VRTK_OutlineObjectCopyHighlighter {


	public	Transform	newFather		= null;



	protected override Renderer CreateHighlightModel( GameObject givenOutlineModel, string givenOutlineModelPath )
	{
		Renderer retVal = base.CreateHighlightModel( givenOutlineModel, givenOutlineModelPath );

		if ( newFather == null )
			return retVal;

		retVal.transform.SetParent( newFather );
		return retVal;
	}

}
