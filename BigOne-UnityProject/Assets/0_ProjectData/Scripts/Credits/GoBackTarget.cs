using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoBackTarget : MonoBehaviour
{

	public	Image FaderImage = null;

	AsyncOperation async = new AsyncOperation();

	private	bool	m_Loaded = false;

	private	float	fadeTime	= 1.0f;

	private	bool	IsFading = false;



	private	IEnumerator	Start()
	{
		yield return new WaitForSeconds( 2f );

		async = SceneManager.LoadSceneAsync( 0 );
		async.allowSceneActivation = false;

		while( async.progress < 0.9f )
			yield return null;

		m_Loaded = true;
		FaderImage.color = Color.clear;
	}


	 void OnCollisionEnter( Collision collision )
    {
		if ( m_Loaded && IsFading == false )
		{
			if (collision.gameObject.tag == "Bullet")
			{
				IsFading = true;
				StartCoroutine( FadeOut() );
			}
		}
    }


	IEnumerator	FadeOut()
	{
		float interpolant = 0f;
		float currentTime = 0f;

		while( interpolant < 1f )
		{
			currentTime += Time.deltaTime;
			interpolant = currentTime / fadeTime;
			FaderImage.color = Color.Lerp ( Color.clear, Color.black, interpolant );
			yield return null;
		}
		IsFading = false;
		async.allowSceneActivation = true;
	}





	/*
    public GameObject loadingCanvasesGameObject;

    public IEnumerator BackToMain()
    {
        //SceneManager.LoadScene("Main_Scene");

        loadingCanvasesGameObject.SetActive(true);

        AsyncOperation backToMenuAsync = SceneManager.LoadSceneAsync("Main_Scene");

        while(!backToMenuAsync.isDone)
        {
            //Debug.Log(backToMenuAsync.progress);
            yield return new WaitForSeconds(.5f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(BackToMain());
        }
    }
	*/
}
