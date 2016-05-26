using UnityEngine;
using System.Collections;

public class EnemyManager : EnemyStatus 
{
	void OnCollisionEnter(Collision pOther)
	{
		if (pOther.collider.tag == "Matt")
		{
			if (aCurrentAIState == eEnemyAIState.DIE)
				return;

			if (!aIsStunned)
			{
				Miedo	lMiedoRef	=	pOther.gameObject.GetComponent<Miedo>();

				if (lMiedoRef.enabled)
				{
					if (lMiedoRef.aMiedoState == eMiedoPhase.STEALTH)
					{
						//enemy will spot Matt whenever he stumbles upon it

						lMiedoRef.mpActivateDefensiveMode();
						pOther.transform.root.FindChild("Camera").GetComponent<PerlinShake>().mpInitShake(0.5f, 600.0f, 3.0f);

						// Go and chase Matt!
						if (aCurrentAIState == eEnemyAIState.WANDER)
						{
							audioSource.PlayOneShot(aSpotLaughSFX);
						}

						aCurrentAIState	=	eEnemyAIState.APPROACHING;
					}
				}
			}
		}
	}

	void Start()
	{
		mpInitBehaviour();
		mpInitStatus();
	}
}
