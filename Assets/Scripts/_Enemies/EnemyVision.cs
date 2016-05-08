using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour 
{
	private	EnemyManager	aEnemyManager;

	void Start()
	{
		aEnemyManager	=	transform.parent.GetComponent<EnemyManager>();
	}

	void OnTriggerEnter(Collider pCollider)
	{
		if (pCollider.tag == "Matt")
		{
			// Go and chase Matt!
			if (aEnemyManager.aCurrentAIState == eEnemyAIState.WANDER)
			{
				aEnemyManager.audioSource.PlayOneShot(aEnemyManager.aSpotLaughSFX);
			}

			aEnemyManager.aCurrentAIState	=	eEnemyAIState.APPROACHING;
		}
	}

}
