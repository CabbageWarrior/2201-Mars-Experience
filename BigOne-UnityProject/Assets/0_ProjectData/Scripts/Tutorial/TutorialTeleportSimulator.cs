using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Cutscene;

public class TutorialTeleportSimulator : MonoBehaviour
{
	public	Collider					player;
	public	GameObject					maria;
	public	GameObject					teleportSnapPoint1;
	public	GameObject					teleportSnapPoint2;
	public	GameObject					teleportSnapPoint3;
	private MARIAManager				mariaManager;
	public	float						timeBeforeTutorialAudio		= 3.0f;        // coroutine parameter before Maria's first audio
	public	CutsceneSequence			mariasFirstAudio;
	

	private void Start()
	{
		mariaManager = maria.GetComponent<MARIAManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other == player)
		{
			if (gameObject.name == "TeleportSnapPoint1")
			{
				maria.SetActive(true);
				mariaManager.SetGrabbable(false);
				mariaManager.IsPatrolEnabled = false;
				StartMariaCoroutine();
			}
			if (gameObject.name == "TeleportSnapPoint2")
			{
				teleportSnapPoint1.SetActive(false);
				teleportSnapPoint3.SetActive(true);
			}
			if (gameObject.name == "TeleportSnapPoint3")
			{
				teleportSnapPoint2.SetActive(false);
				mariaManager.SetGrabbable(true);
			}
		}
	}

	public void StartMariaCoroutine()
	{
		StartCoroutine(PlayDelayedTutorialAudio(mariasFirstAudio));
	}

	public IEnumerator PlayDelayedTutorialAudio(CutsceneSequence sequence)
	{
		yield return new WaitForSeconds(timeBeforeTutorialAudio);
		sequence.Play();
	}
}
