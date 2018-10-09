using UnityEngine;
using VRTK.Controllables.PhysicsBased;

public class ButtonEvents : MonoBehaviour
{
	[SerializeField]
	private GameEvent m_OnPressed	= null;
	[SerializeField]
	private GameEvent m_OnReleased	= null;

	[SerializeField]
	private bool m_IsEnabled		= true;
	public bool IsEnabled
	{
		get
		{
			return m_IsEnabled;
		}
		set
		{
			m_IsEnabled = value;
		}
	}

	private bool m_IsPressed = false;
	public bool IsPressed
	{
		get
		{
			return m_IsPressed;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnPressed
	public void OnPressed()
	{
		if (!m_IsEnabled) return;

		m_OnPressed.Invoke();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnReleased
	public void OnReleased()
	{
		if (!m_IsEnabled) return;

		m_OnReleased.Invoke();
	}


	//////////////////////////////////////////////////////////////////////////
	// OnSwitch
	public	void OnSwitch()
	{
		if (!m_IsEnabled) return;

		if ( m_IsPressed )
		{
			m_OnReleased.Invoke();
		}
		else
		{
			m_OnPressed.Invoke();
		}
	}

}