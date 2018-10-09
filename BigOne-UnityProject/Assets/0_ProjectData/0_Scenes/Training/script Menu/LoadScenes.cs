using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScenes : MonoBehaviour {

    public float timer;
    public int sceneIndex;

    // in teoria la variabile background non serve più
    public GameObject background;

    public void Start()
    {
        {
            background.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
        }
    }

    public void AfterTime (){
        if (background)
        {
            background.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
        }

        Invoke("LoadNextScene",timer);
	}

    public void LoadNextScene(){
        SceneManager.LoadScene(sceneIndex);
      }

    public void ExitGame () {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
