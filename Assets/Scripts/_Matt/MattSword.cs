using UnityEngine;
using System.Collections;

public class MattSword : MonoBehaviour 
{
	public	GameObject	aSparks;
	public	float		aHitForce;

	public	AudioClip[]	aSwingClips;
	public	AudioClip	aHitSFX;
	private	int			aCurrentSwing;
	private	int			aTotalSwings;

	private	AudioSource	aAudioSource;

	private	MattManager	aMattManager;

	private	eMattState	aStateBeforeAttacking;

	public	float		aAttackRate;
	private	bool		aHitWasEffective;

	void Start()
	{
		aMattManager		=	transform.root.GetComponentInChildren<MattManager>();
		aAudioSource		=	GetComponent<AudioSource>();

		aTotalSwings		=	aSwingClips.Length;
		aCurrentSwing		=	0;
		aHitWasEffective	=	false;
	}

	void OnTriggerStay(Collider pOther)
	{
		if (pOther.tag == "Enemy")
		{
			if (!aHitWasEffective && !aMattManager.aMattCanAttack && !pOther.GetComponent<EnemyManager>().aIsDefeated)
			{
				aHitWasEffective		=	true;

				Vector3	lPushBackForce	=	(pOther.transform.position - transform.parent.position).normalized * aHitForce;
				lPushBackForce.y		=	0;

				if (pOther.GetComponent<EnemyManager>().mfInflictDamage(aMattManager.currentStrength, lPushBackForce))
				{
					Destroy(Instantiate(aSparks, pOther.transform.position, Quaternion.identity), 2.0f);
					aAudioSource.PlayOneShot(aHitSFX);
				}
				else
				{
					aMattManager.aPositiveStreak	+=	2;
				}
				//build up positive streak
				aMattManager.aPositiveStreak++;
			}
		}
	}

	IEnumerator mcAttack()
	{
		aStateBeforeAttacking		=	aMattManager.aCurrentState;
		aMattManager.aCurrentState	=	eMattState.ATTACKING;

		yield return null;

		aMattManager.aCurrentState	=	aStateBeforeAttacking;

		aAudioSource.PlayOneShot(aSwingClips[aCurrentSwing++]);
		aCurrentSwing	=	aCurrentSwing % aTotalSwings;

		yield return new WaitForSeconds(aAttackRate);

		aMattManager.aMattCanAttack	=	true;
		aHitWasEffective			=	false;
	}


	void Update()
	{
		if (aMattManager.aBiorhythm == eMatea.TRISTEZA)
		{
			if (Input.GetButtonDown("B"))
			{
				aMattManager.mpInflictDamageToMatt(10.0f, Vector3.zero, 0);
				transform.root.FindChild("Camera").GetComponent<PerlinShake>().mpInitShake(0.5f, 600.0f, 3.0f);
			}
		}
		else if (Input.GetButton("B"))
		{
			if (aMattManager.aCurrentState == eMattState.RUNNING || aMattManager.aCurrentState == eMattState.IDLE)
			{
				if (aMattManager.aMattCanAttack)
				{
					aMattManager.aMattCanAttack	=	false;
					StartCoroutine(mcAttack());
				}
			}
		}
	}
}
