using UnityEngine;
using System.Collections;

public class EnemyAttackPoint : MonoBehaviour 
{
	private	EnemyManager	aEnemyManager;

	void Start()
	{
		aEnemyManager	=	transform.root.GetComponentInChildren<EnemyManager>();
	}

	void Update()
	{
		if (aEnemyManager.aCurrentAIState == eEnemyAIState.DIE)
		{
			GetComponent<Collider>().enabled	=	false;
		}
	}

	void OnCollisionEnter(Collision pOther)
	{
		if (pOther.transform.tag == "Matt")
		{
			if (aEnemyManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)"))
			{
				if (!aEnemyManager.aIsStunned)
				{
					Vector3	lPushBackForce = (pOther.transform.position - transform.position).normalized * aEnemyManager.aHitForce;
					lPushBackForce.y = 0;

					pOther.gameObject.GetComponent<MattManager>().mpInflictDamageToMatt(aEnemyManager.aStrength, lPushBackForce);
					pOther.transform.root.FindChild("Camera").GetComponent<PerlinShake>().mpInitShake(0.5f, 600.0f, 3.0f);
				}
			}
		}
	}
}
