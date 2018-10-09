namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEventHelper;
    using VRTK.Controllables;
	using System.Collections;
	using VRTK.Highlighters;

    public class Sliders : MonoBehaviour
    {
        public bool isCorrect = false;
        public float correctValue;
        SliderManager sliderManager;

		private	IEnumerator	Start()
		{
			sliderManager = GameObject.FindObjectOfType<SliderManager>();

			VRTK_InteractHaptics haptics = GetComponent<VRTK_InteractHaptics>();
			VRTK_OutlineObjectCopyHighlighter highlighter = GetComponent<VRTK_OutlineObjectCopyHighlighter>();
			VRTK_InteractObjectHighlighter interactHighlighter = GetComponent<VRTK_InteractObjectHighlighter>();
			VRTK_InteractableObject interactable = GetComponent<VRTK_InteractableObject>();
			while( interactable == null )
			{
				interactable = GetComponent<VRTK_InteractableObject>();
				yield return null;
			}
			
			haptics.objectToAffect = interactable;

			interactable.objectHighlighter = highlighter;
			interactHighlighter.objectToAffect = interactable;
			haptics.enabled = true;
			highlighter.Initialise();
			highlighter.enabled = true;
			interactHighlighter.enabled = true;
		}

#pragma warning disable 0618
        protected VRTK_Control_UnityEvents controlEvents;
#pragma warning restore 0618
        protected VRTK_BaseControllable controllableEvents;

        protected virtual void OnEnable()
        {
#pragma warning disable 0618
            if (GetComponent<VRTK_Control>() != null && GetComponent<VRTK_Control_UnityEvents>() == null)
            {
                controlEvents = gameObject.AddComponent<VRTK_Control_UnityEvents>();
            }
            controlEvents = GetComponent<VRTK_Control_UnityEvents>();
#pragma warning restore 0618
            controllableEvents = GetComponent<VRTK_BaseControllable>();

            ManageListeners(true);
        }

        protected virtual void OnDisable()
        {
            ManageListeners(false);
        }

        protected virtual void ManageListeners(bool state)
        {
            if (state)
            {
                if (controlEvents != null)
                {
                    controlEvents.OnValueChanged.AddListener(HandleChange);
                }
                if (controllableEvents != null)
                {
                    controllableEvents.ValueChanged += ValueChanged;
                }
            }
            else
            {
                if (controlEvents != null)
                {
                    controlEvents.OnValueChanged.RemoveListener(HandleChange);
                }
                if (controllableEvents != null)
                {
                    controllableEvents.ValueChanged -= ValueChanged;
                }
            }
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            UpdateText(e.value.ToString("F2"), (e.normalizedValue * 100f).ToString("F0"));
			switch(this.gameObject.name)
			{
				case "SliderControl1":
					if (e.normalizedValue >= 0.90)
					{
						isCorrect = true;
					}
                    else if (e.normalizedValue < 0.90)
                    {
                        isCorrect = false;
                    }
					break;

				case "SliderControl2":
					if (e.normalizedValue <= 0.05)
					{
						isCorrect = true;
					}
                    else if (e.normalizedValue > 0.05)
                    {
                        isCorrect = false;
                    }
                    break;

				case "SliderControl3":
					if (e.normalizedValue >= 0.90)
					{
						isCorrect = true;
					}
                    else if (e.normalizedValue < 0.90)
                    {
                        isCorrect = false;
                    }
                    break;

				case "SliderControl4":
					if(e.normalizedValue <= 0.05)
					{
						isCorrect = true;
					}
                    else if (e.normalizedValue > 0.05)
                    {
                        isCorrect = false;
                    }
                    break;

				case "SliderControl5":
					if (e.normalizedValue <= 0.05)
					{
						isCorrect = true;
					}
                    else if (e.normalizedValue > 0.05)
                    {
                        isCorrect = false;
                    }
                    break;

				case "SliderCheck":
					if (e.normalizedValue >= 0.98)
					{
						if (!isCorrect)
						{
							isCorrect = true;
							//Debug.Log(sliderManager.CheckSliders());
							sliderManager.CheckSliders();
						}
					}
					else if (isCorrect)
					{
						isCorrect = false;
					}
					break;
			}

           // isCorrect = (e.normalizedValue == correctValue);
			//Debug.Log ("Slider: " + this.gameObject.name + " value: " + e.value);
            //Debug.Log ("Slider: " + this.gameObject.name + " value: " + e.normalizedValue);
            //Debug.Log ("intNvalue: " + Mathf.RoundToInt(e.normalizedValue));
            //Debug.Log(Mathf.RoundToInt(e.normalizedValue));

           /* if (this.gameObject.name == "SliderCheck" && e.normalizedValue >= 0.98)
            {
                isCorrect = true;
                Debug.Log(sliderManager.CheckSliders());
            }*/
        }

        protected virtual void HandleChange(object sender, Control3DEventArgs e)
        {
           // UpdateText(e.value.ToString("F2"), (e.normalizedValue * 100f).ToString("F0"));
        }

        protected virtual void UpdateText(string valueText, string normalizedValueText)
        {
            //Debug.LogWarning("Richiamato metodo inutile \"UpdateText\".");
            /*
            if (display != null)
            {
                display.text = valueText;
            }
            */
        }
    }
}