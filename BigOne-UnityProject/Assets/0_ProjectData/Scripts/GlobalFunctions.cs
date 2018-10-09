using UnityEngine;
public class GlobalFunctions : MonoBehaviour
{
    /// <summary> Use this to identity is executing in editor or in build </summary>
#if UNITY_EDITOR
    public const bool InEditor = true;
#else
	public	const	bool InEditor = false;
#endif

    public void Quit()
    {
        if (InEditor)
            // Commented for debugging stuff
            //UnityEditor.EditorApplication.isPlaying = false;

            Debug.Log("[[[ YOU PRESSED THE \"CLOSE APPLICATION\" BUTTON!!!!!!!! ]]]");
        else
#pragma warning disable CS0162 // È stato rilevato codice non raggiungibile
            Application.Quit();
#pragma warning restore CS0162 // È stato rilevato codice non raggiungibile
    }
}
