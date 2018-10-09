// Scripted by Roberto Leogrande

using System.Collections;
using UnityEngine;
using VRTK;

public class PipBoy4K : MonoBehaviour {

	[Range( 0f, 180f )]
	public		float		m_AngleToShow				= 30.0f;
	[Range( 0f, 3f )]
	public		float		m_TimeToWaitBeforeShow		= 2.0f;
	[Range( 0f, 3f )]
	public		float		m_ShowTime					= 0.5f;


	private		GameObject	m_CanvasToShow				= null;
	private		Transform	m_HeadsetTransform			= null;
	private		Transform	m_ElectronicDevice			= null;

	private		float		m_CurrentTime				= 0f;
	private		float		m_WaiterTime				= 0f;
	private		IEnumerator	m_WaitCO					= null;
	private		IEnumerator	m_ScaleCO					= null;
	private		bool		m_Showing					= false;

	private		bool		m_IsOK						= false;



	//////////////////////////////////////////////////////////////////////////
	// START
	private IEnumerator Start ()
	{
		Transform canvasObject = transform.GetChild( 0 );
		if ( canvasObject == null )
		{
			print( "PipBoy4K::Start::You need to assign canvas object as child of bracialet !!" );
			yield break;
		}
		m_CanvasToShow = canvasObject.gameObject;

		// Ensure that canvas is disabled at start
		m_CanvasToShow.SetActive( false );

		//	HEADSET
		while ( m_HeadsetTransform == null )
		{
			m_HeadsetTransform = VRTK_DeviceFinder.HeadsetTransform();
			yield return null;
		}

		//	PIP BOY
		GameObject leftController = null;
		while ( leftController == null )
		{
			leftController = PlayerCommon.Instance.LeftController;
			yield return null;
		}
		m_ElectronicDevice = leftController.transform.GetChild( 3 );

		m_CanvasToShow.transform.localScale = Vector3.zero;

		//	"Enable script"
		m_IsOK = true;
	}


	//////////////////////////////////////////////////////////////////////////
	// WaitToShowCO ( Coroutine )
	private	IEnumerator	WaitToShowCO( System.Action action )
	{
		yield return new WaitForSecondsRealtime( m_TimeToWaitBeforeShow );
		
		m_WaitCO = null;

		action();
	}


	//////////////////////////////////////////////////////////////////////////
	// ShowCO ( Coroutine )
	private	IEnumerator	ShowCO()
	{
		float interpolant = m_CurrentTime = 0f;

		m_CanvasToShow.SetActive( true );

		while( interpolant < 1.0f )
		{
			m_CurrentTime += Time.unscaledDeltaTime;
			interpolant = m_CurrentTime / m_ShowTime;
			m_CanvasToShow.transform.localScale = Vector3.Lerp( m_CanvasToShow.transform.localScale, Vector3.one, interpolant );
			yield return null;
		}

		m_CurrentTime = 0f;
		m_CanvasToShow.transform.localScale = Vector3.one;
	}


	//////////////////////////////////////////////////////////////////////////
	// HideCO ( Coroutine )
	private	IEnumerator	HideCO()
	{
		float interpolant = m_CurrentTime = 0f;

		while( interpolant < 1.0f )
		{
			m_CurrentTime += Time.unscaledDeltaTime;
			interpolant = m_CurrentTime / m_ShowTime;
			m_CanvasToShow.transform.localScale = Vector3.Lerp( m_CanvasToShow.transform.localScale, Vector3.zero, interpolant );
			yield return null;
		}

		m_CurrentTime = 0f;
		m_CanvasToShow.transform.localScale = Vector3.zero;
		m_CanvasToShow.SetActive( false );
	}
	


	//////////////////////////////////////////////////////////////////////////
	// UNITY
	private void Update ()
	{
		if ( m_IsOK == false )
			return;

		if ( Mathf.Abs( Vector3.Angle( m_ElectronicDevice.right, m_HeadsetTransform.forward ) ) < m_AngleToShow )
		{
			m_WaiterTime += Time.unscaledDeltaTime;
			if ( m_WaiterTime > m_TimeToWaitBeforeShow && m_ScaleCO == null && m_Showing == false )
			{
				foreach( Transform t in transform.GetChild(0) )
					t.gameObject.SetActive( false );

				transform.GetChild(0).GetChild(0).gameObject.SetActive( true );
				m_Showing = true;
				StartCoroutine( m_ScaleCO = ShowCO() );
			}
		}
		else
		{
			m_WaiterTime = 0f;
			if ( m_Showing == true )
			{
				m_ScaleCO = null;
				m_Showing = false;
				StopAllCoroutines();
				StartCoroutine( HideCO() );
//				m_CanvasToShow.SetActive( false );
			}
		}

	}

}
