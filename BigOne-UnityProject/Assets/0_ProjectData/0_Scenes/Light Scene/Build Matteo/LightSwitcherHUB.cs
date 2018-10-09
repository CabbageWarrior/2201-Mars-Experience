using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcherHUB : MonoBehaviour {


	public		float			m_TurningTime			= 2f;

	[SerializeField]
	private		Light[]			m_Lights				= null;
	[SerializeField]
	private		Renderer[]		m_Renderers				= null;


	private void Awake()
	{
		List<Renderer> renderers = new List<Renderer>();
		m_Lights = GetComponentsInChildren<Light>();
		foreach( var light in m_Lights )
		{
			Renderer renderer = light.GetComponent<Renderer>();
            //renderer.material.SetColor( "_EmissionColor", Color.black );
            light.intensity = 1f;
			renderers.Add ( renderer );
		}
		m_Renderers = renderers.ToArray();
	}
	

	private	IEnumerator	TurningLights()
	{
		float	currentTime = 0f;
		float	interpolant	= 0f;

        while (interpolant < 1f)
        {
            currentTime += Time.deltaTime;
            interpolant = currentTime / m_TurningTime;
            for (int i = 0; i < m_Lights.Length; i++)
            {
                Light light = m_Lights[i];
                Renderer renderer = m_Renderers[i];

                light.intensity = interpolant;
                renderer.material.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, interpolant));
            }
            yield return null;
        }

  //      for ( int i = 0; i < m_Lights.Length; i++ )
		//{
		//	Light light = m_Lights[ i ];
		//	Renderer renderer = m_Renderers[ i ];

		//	light.intensity =  1f;
		//	renderer.material.SetColor( "_EmissionColor", Color.white);
		//}
  //      yield return null;
	}

    private IEnumerator TurningOffLights()
    {
        float currentTime = 0f;
        float interpolant = 0f;

        while (interpolant < 1f)
        {
            currentTime += Time.deltaTime;
            interpolant = currentTime / m_TurningTime;
            for (int i = 0; i < m_Lights.Length; i++)
            {
                Light light = m_Lights[i];
                Renderer renderer = m_Renderers[i];

                light.intensity -= interpolant;
                renderer.material.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.black, interpolant));
            }
            yield return null;
        }

        //for (int i = 0; i < m_Lights.Length; i++)
        //{
        //    Light light = m_Lights[i];
        //    Renderer renderer = m_Renderers[i];

        //    light.intensity = 0.001f;
        //    renderer.material.SetColor("_EmissionColor", Color.black);
        //}
        //yield return null;
    }

    public void	TurnOnLights()
	{
		StartCoroutine( TurningLights() );
	}

    public void TurnOffLights()
    {
        StartCoroutine(TurningOffLights());
    }

}
