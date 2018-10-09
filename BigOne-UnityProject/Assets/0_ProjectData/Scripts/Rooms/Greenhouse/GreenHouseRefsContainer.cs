using UnityEngine;

public class GreenHouseRefsContainer : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_Buttons = null;

    [SerializeField]
    private Transform[] m_Pipes = null;

    [SerializeField]
    private Transform[] m_PlantsUp = null;

    [SerializeField]
    private Transform[] m_PlantsDown = null;
    
    private void Start()
    {
        DisableButtons();
    }
    
    public void DisableButtons()
    {
        foreach (Transform t in m_Buttons)
        {
            t.GetComponentInChildren<VR_UI_Interactable_Button>().IsEnabled = false;
        }
    }

    public void EnableButtons()
    {
        foreach (Transform t in m_Buttons)
        {
            t.GetComponentInChildren<VR_UI_Interactable_Button>().IsEnabled = true;
        }
    }

    public void EnablePipes()
    {
        foreach (Transform t in m_Pipes)
        {
            t.gameObject.SetActive(true);
        }
    }

    public void DisablePipes()
    {
        foreach (Transform t in m_Pipes)
        {
            t.gameObject.SetActive(false);
        }
    }

    public void AnimatePlants()
    {
        foreach (Transform t in m_PlantsUp)
        {
            t.gameObject.SetActive(true);
        }

        foreach (Transform t in m_PlantsDown)
        {
            t.gameObject.SetActive(false);
        }
    }
}