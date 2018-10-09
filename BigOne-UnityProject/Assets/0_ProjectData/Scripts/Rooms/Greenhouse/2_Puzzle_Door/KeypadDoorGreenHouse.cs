namespace VRTK.Examples
{
    using UnityEngine;

    public class KeypadDoorGreenHouse : MonoBehaviour
    {
        public Animator doorAnimator;

        public void OpenDoor()
        {
            doorAnimator.Play("Opening");
        }
    }
}