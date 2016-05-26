using UnityEngine;
using System.Collections;

public class DeadVolume : MonoBehaviour 
{
	public bool aPlayFallingSound;

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			if (aPlayFallingSound)
			{
				GetComponent<AudioSource>().Play();
				pOther.GetComponent<MattManager>().mpRespawnMatt();
			}

			CheckpointManager.mpLerpCameraOverlay();
		}
	}
}
