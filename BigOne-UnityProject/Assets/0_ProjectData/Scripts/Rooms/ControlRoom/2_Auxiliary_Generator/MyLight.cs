using System.Collections;
using UnityEngine;

public class MyLight : MonoBehaviour
{
    public float totalSeconds;      // The total of seconds the flash will last
    public float maxIntensity;      // The maximum intensity the flash will reach
    public Light myLight;           // Your light

    void Start()
    {
        StartCoroutine(FlashNow());
    }

    public IEnumerator FlashNow()
    {
        while (myLight.color == Color.red)
        {
            float waitTime = totalSeconds / 5;
            // Get half of the seconds (One half to get brighter and one to get darker)
            {
                while (myLight.intensity < maxIntensity)
                {
                    myLight.intensity += Time.deltaTime / waitTime; // Increase intensity
                    yield return null;
                }
                while (myLight.intensity > 3)
                {
                    myLight.intensity -= Time.deltaTime / waitTime; //Decrease intensity
                    yield return null;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}