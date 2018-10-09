namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEventHelper;

    public class KeypadMorseButtons : MonoBehaviour
    {
        public KeypadMorseManager m_KeypadMorseManager;
        public KeypadMorseManager.KeypadOperations operation;
        public KeypadMorseManager.KeypadCodes codes;

        private VRTK_Button_UnityEvents buttonEvents;

        private void Start()
        {
            buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
            if (buttonEvents == null)
            {
                buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
            }
            buttonEvents.OnPushed.AddListener(handlePush);
        }

        private void handlePush(object sender, Control3DEventArgs e)
        {
            VRTK_Logger.Info("Pushed");

            switch (operation)
            {
                case KeypadMorseManager.KeypadOperations.ADD:
                    switch (gameObject.name)
                    {
                        case "ButtonDot":
                            m_KeypadMorseManager.CodeDisplay.text += ".";
                            break;

                        case "ButtonDash":
                            m_KeypadMorseManager.CodeDisplay.text += "-";
                            break;
                    }
                    break;
                case KeypadMorseManager.KeypadOperations.REMOVE:
                    if (m_KeypadMorseManager.CodeDisplay.text.Length > 0)
                    {
                        m_KeypadMorseManager.CodeDisplay.text = m_KeypadMorseManager.CodeDisplay.text.Remove(m_KeypadMorseManager.CodeDisplay.text.Length - 1);
                    }
                    break;
              /*  case KeypadMorseManager.KeypadOperations.CHECK:
                   if (m_KeypadMorseManager.CheckMorse())
                    {
                        StartCoroutine(m_KeypadMorseManager.Correct());
                    }
                    else
                    {
                        StartCoroutine(m_KeypadMorseManager.Wrong());
                    }
                    break;*/
            }
        }
    }
}