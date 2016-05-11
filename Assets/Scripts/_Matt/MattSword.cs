using UnityEngine;
using System.Collections;

public class MattSword : MonoBehaviour 
{
	public	GameObject	aSparks;
	public	float		aHitForce;

	public	AudioClip[]	aClips;
	private	AudioSource	aAudioSource;

	private	bool		aAttackButtonIsDown;

	private	MattManager	aMattManager;

	private	eMattState	aStateBeforeAttacking;

	void Start()
	{
		aMattManager	=	transform.root.GetComponentInChildren<MattManager>();
		aAudioSource	=	GetComponent<AudioSource>();
	}

	void OnTriggerStay(Collider pOther)
	{
		if (pOther.tag == "Enemy")
		{
			
			if (aAttackButtonIsDown)
			{
				Vector3	lPushBackForce = (pOther.transform.position - transform.parent.position).normalized * aHitForce;
				lPushBackForce.y = 0;

				// play hit animation on enemy
				pOther.GetComponent<EnemyManager>().aCurrentAnimState	=	eEnemyAnimState.HIT;

				// damage enemy
				pOther.GetComponent<EnemyManager>().mpInflictDamage(aMattManager.currentStrength, lPushBackForce);

//				Destroy(Instantiate(aSparks, pOther.transform.position, Quaternion.identity), 2.0f);

				//let go button
				aAttackButtonIsDown	=	false;

//				aAudioSource.PlayOneShot(aClips[Random.Range(0, 2)]);
			}
		}
	}

	IEnumerator mcAttack()
	{
		aAttackButtonIsDown = true;

		do
		{
			yield return null;
		}
		while (aMattManager.aAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)"));

		aMattManager.aCurrentState	=	aStateBeforeAttacking;
		aAttackButtonIsDown = false;
	}

	void Update()
	{
		if (Input.GetButtonDown("X"))
		{
			if (aMattManager.aBiorhythm == eMatea.TRISTEZA)
			{
				aMattManager.mpInflictDamageToMatt(10.0f, Vector3.zero, 0);
				transform.root.FindChild("Camera").GetComponent<PerlinShake>().mpInitShake(0.5f, 600.0f, 3.0f);
			}
			else
			{
				if (!aAttackButtonIsDown)
				{	
					aStateBeforeAttacking		=	aMattManager.aCurrentState;
					aMattManager.aCurrentState	=	eMattState.ATTACKING;
					StartCoroutine(mcAttack());
				}
			}
		}
	}
}
