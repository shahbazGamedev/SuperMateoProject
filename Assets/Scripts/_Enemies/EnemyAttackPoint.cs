using UnityEngine;
using System.Collections;

public class EnemyAttackPoint : MonoBehaviour 
{
	private	EnemyManager	aEnemyManager;

	void Start()
	{
		aEnemyManager	=	transform.root.GetComponent<EnemyManager>();
		if (GetComponent<Collider>() == null)
		{
			Debug.LogError("No collider on attack point");
		}
	}

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
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
