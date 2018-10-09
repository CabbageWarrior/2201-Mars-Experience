using UnityEngine;

public class Atom : MonoBehaviour
{
    // vars
    private bool shakeOn = false;
    public float shakePower = 1;

    /// <summary>
    /// Sprite original position.
    /// </summary>
    private Vector3 originPosition;

    void Start()
    {
        ShakeCameraOn();
    }

    void Update()
    {
        // If shake is enabled
        if (shakeOn)
        {
            // Reset original position
            transform.localPosition = originPosition;

            // Generate random position in a 1 unit circle and add power
            Vector2 ShakePos = Random.insideUnitCircle * shakePower;

            // Transform to new position adding the new coordinates
            transform.localPosition = new Vector3(transform.localPosition.x + ShakePos.x, transform.localPosition.y + ShakePos.y, transform.localPosition.z);
        }
    }

    /// <summary>
    /// Shake Camera On.
    /// </summary>
    /// <param name="sPower">Shaking power.</param>
    public void ShakeCameraOn()
    {
        // Save position before start shake.
        // This is really important otherwise
        // the sprite can goes away and will
        // not return in native position.
        originPosition = transform.localPosition;

        // Enable shaking and setting power.
        shakeOn = true;
    }

    /// <summary>
    /// Shake Camera Off.
    /// </summary>
    public void ShakeCameraOff()
    {
        shakeOn = false;

        // Reset original position.
        transform.localPosition = originPosition;
    }
}