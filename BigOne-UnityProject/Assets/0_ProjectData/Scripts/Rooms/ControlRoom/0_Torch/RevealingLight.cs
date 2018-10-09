using UnityEngine;

public class RevealingLight : MonoBehaviour
{
    public Transform blacklight;
    public TorchManager torchManager;

    private Renderer rend;

    private void Start()
    {
        torchManager = FindObjectOfType<TorchManager>();

		// fix for missing refs !!!
		if (blacklight == null || torchManager == null )
		{
			enabled = false;
			return;
		}

        

		rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (torchManager.blackLight.activeSelf)
        {
            rend.material.SetVector("_LightPos", blacklight.position);
            rend.material.SetVector("_LightDir", blacklight.forward);
        }
        else
        {
            rend.material.SetVector("_LightPos", Vector3.zero);
            rend.material.SetVector("_LightDir", Vector3.zero);
        }
    }
}
