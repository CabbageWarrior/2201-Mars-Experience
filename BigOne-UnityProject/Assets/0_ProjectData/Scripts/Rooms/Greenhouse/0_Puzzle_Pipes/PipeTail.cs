// Scripted by Roberto Leogrande

using UnityEngine;

public class PipeTail : MonoBehaviour {

	private bool		m_Correctlink				= false;
	public	bool		CorrectLink
	{
		get { return m_Correctlink; }
	}

	private bool		colorSet					= false;
	private	GameObject	m_PipeThisTailValidConnector = null;


	//////////////////////////////////////////////////////////////////////////
	// Setup
	public	PipeTail	Setup( GameObject Connector )
	{
		m_PipeThisTailValidConnector = Connector;
//		Connector.GetComponent<Renderer>().enabled = false;
	//	myRenderer.enabled = false;
//		myRenderer.material.color = Color.clear;
		return this;
	}


	//////////////////////////////////////////////////////////////////////////
	// UNITY
	private void OnTriggerStay( Collider other )
	{
		if ( other.gameObject != m_PipeThisTailValidConnector || colorSet )
			return;

		other.GetComponent<Renderer>().enabled = true;
		m_Correctlink = true;
		colorSet = true;
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.gameObject != m_PipeThisTailValidConnector )
			return;

		colorSet = false;
		m_Correctlink = false;
		other.GetComponent<Renderer>().enabled = false;
	}
	
}
