using UnityEngine;
using System.Collections;

public class DeadVolume : MonoBehaviour 
{
	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			StartCoroutine(mcWaitBeforeSending());
		}
	}

	IEnumerator	mcWaitBeforeSending()
	{
		CheckpointManager.mpLerpCameraOverlay();
		yield return new WaitForSeconds(0.4f);
		CheckpointManager.mpSendMattToCheckpoint();
	}

}
