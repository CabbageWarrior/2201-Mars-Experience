// Scripted by Roberto Leogrande

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// https://stackoverflow.com/questions/28202442/how-to-stop-drag-event-in-onbegindrag-in-unity-4-6

[RequireComponent( typeof( BoxCollider ) )]
public class VR_UI_Interactable_Button : MonoBehaviour {

	// COLORS
	[SerializeField]
	private Color               m_NomalColor            = Color.white;

	[SerializeField]
	private Color               m_HighlightedColor      = Color.white;

	[SerializeField]
	private Color               m_UsedColor             = Color.white;

	[SerializeField]
	private Color               m_DisabledColor         = Color.white;

	// ACTIVATION TIME
	[SerializeField]
	[Range(0.1f, 2f)]
	private float               m_ActivationTime        = 2f;

	// BUTTON EVENT
	[SerializeField]
	private GameEvent           m_OnActivation          = null;

	private Image               m_Image                 = null;
	private bool                m_HasEvents             = true;

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



	//////////////////////////////////////////////////////////////////////////
	// OnEnable
	private void OnEnable()
	{
		if ( m_Image == null )
			m_Image = GetComponent<Image>();

		if ( m_Image == null )
			return;

		m_Image.color = m_NomalColor;

		if ( m_OnActivation.GetPersistentEventCount() == 0 )
		{
			m_HasEvents = false;
			m_Image.color = m_DisabledColor;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// SeEnabled
	public void SetEnabled( bool state )
	{
		m_IsEnabled = state;
		if ( m_Image != null )
		{
			m_Image.color = ( m_IsEnabled == true ) ? m_NomalColor : m_DisabledColor;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// MakeTransition
	private	IEnumerator MakeTransition()
	{
		if ( m_Image == null )
		{
			m_OnActivation.Invoke();
			yield break;
		}

		Color startColor = m_Image.color;
		float interpolant = 0f;
		float currentTime = 0f;

		while ( interpolant < 1.0f )
		{
			currentTime += Time.deltaTime;
			interpolant = currentTime / m_ActivationTime;
			m_Image.color = Color.Lerp( startColor, m_HighlightedColor, interpolant );
			yield return null;
		}

		m_Image.color = m_UsedColor;
		m_OnActivation.Invoke();
	}


	//////////////////////////////////////////////////////////////////////////
	// UNITY
	private void OnTriggerEnter( Collider other )
	{
		if ( m_HasEvents == false )
			return;

		if ( other.tag == "Player" && m_IsEnabled )
		{
			StopAllCoroutines();
			StartCoroutine( MakeTransition() );
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( m_HasEvents == false )
			return;

		if ( m_Image == null )
			return;

		if ( other.tag == "Player" && m_IsEnabled )
		{
			StopAllCoroutines();
			m_Image.color = m_NomalColor;
		}
	}

}
