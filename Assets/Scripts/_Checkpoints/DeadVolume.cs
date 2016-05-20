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
				pOther.GetComponent<MattPhysics>().aMattJustRespawned	=	true;
			}
			pOther.GetComponent<MattMATEA>().mpDisableEmotions();
			CheckpointManager.mpLerpCameraOverlay();
		}
	}
}
