// Scripted by Roberto Leogrande

using UnityEngine;
using System.Collections.Generic;

namespace HintSystem {

	public interface IHintCheck {

		bool IsCompleted { get; }

	}

	[System.Serializable]
	public class HintCommon : ScriptableObject {

		[SerializeField]
		public	HintCommon			Father	= null;


		[SerializeField][HideInInspector]
		public	string				Path		= "";

		[SerializeField]
		public	string				Name		= null;
		[SerializeField]
		private	bool				Sent		= false;
		public	bool				IsSent
		{
			get { return Sent; }
		}

		public	bool				IsCompleted
		{
			get
			{
				return ( Childs.Find( c => c.IsSent == false ) == null );
			}
		}

		[SerializeField]
		public	List<HintCommon>	Childs	= null;



		//////////////////////////////////////////////////////////////////////////
		// SetAsSent
		public	void				SetAsSent()
		{
			{
				Sent = true;
				if ( Father != null )
				{
					if ( Father.IsCompleted )
					{
						Debug.Log( "Father " + Father.Name + " completed" );
						Father.SetAsSent();
					}
				}
			}
		}


		//////////////////////////////////////////////////////////////////////////
		// Reset
		public void Reset()
		{
			Sent = false;

			foreach( var child in Childs )
			{
				child.Reset();
			}
		}

	}

}