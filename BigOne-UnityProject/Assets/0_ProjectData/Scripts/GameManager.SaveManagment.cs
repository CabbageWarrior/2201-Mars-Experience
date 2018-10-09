// Scripted by Roberto Leogrande

using UnityEngine;
using System.Linq;
using System.Collections.Generic;

//	DELEGATES FOR EVENTS
public delegate void StreamingEvent( SaveLoadUtilities.PermanentGameData stream );


public class SaveFileinfo {

	public	string	fileName	= "";
	public	string	filePath	= "";
	public	string	saveTime	= "";
	public	bool	isAutomatic = false;

}


// INTERFACE
/// <summary> Clean interface of only GameManager class </summary>
public interface IGameManager_SaveManagement {

	// PROPERTIES

	/// <summary> Events called when game is saving </summary>
	event StreamingEvent		OnSave;

	/// <summary> Events called when game is loading </summary>
	event StreamingEvent		OnLoad;

	/// <summary> Save current play </summary>
	void Save( string fileName, bool isAutomaic = false );

	/// <summary> Load a file </summary>
	void Load( string fileName );
}


// CLASS
public partial class GameManager : IGameManager_SaveManagement {
		
//	public static bool IsLoading	= false;

	const	string		SavePath		= "Saves";
	const	int			SavesTotNumber	= 10;

	/// <summary> Events called when game is saving </summary>
	public event StreamingEvent			OnSave						= null;

	/// <summary> Events called when game is loading </summary>
	public event StreamingEvent			OnLoad						= null;

	private	 List<SaveFileinfo>			m_SaveFiles					= null;
	public	 List<SaveFileinfo>			SaveFiles
	{
		get { return m_SaveFiles; }
	}


	private	void	CheckForSaves()
	{
		// Skip process if save path does not exists
		if ( System.IO.Directory.Exists( System.IO.Path.Combine( Application.dataPath, SavePath ) ) == false )
			return;

		// No file found
		string[] files = System.IO.Directory.GetFiles( System.IO.Path.Combine( Application.dataPath, SavePath ) );
		if ( files.Length == 0 )
			return;

		// sort files info for last modify
		System.IO.FileInfo[] sortedFiles = new System.IO.DirectoryInfo( System.IO.Path.Combine( Application.dataPath, SavePath ) )
			.GetFiles()
			.OrderBy( f => f.LastWriteTime )
			.ToArray();

		// Fill list of structure
		m_SaveFiles = new List<SaveFileinfo>(10);
		for( int i = 0; i < SavesTotNumber + 1; i++ )
		{
			SaveFileinfo info	= m_SaveFiles[ i ];
			info.fileName		= sortedFiles[ i ].Name;
			info.filePath		= sortedFiles[ i ].FullName;
			info.isAutomatic	= info.fileName.Contains( "autosave" );
			info.saveTime		= sortedFiles[ i ].LastWriteTime.ToString( "dd-MM-yy" );
		}

		// TODO: USE THIS TO FILL A SAVE LIST FOR PAUSE MENU
	}


	//////////////////////////////////////////////////////////////////////////
	// Save
	/// <summary> Used to save data of puzzles </summary>
	public void Save( string fileName, bool isAutomaic = false )
	{
		// TODO: CHECK FOR AUTOMAIC SAVE

		SaveLoadUtilities.PermanentGameData saveData = new SaveLoadUtilities.PermanentGameData();

		// call all puzzles save callbacks
		OnSave( saveData );

		// write data on disk
		string toSave = JsonUtility.ToJson( saveData );
		System.IO.File.WriteAllText( "SaveFile.txt", toSave );
	}


	//////////////////////////////////////////////////////////////////////////
	// Load
	/// <summary> Used to send signal of load to all registered callbacks </summary>
	public void Load( string fileName = "" )
	{
		if ( fileName == null || fileName.Length == 0 )
			return;

		// load data from disk
		string toLoad = System.IO.File.ReadAllText( fileName );
		SaveLoadUtilities.PermanentGameData stream = JsonUtility.FromJson<SaveLoadUtilities.PermanentGameData>( toLoad );

		if ( stream == null )
		{
			Debug.LogError( "GameManager::Load:: Save \"" + fileName +"\" cannot be loaded" );
			return;
		}

		// call all puzzles load callbacks
		OnLoad( stream );
	}

}

