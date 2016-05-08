using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour 
{
	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			CheckpointManager.mpSetNewCheckPoint(transform.position);
			Destroy(gameObject);
		}
	}
}
