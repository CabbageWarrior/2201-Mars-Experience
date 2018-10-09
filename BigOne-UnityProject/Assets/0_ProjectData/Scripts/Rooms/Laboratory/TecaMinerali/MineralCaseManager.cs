using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MineralCaseManager : Puzzle
{
    #region Enumerators
    public enum DisplayType
    {
        CharacterSlots,
        WholeString
    }

    public enum ButtonValueType
    {
        ACTION,
        INTEGER,
        STRING
    }

    public enum KeypadOperation
    {
        ADD,
        REMOVE,
        CHECK
    }
    #endregion

    #region Properties
    public string[] correctPassword;

    [Header("Value Display")]
    public DisplayType displayType = DisplayType.CharacterSlots;
    public Text[] displayValues;
    [Space]
    public Text checkDisplay;

    [Header("Audio Effects")]
    public AudioSource positiveResolutionAudio;
    public AudioSource negativeResolutionAudio;

    public int currentTextMesh = 0;
    [HideInInspector]
    public bool isPuzzleEnabled = false;
    public string[] currentValue;
    public Transform arrow;
    public Transform[] arrowPosition;
	public GameObject magMineral;
    public GameObject displays;
    public GameObject arrows;
    public GameObject letters;
    public GameObject menu;

    Animator anim;
    #endregion

    protected void Start()
    {
        //Debug.Log("Tastierino abilitato dallo Start!");
        isPuzzleEnabled = true;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        arrow.localPosition = arrowPosition[currentTextMesh].localPosition;
    }

    #region Value Edit
    /// <summary>
    /// Adds a character.
    /// </summary>
    /// <param name="character">Character to add.</param>
    public void AddCharacter(string character)
    {
        if (currentValue[currentTextMesh].Length < correctPassword[currentTextMesh].Length)
        {
            currentValue[currentTextMesh] += "" + character;

            if (displayType == DisplayType.CharacterSlots)
            {
                displayValues[currentTextMesh].text = character;
                currentTextMesh++;
            }
            else
            {
                displayValues[currentTextMesh].text = currentValue[currentTextMesh];
            }
        }
    }
    
    /// <summary>
    /// Adds a number.
    /// </summary>
    /// <param name="number">Number to add.</param>
    public void AddNumber(int number)
    {
        AddCharacter(number.ToString());
    }

    /// <summary>
    /// Removes the last character.
    /// </summary>
    public void RemoveCharacter()
    {
        if (currentValue[currentTextMesh].Length > 0)
        {
            currentValue[currentTextMesh] = currentValue[currentTextMesh].Substring(0, currentValue[currentTextMesh].Length - 1);

            if (displayType == DisplayType.CharacterSlots)
            {
                currentTextMesh--;
                displayValues[currentTextMesh].text = "";
            }
            else
            {
                displayValues[currentTextMesh].text = currentValue[currentTextMesh];
            }
        }
    }
    #endregion

    #region Callbacks
    public IEnumerator Correct()
    {
        isPuzzleEnabled = false;

        positiveResolutionAudio.Play();

		OnCompletion();
        
        checkDisplay.text = "OK";
        yield return new WaitForSeconds(2);
        checkDisplay.text = "";

       
    }

    public IEnumerator Wrong()
    {
        negativeResolutionAudio.Play();

        checkDisplay.text = "Error";
        yield return new WaitForSeconds(2);
        checkDisplay.text = "";
    }
    #endregion

    public void CheckCode()
    {
        if (currentValue[0] == correctPassword[0] &&
            currentValue[1] == correctPassword[1] &&
            currentValue[2] == correctPassword[2]
        )
        {
            StartCoroutine(Correct());
        }
        else
        {
            StartCoroutine(Wrong());
        }
    }

    public void NextRow()
    {
        if (currentTextMesh == 2)
        {
            currentTextMesh = 0;
        }
        else
        {
            currentTextMesh++;
        }
    }

    public void PreviousRow()
    {
        if (currentTextMesh == 0)
        {
            currentTextMesh = 2;
        }
        else
        {
            currentTextMesh--;
        }
    }

	public void OpenCase()
	{
        anim.Play("Opening");
        displays.SetActive(false);
        arrows.SetActive(false); ;
        letters.SetActive(false); ;
        menu.SetActive(false); ;
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

        // Insert here your customization
        // ...
    }
    */
}