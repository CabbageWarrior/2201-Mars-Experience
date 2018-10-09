using UnityEngine;
using VRTK;
using VRTK.UnityEventHelper;

namespace BigOne.PuzzleComponents
{
    public class KeypadButton : MonoBehaviour
    {
        public KeypadManager keypadManager;
        public KeypadManager.ButtonValueType buttonValueType;
        public KeypadManager.KeypadOperation operation;
        public string value;

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
            if (!keypadManager.isPuzzleEnabled)
            {
                Debug.Log("Puzzle spento!");
                return;
            }

            // VRTK_Logger.Info("Pushed");

            switch (buttonValueType)
            {
                case KeypadManager.ButtonValueType.ACTION:
                    switch (operation)
                    {
                        case KeypadManager.KeypadOperation.CHECK:
                            keypadManager.CheckCode();
                            break;
                        case KeypadManager.KeypadOperation.REMOVE:
                            keypadManager.RemoveCharacter();
                            break;
                    }
                    break;
                case KeypadManager.ButtonValueType.INTEGER:
                    switch (operation)
                    {
                        case KeypadManager.KeypadOperation.ADD: //Adding numbers to string "Input" everytime we push the equivalent button
                            keypadManager.AddNumber(System.Int32.Parse(value));
                            break;
                    }
                    break;
                case KeypadManager.ButtonValueType.STRING:
                    switch (operation)
                    {
                        case KeypadManager.KeypadOperation.ADD: //Adding numbers to string "Input" everytime we push the equivalent button
                            keypadManager.AddCharacter(value);
                            break;
                    }
                    break;
            }
        }
    }
}