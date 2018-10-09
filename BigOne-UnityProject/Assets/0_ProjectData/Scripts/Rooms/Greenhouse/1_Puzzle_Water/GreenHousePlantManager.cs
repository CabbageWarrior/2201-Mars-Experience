using System.Collections;
using UnityEngine;

public class GreenHousePlantManager : Puzzle
{
    public GameObject plantsDown;
    public GameObject plantsUp;

    //public GameObject flowers;

    public bool turnedOnCubes = false;

    [Header("Monitors and Panels")]
    public GameObject monitorOff;
    public GameObject monitorOn;
    [Space]
    public GameObject panelCloseTheLit;
    public GameObject panelPuzzle;
    public GameObject panelCheckOK;
    public GameObject panelCheckKO;

    GameObject mainBackground;
    GameObject closeTheLitBackground;
    GameObject blackBackground;
    GameObject errorBackground;
    GameObject dropBackground;
    GameObject textRemainingWater;
    GameObject textError;
    GameObject cubesDisplay;
    GameObject canvasButtons;

    public Material fioriereDx;
    public Material fioriereSx;

    void Start()
    {
        fioriereDx.SetColor("_EmissionColor", Color.black);
        fioriereSx.SetColor("_EmissionColor", Color.black);
    }

    //protected void Start()
    //{
    //    #region FIND GAMEOBJECTS
    //    mainBackground = transform.Find("WaterDisplays/MainBackground").gameObject;
    //    closeTheLitBackground = transform.Find("WaterDisplays/CloseTheLitBackground").gameObject;
    //    blackBackground = transform.Find("WaterDisplays/BlackBackground").gameObject;
    //    errorBackground = transform.Find("WaterDisplays/ErrorBackground").gameObject;
    //    dropBackground = transform.Find("WaterDisplays/DropBackground").gameObject;
    //    textRemainingWater = transform.Find("WaterDisplays/CubesDisplay/Water/Canvas/TextRemainingWater").gameObject;
    //    textError = transform.Find("WaterDisplays/ErrorBackground/Canvas/TextError").gameObject;
    //    cubesDisplay = transform.Find("WaterDisplays/CubesDisplay").gameObject;
    //    canvasButtons = transform.Find("Canvas").gameObject;
    //    #endregion

    //    #region SET BACKGROUNDS ALL FALSE
    //    //Our water panel is inactive at the start.
    //    mainBackground.SetActive(false);
    //    closeTheLitBackground.SetActive(false);
    //    textRemainingWater.SetActive(false);
    //    dropBackground.SetActive(false);
    //    errorBackground.SetActive(false);
    //    textError.SetActive(false);
    //    cubesDisplay.SetActive(false);
    //    #endregion
    //}

    //private void Update()
    //{

    //    #region TestWithKeyboard
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        Phase1();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        Phase2();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        Phase3();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        Phase4();
    //    }
    //    #endregion
    //}


    /// <summary>
    /// Adds colors to cubes.
    /// </summary>
    public void Phase1()
    {
        fioriereDx.SetColor("_EmissionColor", Color.white);
        fioriereSx.SetColor("_EmissionColor", Color.white);
    }

    /// <summary>
    /// Shows the "close the panel" tooltip.
    /// </summary>
    public void Phase2()
    {
        //blackBackground.SetActive(false);
        //closeTheLitBackground.SetActive(true);

        monitorOff.SetActive(false);
        monitorOn.SetActive(true);
    }

    /// <summary>
    /// Turns the water puzzle on.
    /// </summary>
    public void Phase3()
    {
        //closeTheLitBackground.SetActive(false);
        //cubesDisplay.SetActive(true);
        //canvasButtons.SetActive(true);
        //turnedOnCubes = true;

        panelCloseTheLit.SetActive(false);
        panelPuzzle.SetActive(true);
        turnedOnCubes = true;
    }

    /// <summary>
    /// Completes the water puzzle.
    /// </summary>
    public void Phase4()
    {
        //mainBackground.SetActive(false);
        //textRemainingWater.SetActive(false);
        //cubesDisplay.SetActive(false);
        //canvasButtons.SetActive(false);
        //dropBackground.SetActive(true);
        panelPuzzle.SetActive(false);
        panelCheckOK.SetActive(true);

        plantsDown.SetActive(false);
        plantsUp.SetActive(true);

        //flowers.SetActive(true);
        
        turnedOnCubes = false;
    }

    /// <summary>
    /// Shows the error background (phase 3) - StartCoroutine in WaterManager script, void Check
    /// </summary>
    public IEnumerator WaterError()
    {
        //errorBackground.SetActive(true);
        //textError.SetActive(true);
        //turnedOnCubes = false;
        //yield return new WaitForSeconds(2);

        //errorBackground.SetActive(false);
        //textError.SetActive(false);
        //turnedOnCubes = true;

        panelPuzzle.SetActive(false);
        panelCheckKO.SetActive(true);
        turnedOnCubes = false;
        yield return new WaitForSeconds(2);

        panelCheckKO.SetActive(false);
        panelPuzzle.SetActive(true);
        turnedOnCubes = true;
    }

    /*
    protected override void OnStateSwitch(GAMESTATE currentState, GAMESTATE nextState, ref bool canSwitch)
    {
        base.OnStateSwitch(currentState, nextState, ref canSwitch);

        // Insert here your customization
        // ...
    }
    */

    /*
    protected override void OnSave(SaveLoadUtilities.PermanentGameData data)
    {
        // Insert here your customization
        // ...

        base.OnSave(data);
    }
    */

    /*
    protected override void OnLoad(SaveLoadUtilities.PermanentGameData data)
    {
        base.OnLoad(data);

        if (IsCompleted)
        {
            // Do things to emulate puzzle completion
        }

        // Insert here your customization
        // ...
    }
    */
}