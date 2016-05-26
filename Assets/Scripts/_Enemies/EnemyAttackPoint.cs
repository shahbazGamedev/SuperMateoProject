using UnityEngine;
using System.Collections;

public class EnemyAttackPoint : MonoBehaviour 
{
	private	EnemyManager	aEnemyManager;
	public	bool			aDamageOnContact;

	void Start()
	{
		aEnemyManager	=	transform.root.GetComponentInChildren<EnemyManager>();
	}

	void Update()
	{
		if (aEnemyManager.aCurrentAIState == eEnemyAIState.DIE)
		{
			if (!aDamageOnContact)
				GetComponent<Collider>().enabled	=	false;
		}
	}

	void OnCollisionEnter(Collision pOther)
	{
		if (pOther.transform.tag == "Matt")
		{
			print("damage");
			if (aDamageOnContact)
			{
				mpDamagePlayer(pOther);
			}
			else
			{
				if (aEnemyManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)"))
				{
					if (!aEnemyManager.aIsStunned)
					{
						mpDamagePlayer(pOther);
					}
				}
			}
		}
	}

	void mpDamagePlayer(Collision pOther)
	{
		Vector3	lPushBackForce = (pOther.transform.position - transform.position).normalized * aEnemyManager.aHitForce;
		lPushBackForce.y = 0;

		pOther.gameObject.GetComponent<MattManager>().mpInflictDamageToMatt(aEnemyManager.aStrength, lPushBackForce);
		pOther.transform.root.FindChild("Camera").GetComponent<PerlinShake>().mpInitShake(0.5f, 600.0f, 3.0f);
	}
}
