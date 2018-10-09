using UnityEngine;
using VRTK;

public class Chips : MonoBehaviour
{

    private void OnTriggerStay( Collider col )
    {
		VRTK_SnapDropZone dropZone = col.GetComponent<VRTK_SnapDropZone>();
        Slots slots = col.GetComponent<Slots>();


        if ( dropZone == null || slots == null) return;
		
        if ( dropZone.ValidSnappableObjectIsHovering() )
        {
            if (slots.chipSnapped == false)
            {
                col.transform.localRotation = Quaternion.Euler(0, 0, AngleToNinety(transform.rotation.eulerAngles.z));
            }
        }
    }

    float AngleToNinety( float angle )
    {
        return Mathf.Round(angle / 90) * 90;
    }
	
}