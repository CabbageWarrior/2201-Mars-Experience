// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;
using System.Collections;

public class PlayerTracker : MonoBehaviour {

	public	bool		m_FollowPosition	= true;
	public	bool		m_FollowRotation	= false;
	public	bool		m_OnlyYAxis			= false;
	public	bool		m_SetFloating		= false;
	public	bool		m_Smoothed			= false;

	public	Vector3		m_Offset			= Vector3.zero;


	private Transform	m_ObjectToFollow	= null;
	[SerializeField]
	private	float		m_FloatingSpeed		= 1f;
	[SerializeField]
	private	float		m_FloatingAmplitude	= 1f;
	private	float		m_Theta				= 0f;



	private IEnumerator Start()
	{
		while(  m_ObjectToFollow == null )
		{
			if ( UnityEngine.VR.VRSettings.isDeviceActive )
			{
				m_ObjectToFollow = VRTK_DeviceFinder.HeadsetTransform();
			}
			else
			if ( Camera.main != null )
			{
				m_ObjectToFollow = Camera.main.transform; // Simulator
			}
			yield return null;
		}
	}


	//////////////////////////////////////////////////////////////////////////
	// OnEnable
	private void OnEnable()
	{
		if ( m_ObjectToFollow == null )
			return;

		Vector3 finalPosition = m_ObjectToFollow.position + transform.TransformDirection( m_Offset );
		transform.position = finalPosition;
	}


	//////////////////////////////////////////////////////////////////////////
	// Update
	private void Update ()
	{
		if ( m_ObjectToFollow == null )
			return;

		// Follow the position
		if ( m_FollowPosition == true )
		{
			Vector3 finalPosition = m_ObjectToFollow.position + transform.TransformDirection( m_Offset );
			
			if ( m_SetFloating == true )
			{
				m_Theta += Time.deltaTime * m_FloatingSpeed;
				finalPosition += ( Vector3.up * Mathf.Sin( m_Theta ) * m_FloatingAmplitude );
			}

			// set position on target with offset
			transform.position = Vector3.Lerp( transform.position, finalPosition, m_Smoothed ? Time.deltaTime * 8f : 1f );

		}

		// Foloow the rotation
		if ( m_FollowRotation == true )
		{
			Quaternion finalRotation = Quaternion.identity;
			if ( m_OnlyYAxis )
			{
				Vector3 e = m_ObjectToFollow.rotation.eulerAngles;
				finalRotation = Quaternion.Euler( 0.0f, e.y, 0.0f );
			}
			else
			{
				finalRotation = m_ObjectToFollow.rotation;
			}

			transform.rotation = Quaternion.Lerp( transform.rotation, finalRotation, m_Smoothed ? Time.deltaTime * 8f : 1f );
		}
	}


}
