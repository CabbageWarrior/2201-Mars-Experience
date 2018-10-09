using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_ControllerEvents))]
public class CustomControllerEvents : MonoBehaviour
{
    [SerializeField]
    private GameEventArg1 m_OnButton1Click;
    [SerializeField]
    private GameEventArg1 m_OnButton2Click;

    private VRTK_ControllerEvents controllerEvents;

    private bool isButtonOneAlreadyPressed = false;
    private bool isButtonTwoAlreadyPressed = false;

    private void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        controllerEvents.ButtonOnePressed += new ControllerInteractionEventHandler(buttonOnePressed);
        controllerEvents.ButtonOneReleased += new ControllerInteractionEventHandler(buttonOneReleased);

        controllerEvents.ButtonTwoPressed += new ControllerInteractionEventHandler(buttonTwoPressed);
        controllerEvents.ButtonTwoReleased += new ControllerInteractionEventHandler(buttonTwoReleased);
    }

    #region Button One
    private void buttonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!isButtonOneAlreadyPressed && m_OnButton1Click != null)
        {
            isButtonOneAlreadyPressed = true;
            m_OnButton1Click.Invoke(gameObject);
        }
    }
    private void buttonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
        isButtonOneAlreadyPressed = false;
    }
    #endregion

    #region Button Two
    private void buttonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!isButtonTwoAlreadyPressed && m_OnButton2Click != null)
        {
            isButtonTwoAlreadyPressed = true;
            m_OnButton2Click.Invoke(gameObject);
        }
    }
    private void buttonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        isButtonTwoAlreadyPressed = false;
    }
    #endregion
}