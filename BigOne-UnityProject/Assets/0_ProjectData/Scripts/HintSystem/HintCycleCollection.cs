// Scripted by Roberto Leogrande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HintSystem {

	public class HintCycleCollection : ScriptableObject, IEnumerable< HintCycle > {

		[SerializeField]
		private List<HintCycle> m_HintCycles;

		public int	Count
		{
			get {
				if ( m_HintCycles == null ) return 0;

				return m_HintCycles.Count;
			}
		}

		// INDEXER
		public HintCycle this[int i]
		{
			get { 
				if ( m_HintCycles == null || i < 0 || i > m_HintCycles.Count )
				{
					return null;
				}
				return m_HintCycles[i];
			}
			set {
				if ( m_HintCycles == null || i < 0 || i > m_HintCycles.Count )
				{
					return;
				}
				m_HintCycles[i] = value;
			}
		}


		// For IEnumerable<Car>
		public IEnumerator<HintCycle> GetEnumerator()
		{
			return m_HintCycles.GetEnumerator();
		}

		// For IEnumerable
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}


		//////////////////////////////////////////////////////////////////////////
		// Create
		public	void	Create()
		{
			if ( m_HintCycles == null )
			{
				m_HintCycles = new List<HintCycle>();
			}
		}


		//////////////////////////////////////////////////////////////////////////
		// Add
		public void	Add( HintCycle cycle )
		{
			if ( m_HintCycles == null )
			{
				this.Create();
			}
			m_HintCycles.Add( cycle );
		}

		
		//////////////////////////////////////////////////////////////////////////
		// Remove
		public	void	RemoveAt( int idx )
		{
			if ( m_HintCycles == null || idx < 0 || idx > m_HintCycles.Count - 1 )
			{
				return;
			}
			m_HintCycles.RemoveAt( idx );
		}

	}

}