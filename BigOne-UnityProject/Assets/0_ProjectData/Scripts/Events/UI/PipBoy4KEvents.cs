// Scripted by Roberto Leogrande

using UnityEngine;


public class PipBoy4KEvents : MonoBehaviour {
	
	[SerializeField]
	private	VR_UI_Interactable_Toggle[]	m_SavesToogles		= null;


	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start()
	{
		if ( m_SavesToogles == null || m_SavesToogles.Length == 0 )
		{
			Debug.Log( "WARNING: PipBoy4KEvents: Array of toogles in save panel is empty" );
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnToogleSet
	public	void	OnToogleSet( VR_UI_Interactable_Toggle thisToggle )
	{
		foreach( var t in m_SavesToogles )
		{
			if ( t == thisToggle )
				continue;

			t.IsOn = false;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnLoad
	public	void	OnLoad()
	{
		VR_UI_Interactable_Toggle toggle = System.Array.Find( m_SavesToogles, t => t.IsOn == true );
		if ( toggle != null )
		{
			GameManager.SaveManagemnt.Load( ( string ) toggle.Value );
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnSave
	public	void	OnSave()
	{
		VR_UI_Interactable_Toggle toggle = System.Array.Find( m_SavesToogles, t => t.IsOn == true );
		if ( toggle != null )
		{

		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnDelete
	public	void	OnDelete()
	{

	}


	//////////////////////////////////////////////////////////////////////////
	// OnQuit
	public	void	OnQuit()
	{

	}

}
