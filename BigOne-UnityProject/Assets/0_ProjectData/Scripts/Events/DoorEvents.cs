// Scripted by Roberto Leogrande

using UnityEngine;



public class DoorEvents : MonoBehaviour {

	[SerializeField]
	private	GameEvent	m_OnOpenend;
	[SerializeField]
	private	GameEvent	m_OnClosed;



	//////////////////////////////////////////////////////////////////////////
	// OnClosed
	public	void	OnClosed()
	{
		m_OnOpenend.Invoke();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnOpened
	public	void	OnOpened()
	{
		m_OnClosed.Invoke();
	}

	
}
