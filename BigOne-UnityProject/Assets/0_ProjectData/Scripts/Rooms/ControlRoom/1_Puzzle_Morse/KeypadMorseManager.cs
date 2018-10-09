using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeypadMorseManager : Puzzle
{
    public string correctPass = "--.-.-....-";
    public Text CodeDisplay;
	public GameObject morseCode;
	

    public enum KeypadCodes
    {
        DOT,
        DASH,
        NONE
    }

    public enum KeypadOperations
    {
        ADD,
        REMOVE,
        CHECK
    }

    public IEnumerator Correct()
    {
        CodeDisplay.text = "OK";
        yield return new WaitForSeconds(1);
        CodeDisplay.text = "";
    }

    public IEnumerator Wrong()
    {		
        CodeDisplay.text = "Error";
        yield return new WaitForSeconds(2);
        CodeDisplay.text = "";
    }

    public void CheckMorse()
    {
        if (CodeDisplay.text == correctPass)
        {
			OnCompletion();
          
        }
        else
        {
			OnError();
        }
    }

	public void StartCoroutineForCorrectPuzzle()
	{
		StartCoroutine(Correct());
	}
	public void StartCoroutineForWrongPuzzle()
	{
		StartCoroutine(Wrong());
	}

    public void AddDot()
    {
        CodeDisplay.text += ".";
    }

    public void AddDash()
    {
        CodeDisplay.text += "-";
    }

    public void RemoveCharacter()
    {
        if (CodeDisplay.text.Length > 0)
        {
           CodeDisplay.text = CodeDisplay.text.Remove(CodeDisplay.text.Length - 1);
        }
    }
}