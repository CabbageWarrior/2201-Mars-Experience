// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lab_Minigame {


	public class MinigameLevelScriptable : ScriptableObject {

		[SerializeField]
		private	Vector2							m_GridSize			= Vector2.zero;
		public	Vector2							GridSize {
			get { return m_GridSize; }
			set { m_GridSize = value; }
		}


		[SerializeField]
		private	List<MinigameLevelData>			m_Levels			= null;
		public	List<MinigameLevelData>			Levels {
			get { return m_Levels; }
			set { m_Levels = value; }
		}

	}

	 
}