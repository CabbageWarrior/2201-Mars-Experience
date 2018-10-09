// Scripted by Roberto Leogrande

using UnityEngine;

namespace SaveLoadUtilities {

	using System;
	using System.Collections.Generic;
	using HintSystem;
	using Cutscene;

	[Serializable]
	public	class PuzzleInfo
	{

		[SerializeField]
		private string	m_Name;
		public	string Name
		{
			get { return m_Name; }
		}

		[SerializeField]
		private	bool	m_Completed;
		public	bool Completed
		{
			get { return m_Completed; }
		}


		public PuzzleInfo( string name, bool completed )
		{
			this.m_Name			= name;
			this.m_Completed	= completed;
		}

	}


	[Serializable]
	public class PermanentGameData
	{

		[SerializeField]
		private	List<PuzzleInfo>		m_PuzzleInfos		= null;

		[SerializeField]
		private	List<HintCycle>			m_HintCycles		= null;

		[SerializeField]
		private	AudioLogOnline[]		m_AudioLogs			= null;

		//////////////////////////////////////////////////////////////////////////
		// COSNTRUCTOR
		public PermanentGameData()
		{
			m_PuzzleInfos	= new List<PuzzleInfo>();
			m_HintCycles	= new List<HintCycle>();
		}

		//////////////////////////////////////////////////////////////////////////
		// SaveAudioLogs
		public	void	SaveAudioLogs( AudioLogOnline[] audioLogs )
		{
			m_AudioLogs = audioLogs;
		}

		//////////////////////////////////////////////////////////////////////////
		// GetAudioLogs
		public AudioLogOnline[]	GetAudioLogs()
		{
			return m_AudioLogs;
		}

		//////////////////////////////////////////////////////////////////////////
		// FindAudioLog
		public	AudioLogOnline	FindAudioLog( AudioLogOnline scriptable )
		{
			AudioLogOnline audioLog = Array.Find( m_AudioLogs, a => a.LogSequence == scriptable );
			return audioLog;
		}

		//////////////////////////////////////////////////////////////////////////
		// SavePuzzleInfo
		public	void	SavePuzzleInfo( PuzzleInfo info )
		{
			m_PuzzleInfos.Add( info );
		}


		//////////////////////////////////////////////////////////////////////////
		// GetPuzzleInfo
		public	PuzzleInfo GetPuzzleInfo( string puzzleName )
		{
			return m_PuzzleInfos.Find( p => p.Name == puzzleName );
		}


		//////////////////////////////////////////////////////////////////////////
		// SaveRoomSection
		public	void SaveRoomSection( HintCycle[] hintCycles )
		{
			// this is to avoid multiple instance of the same cycle
			foreach( HintCycle cycle in hintCycles )
			{
				if ( m_HintCycles.Find( c => c.Path == cycle.Path ) )
					continue;

				m_HintCycles.Add( cycle );
			}
			
		}


		//////////////////////////////////////////////////////////////////////////
		// GetHintCycle
		public	HintCycle GetHintCycle( string path )
		{
			return m_HintCycles.Find( c => c.Path == path );
		}

	}


}

