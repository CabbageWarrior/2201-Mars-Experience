// Scripted by Roberto Leogrande

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IVR_UI_Interactable_Toggle
{
	object	Value { set; }
}

[RequireComponent( typeof( BoxCollider ) )]
public class VR_UI_Interactable_Toggle : MonoBehaviour, IVR_UI_Interactable_Toggle {

	[SerializeField]
	private	GameEvent			m_OnValueOn				= null;

	[SerializeField]
	private	GameEvent			m_OnValueOff			= null;

	// COLORS
	[SerializeField]
	private Color               m_NomalColor			= Color.white;

	[SerializeField]
	private Color               m_HighlightedColor		= Color.white;

	[SerializeField]
	private Color               m_CheckedColor			= Color.white;

	[SerializeField]
	private Color               m_DisabledColor			= Color.white;

	// ACTIVATION TIME
	[SerializeField]
	[Range(0.1f, 2f)]
	private float               m_ActivationTime        = 2f;

	[SerializeField]
	private	bool				m_IsOn					= false;
	public	bool				IsOn
	{
		get { return m_IsOn; }
		set { m_IsOn = value; OnValueSet(); }
	}

	private bool                m_IsEnabled             = true;
	public bool IsEnabled
	{
		get {
			return m_IsEnabled;
		}
		set {
			m_IsEnabled = value;
			SetEnabled( m_IsEnabled );
		}
	}

	private	object				m_Value					= null;
	public	object				Value
	{
		get { return m_Value; }
	}
	// hidden setter
	object IVR_UI_Interactable_Toggle.Value
	{
		set { m_Value = value; }
	}


	private	Image				m_Background			= null;
	private	Image				m_CheckMark				= null;
	private	Image				m_Label					= null;


	//////////////////////////////////////////////////////////////////////////
	// START
	private void Start()
	{
		m_Background = transform.GetChild( 0 ).GetComponent<Image>();
		m_CheckMark = m_Background.transform.GetChild( 0 ).GetComponent<Image>();
		m_Label = transform.GetChild( 1 ).GetComponent<Image>();

		m_CheckMark.enabled = false;
	}


	//////////////////////////////////////////////////////////////////////////
	// OnEnable
	private void OnEnable()
	{
		this.Start();

		m_Background.color = ( m_IsEnabled == true ) ? m_NomalColor : m_DisabledColor;
		m_CheckMark.enabled = m_IsOn;
	}


	//////////////////////////////////////////////////////////////////////////
	// SetValue
	private void OnValueSet()
	{
		m_CheckMark.enabled = m_IsOn;
		m_Background.color = ( m_IsOn == true ) ? m_CheckedColor : m_NomalColor;
		if ( m_IsOn == true )
		{
			if ( m_OnValueOn != null )
				m_OnValueOn.Invoke();
		}
		else
		{
			if ( m_OnValueOff != null )
				m_OnValueOff.Invoke();
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// SetEnabled
	public void SetEnabled( bool state )
	{
		m_IsEnabled = state;
		m_Background.color = ( m_IsEnabled == true ) ? m_NomalColor : m_DisabledColor;
	}

	
	//////////////////////////////////////////////////////////////////////////
	// MakeTransition
	private	IEnumerator MakeTransition()
	{
		Color startColor = m_Background.color;
		float interpolant = 0f;
		float currentTime = 0f;

		while ( interpolant < 1.0f )
		{
			currentTime += Time.deltaTime;
			interpolant = currentTime / m_ActivationTime;
			m_Background.color = Color.Lerp( startColor, m_HighlightedColor, interpolant );
			yield return null;
		}

		m_IsOn = !m_IsOn;
		this.OnValueSet();
	}


	//////////////////////////////////////////////////////////////////////////
	// UNITY
	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" && m_IsEnabled )
		{
			StopAllCoroutines();
			StartCoroutine( MakeTransition() );
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.tag == "Player" && m_IsEnabled )
		{
			StopAllCoroutines();
			m_Background.color = m_NomalColor;
		}
	}

}
