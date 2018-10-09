using UnityEngine;

public class AtomGroupGenerator : MonoBehaviour
{
    public GameObject atomPrefab;
    public int[] atomsPerCircle = { 1, 6, 13, 20, 27, 50 };
    public float circleDeltaSpace = .2f;
    public float xMultiplier = 1;
    public float yMultiplier = 1;

    public void CreateAtoms(int nAtoms)
    {
        if (nAtoms == 0) return;

        int nRemaingAtoms = nAtoms;
        int circleLevel = 0;

        int nAtomsToPlaceInThisLevel;
        float spawnLevelAngle;
        while (nRemaingAtoms > 0)
        {
            nAtomsToPlaceInThisLevel = atomsPerCircle[circleLevel];

            if (nAtomsToPlaceInThisLevel > nRemaingAtoms) nAtomsToPlaceInThisLevel = nRemaingAtoms;

            nRemaingAtoms -= nAtomsToPlaceInThisLevel;

            spawnLevelAngle = 2 * Mathf.PI / nAtomsToPlaceInThisLevel;

            for (int i = 0; i < nAtomsToPlaceInThisLevel; i++)
            {
                float currentAngle = spawnLevelAngle * i;

                Vector3 currentPosition = //transform.position +
                    new Vector3(
                    Mathf.Sin(currentAngle) * xMultiplier,
                    Mathf.Cos(currentAngle) * yMultiplier,
                    0
                    ) * circleLevel * circleDeltaSpace;

               GameObject newAtom = Instantiate(atomPrefab, this.transform);
                newAtom.transform.localPosition += currentPosition;
                newAtom.transform.localScale = new Vector3(newAtom.transform.localScale.x * xMultiplier, newAtom.transform.localScale.y * yMultiplier, 1);
            }

            circleLevel++;
        }
    }
}