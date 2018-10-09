using UnityEngine;
using VRTK;

public class TorchManager : MonoBehaviour
{
    public GameObject blackLight;

	[Space]
	public  GameEvent m_OnLightSwitch = null;

    enum LightMode
    {
        Blacklight,
        Off
    };
    LightMode SwitchLight;

    private VRTK_InteractableObject m_VRTK_InteractableObject = null;

    void Start()
    {
        m_VRTK_InteractableObject = GetComponent<VRTK_InteractableObject>();

        BlackLight();
    }

    public void Switch(GameObject grabbingController)
    {
        if (m_VRTK_InteractableObject.GetGrabbingObject() == grabbingController && m_VRTK_InteractableObject.IsGrabbed())
        {
			if ( m_OnLightSwitch != null )
			{
				m_OnLightSwitch.Invoke();
			}

            switch (SwitchLight)
            {
                case LightMode.Blacklight:
                    LightOff();
                    break;
                case LightMode.Off:
                    BlackLight();
                    break;
            }
        }
    }


    public void BlackLight()
    {
        SwitchLight = LightMode.Blacklight;
        blackLight.gameObject.SetActive(true);
    }

    public void LightOff()
    {
        SwitchLight = LightMode.Off;
        blackLight.gameObject.SetActive(false);
    }
}