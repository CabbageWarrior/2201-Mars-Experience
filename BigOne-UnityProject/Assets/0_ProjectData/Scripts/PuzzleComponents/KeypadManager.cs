using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BigOne.PuzzleComponents
{
    public class KeypadManager : Puzzle
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
        public string correctPassword = "";
        [Tooltip("Funziona solo per i tastierini liberi (senza password).")]
        public int maxCharacters;

        [Header("Value Display")]
        public DisplayType displayType = DisplayType.CharacterSlots;
        public Text[] displayValues;

        [Space]

        public Text checkDisplay;

        [Header("Audio Effects")]
        public AudioSource positiveResolutionAudio;
        public AudioSource negativeResolutionAudio;

        public int currentTextMesh = 0;

		//Hidden
        [HideInInspector]
        public bool isPuzzleEnabled = false;
        private string currentValue = "";
		private bool isChecking = false;
        #endregion

        protected void Start()
        {
            //Debug.Log("Tastierino abilitato dallo Start!");
            isPuzzleEnabled = true;

            if (correctPassword != "") maxCharacters = correctPassword.Length;
        }

        #region Value Edit
        /// <summary>
        /// Adds a character.
        /// </summary>
        /// <param name="character">Character to add.</param>
        public void AddCharacter(string character)
        {
            if (!isChecking && currentValue.Length < maxCharacters)
            {
                currentValue += "" + character;

                if (displayType == DisplayType.CharacterSlots)
                {
                    displayValues[currentTextMesh].text = character;
                    currentTextMesh++;
                }
                else
                {
                    displayValues[currentTextMesh].text = currentValue;
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
        public void RemoveCharacter(bool forceCheck = false)
        {
            if ((!isChecking || forceCheck) && currentValue.Length > 0)
            {
                currentValue = currentValue.Substring(0, currentValue.Length - 1);

                if (displayType == DisplayType.CharacterSlots)
                {
                    currentTextMesh--;
                    displayValues[currentTextMesh].text = "";
                }
                else
                {
                    displayValues[currentTextMesh].text = currentValue;
                }
            }
        }

		public void EmptyValue()
		{
			while(currentValue.Length > 0)
			{
				RemoveCharacter(true);
			}
		}
        #endregion

        #region Callbacks
        public IEnumerator Correct()
        {
            if (positiveResolutionAudio) positiveResolutionAudio.Play();
            isPuzzleEnabled = false;

            if (checkDisplay)
            { 
                checkDisplay.text = "OK";
                yield return new WaitForSeconds(1);
                checkDisplay.text = "";
            }
            OnCompletion();
			//currentValue = "";
			EmptyValue();

			isChecking = false;
        }

        public IEnumerator Wrong()
        {
            if (negativeResolutionAudio) negativeResolutionAudio.Play();

            if (checkDisplay)
            {
                checkDisplay.text = "Error";
                yield return new WaitForSeconds(2);
                checkDisplay.text = "";
            }
			//currentValue = "";
			EmptyValue();

			isChecking = false;
        }
        #endregion

        public void CheckCode()
        {
            isChecking = true;

            if (IsCorrect())
            {
                StartCoroutine(Correct());
            }
            else
            {
                StartCoroutine(Wrong());
            }
        }

        public bool IsCorrect()
        {
            return (correctPassword == "" || currentValue == correctPassword);
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

            // insert here your customization
            // ...
        }
        */
    }
}