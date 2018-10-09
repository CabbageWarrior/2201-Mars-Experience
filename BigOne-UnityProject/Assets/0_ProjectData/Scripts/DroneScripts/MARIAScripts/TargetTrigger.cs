using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public float interactionRange;
    //public float timeBeforeRandomization; // Only if we need a different one for every target object

    [SerializeField]
    MARIAInteraction[] possibleInteractions;
    [SerializeField]
    MARIAInteraction interaction;

    private void Awake()
    {
        possibleInteractions = gameObject.GetComponents<MARIAInteraction>();
    }

    public bool CheckForInteractions()
    {
        return possibleInteractions.Length > 0;
    }

    public MARIAInteraction GetRandomInteraction()
    {
        if (CheckForInteractions())
        {
            interaction = possibleInteractions[Random.Range(0, possibleInteractions.Length)];
        }
        return interaction;
    }
}
