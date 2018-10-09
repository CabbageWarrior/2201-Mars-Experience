using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class FinalDoorManager : MonoBehaviour {
	
	public Animator anim;

    public GameObject monitorCanvas;

	public VRTK_SnapDropZone vRTK_SnapDropZone;

	public GameEvent m_OnSnapped;

	void Start()
	{
		vRTK_SnapDropZone.ObjectSnappedToDropZone += ObjectSnappedToDropZone;
	}

	void ObjectSnappedToDropZone(object sender, SnapDropZoneEventArgs e)
	{
		if (e.snappedObject.name == "Clorofilla")
		{
			ExecuteCallback(e.snappedObject.gameObject);
		}
	}

	void ExecuteCallback(GameObject gobj)
	{
		Destroy(gobj);
		Debug.Log("Tessera Inserita");
		anim.SetTrigger("Opening");
        monitorCanvas.SetActive(true);

		OnSnapped();
	}

	void OnSnapped()
	{
		if (m_OnSnapped != null)
		{
			m_OnSnapped.Invoke();
		}
	}
}