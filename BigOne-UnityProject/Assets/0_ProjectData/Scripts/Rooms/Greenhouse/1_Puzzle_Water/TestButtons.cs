namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEventHelper;

    public class TestButtons : MonoBehaviour
    {
        public WaterManager wm; //Water Manager script
        public WaterManager.WaterColor color; //Enum colors
        public WaterManager.WaterOperation operation; //Enum operations

        public GreenHousePlantManager pm; //Plant Manager script

        private VRTK_Button_UnityEvents buttonEvents;

        VRTK_Button button;

        void Start()
        {
            button = GetComponent<VRTK_Button>();

            button.ValueChanged += changed;

            buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
            if (buttonEvents == null)
            {
                buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
            }
            buttonEvents.OnPushed.AddListener(handlePush);

           // wm = FindObjectOfType<WaterManager>();
           // pm = FindObjectOfType<GreenHousePlantManager>();
        }

        private void changed(object sender, Control3DEventArgs e)
        {

            Debug.Log( "ZITTO CHECCO!!!" );

        }


        private void handlePush(object sender, Control3DEventArgs e) //Triggers on button pushed
        {
            VRTK_Logger.Info("Pushed");

            if (pm.turnedOnCubes == true)
            {
                switch (operation)
                {
                    case WaterManager.WaterOperation.ADD:
                        switch (color)
                        {
                            case WaterManager.WaterColor.RED:
                                wm.RedPlus();
                                break;
                            case WaterManager.WaterColor.GREEN:
                                wm.GreenPlus();
                                break;
                            case WaterManager.WaterColor.BLUE:
                                wm.BluePlus();
                                break;
                            case WaterManager.WaterColor.ORANGE:
                                wm.OrangePlus();
                                break;
                        }
                        break;
                    case WaterManager.WaterOperation.REMOVE:
                        switch (color)
                        {
                            case WaterManager.WaterColor.RED:
                                wm.RedMinus();
                                break;
                            case WaterManager.WaterColor.GREEN:
                                wm.GreenMinus();
                                break;
                            case WaterManager.WaterColor.BLUE:
                                wm.BlueMinus();
                                break;
                            case WaterManager.WaterColor.ORANGE:
                                wm.OrangeMinus();
                                break;
                        }
                        break;
                    case WaterManager.WaterOperation.CHECK:
                        wm.Check();
                        break;
                }
            }
        }
    }
}