// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;

public class VRTK_PointerDirectionIndicator_Extension : VRTK_PointerDirectionIndicator {

	[Space()]
	[Header("Extension properties")]

	[HideInInspector]
	// offset that headset has relative to position zero
	public float headsetOffset;

	public bool positionLocked = false;

	[SerializeField]
	// is considered for updating local rotation
	private bool m_LockedRotation = false;
	public bool LockedRotation
	{
		set { m_LockedRotation = value; }
	}

	//////////////////////////////////////////////////////////////////////////
	// GetRotation ( Overrider )
	public override Quaternion GetRotation()
	{
		float offset = (includeHeadsetOffset ? playArea.eulerAngles.y - headsetOffset : 0f);
		return Quaternion.Euler(0f, transform.localEulerAngles.y + offset, 0f);
	}


	//////////////////////////////////////////////////////////////////////////
	// SetPosition ( Overrider )
	public override void SetPosition( bool active, Vector3 position )
	{
		if ( positionLocked == true )
			return;

        // ToDo: Sistemare la Bezier sullo snap della freccetta
		base.SetPosition( active, position );
	}


	//////////////////////////////////////////////////////////////////////////
	// UNITY
	protected override void Update()
	{
		if ( controllerEvents != null && controllerEvents.touchpadTouched && !InsideDeadzone( controllerEvents.GetTouchpadAxis() ) && !m_LockedRotation )
		{
			float touchpadAngle = controllerEvents.GetTouchpadAxisAngle();
			float angle = touchpadAngle + headsetOffset;
			transform.localEulerAngles = new Vector3( 0f, angle, 0f );
		}
	}

}
