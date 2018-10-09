namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEventHelper;
    using VRTK;
    using VRTK.Controllables;
    using UnityEngine.UI;
	using System.Collections;
	using VRTK.Highlighters;

    public abstract class DialsGeneric : MonoBehaviour
    {
        public Text dialText;
        public Transform sprite;
        public DialsManager dialsManager;
        public GameObject[] barCheck;

#pragma warning disable 0618
        protected VRTK_Control_UnityEvents controlEvents;
#pragma warning restore 0618
        protected VRTK_BaseControllable controllableEvents;


		private	IEnumerator	Start()
		{
			VRTK_InteractHaptics haptics = GetComponent<VRTK_InteractHaptics>();
			if ( haptics == null )
				yield break;

			VRTK_OutlineObjectCopyHighlighter highlighter = GetComponent<VRTK_OutlineObjectCopyHighlighter>();
			VRTK_InteractObjectHighlighter interactHighlighter = GetComponent<VRTK_InteractObjectHighlighter>();
			VRTK_InteractableObject interactable = GetComponent<VRTK_InteractableObject>();
			while( interactable == null )
			{
				interactable = GetComponent<VRTK_InteractableObject>();
				yield return null;
			}
			
			haptics.objectToAffect = interactable;
			haptics.enabled = true;
			highlighter.enabled = true;
			interactHighlighter.enabled = true;
		}




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

        Coroutine cr;
        public virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            UpdateText(e.value.ToString("F2"), (e.normalizedValue * 1000f).ToString("F0"));

            sprite.transform.localScale = new Vector3(e.normalizedValue, sprite.transform.localScale.y, sprite.transform.localScale.z);

            //dialsManager.countCheck = 0;

            //for (int i = 0; i < barCheck.Length; i++)
            //{
            //    if (barCheck[i].gameObject.activeInHierarchy)
            //    {
            //        barCheck[i].gameObject.SetActive(false);
            //    }
            //}

            dialsManager.RestartCountDown();

            if (cr != null)
            {
                StopCoroutine(cr);
            }
            cr = StartCoroutine(dialsManager.CountDown());
        }

        protected virtual void HandleChange(object sender, Control3DEventArgs e)
        {
            UpdateText(e.value.ToString("F2"), (e.normalizedValue * 1000f).ToString("F0"));
        }

        protected virtual void UpdateText(string valueText, string normalizedValueText)
        {
            if (dialText != null)
            {
                dialText.text = valueText;
            }
        }
    }
}
