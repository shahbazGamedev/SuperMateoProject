using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour 
{
	private	EnemyManager	aEnemyManager;

	void Start()
	{
		aEnemyManager	=	transform.parent.GetComponent<EnemyManager>();
	}

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			if (aEnemyManager.aCurrentAIState != eEnemyAIState.DIE)
			{
				Miedo	lMiedoRef	=	pOther.GetComponent<Miedo>();

				if (lMiedoRef.enabled)
				{
					if (lMiedoRef.aMiedoState == eMiedoPhase.STEALTH)
						return;
				}

				// Go and chase Matt!
				if (aEnemyManager.aCurrentAIState == eEnemyAIState.WANDER)
				{
					aEnemyManager.audioSource.PlayOneShot(aEnemyManager.aSpotLaughSFX);
				}

				aEnemyManager.aCurrentAIState	=	eEnemyAIState.APPROACHING;
			}
		}
	}

}
