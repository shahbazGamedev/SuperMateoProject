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
				Vector3	lPushBackForce = (pOther.transform.position - transform.position).normalized * aHitForce;
				lPushBackForce.y = 0;

				Miedo	lMiedoRef	=	pOther.gameObject.GetComponent<Miedo>();

				if (lMiedoRef.enabled)
				{
					if (lMiedoRef.aMiedoState == eMiedoPhase.STEALTH)
					{
						//enemy will spot Matt whenever he stumbles upon it

						lMiedoRef.mpActivateDefensiveMode();

						// Go and chase Matt!
						if (aCurrentAIState == eEnemyAIState.WANDER)
						{
							audioSource.PlayOneShot(aSpotLaughSFX);
						}

						aCurrentAIState	=	eEnemyAIState.APPROACHING;
					}
				}

				pOther.gameObject.GetComponent<MattManager>().mpInflictDamageToMatt(aStrength * 0.5f, lPushBackForce);
				pOther.transform.root.FindChild("Camera").GetComponent<PerlinShake>().mpInitShake(0.5f, 600.0f, 3.0f);
			}
		}
	}

	void Start()
	{
		mpInitBehaviour();
		mpInitStatus();
	}

	void Update()
	{
		if (Input.GetButtonDown("Y"))
		{
			mpInflictDamage(25.0f, (transform.position - aTarget.transform.position).normalized * 5000.0f);
		}
	}
}
