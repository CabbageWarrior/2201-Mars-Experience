using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTarget : MonoBehaviour
{
    public GameObject overlayClonePrefab;
	public Sprite infoSprite;
	public string infoText;
	
//    MeshFilter meshFilter;
    Renderer myRenderer;

	//[HideInInspector]
    [Space]
    public GameObject overlayCloneInstance;
    MeshFilter overlayCloneMeshFilter;
    Renderer overlayCloneRenderer;

	private		Mesh	combineMesh		= null;

	private void Awake()
	{

		myRenderer = GetComponent<MeshRenderer>();
		if ( myRenderer == null )
			myRenderer = gameObject.AddComponent<MeshRenderer>();

		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

		combineMesh = new Mesh();

		foreach( MeshFilter meshFilterChild in meshFilters )
		{
			List<CombineInstance> combineList = new List<CombineInstance>();

			for (int i = 0; i < meshFilterChild.mesh.subMeshCount; i++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = meshFilterChild.mesh;
                ci.subMeshIndex = i;
                ci.transform = meshFilterChild.transform.localToWorldMatrix;
                combineList.Add(ci);
            }
			combineMesh.CombineMeshes( combineList.ToArray(), true, false );
		}

		overlayCloneInstance = Instantiate( overlayClonePrefab, transform );
		overlayCloneMeshFilter = overlayCloneInstance.GetComponent<MeshFilter>();
		if ( overlayCloneMeshFilter == null )
		{
			overlayCloneMeshFilter = overlayCloneInstance.AddComponent<MeshFilter>();
		}

		overlayCloneRenderer = overlayCloneInstance.GetComponent<Renderer>();
		overlayCloneInstance.SetActive(false);
		overlayCloneMeshFilter.mesh = combineMesh;
		if ( myRenderer.material.GetTexture("_MainTex") )
		{
			overlayCloneRenderer.material.SetTexture("_MainTex", myRenderer.material.GetTexture("_MainTex"));
		}
		
	}

}
