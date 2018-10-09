using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsSceneManager : MonoBehaviour
{

	public	Image FaderImage = null;

	AsyncOperation async = new AsyncOperation();

	private	bool	m_Loaded = false;

	private	float	fadeTime	= 1.0f;

	private	bool	IsFading = false;

	private	IEnumerator	Start()
	{
		async = SceneManager.LoadSceneAsync( 1 );
		async.allowSceneActivation = false;

		while( async.progress < 0.9f )
			yield return null;

		m_Loaded = true;
		FaderImage.color = Color.clear;
	}

	public void GoToFinalScene()
    {
		if ( m_Loaded && IsFading == false )
		{
			IsFading = true;
			StartCoroutine( FadeOut() );
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

}