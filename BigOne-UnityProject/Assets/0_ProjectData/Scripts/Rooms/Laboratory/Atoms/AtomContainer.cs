using UnityEngine;
using UnityEngine.UI;

public class AtomContainer : MonoBehaviour
{
    public AtomGroupGenerator groupGenerator;
    public GameObject removeAtomsGameObject;
    public Text elementNameTextComponent;
    public Text elementQuantityTextComponent;

    [Header("Element properties")]
    public string elementName;
    public int nAtoms;

    [Space]
    public bool isSlotAvailable = true;

    public void GenerateAtoms(string p_elementName, int p_nAtoms)
    {
        if (isSlotAvailable)
        {
            isSlotAvailable = false;

            removeAtomsGameObject.SetActive(true);
            elementNameTextComponent.gameObject.SetActive(true);
            elementQuantityTextComponent.gameObject.SetActive(true);

            elementNameTextComponent.text = p_elementName;
            elementQuantityTextComponent.text = p_nAtoms.ToString();
            groupGenerator.CreateAtoms(p_nAtoms);

			elementName = p_elementName;
			nAtoms = p_nAtoms;
        }
    }
    public void DiscardAtoms()
    {
        if (!isSlotAvailable)
        {
            isSlotAvailable = true;

            removeAtomsGameObject.SetActive(false);
            elementNameTextComponent.gameObject.SetActive(false);
            elementQuantityTextComponent.gameObject.SetActive(false);

			elementName = "";
			nAtoms = 0;

            foreach (Transform child in groupGenerator.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}