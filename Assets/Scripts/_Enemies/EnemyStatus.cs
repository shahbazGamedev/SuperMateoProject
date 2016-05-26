using UnityEngine;
using System.Collections;

public class EnemyStatus : EnemyBehaviour 
{
	public		float	aMaxHP;
	protected	float	aCurrentHP;

	public		float	aHitForce;
	public		int		aStrength;

	public		float	aStunTime;
	public		bool	aIsStunned;
	public		bool	aIsDefeated;

	public void mpInitStatus()
	{
		aCurrentHP	=	aMaxHP;

		aIsDefeated	=	false;
		aIsStunned	=	false;
	}

	public bool mfInflictDamage(float pDamage, Vector3 pHitDirection)
	{
		if (aIsDefeated)
		{
			aCurrentAIState	=	eEnemyAIState.DIE;
			return false;
		}
		{
			aCurrentHP = Mathf.Clamp(aCurrentHP - pDamage, 0, aMaxHP);

			if (aCurrentHP <= 0)
			{
				aIsDefeated		=	true;
				aCurrentAIState	=	eEnemyAIState.DIE;
				aAudioSource.PlayOneShot(aDieSFX);

				StartCoroutine(mcDestroyEnemy());
			}
			else
			{
				aIsStunned		=	true;
				aCurrentAIState	=	eEnemyAIState.HIT;
				aAudioSource.PlayOneShot(aHitSFX);

				rgbody.AddForce(pHitDirection);

				StopAllCoroutines();
				StartCoroutine(mcStunEnemy());
			}
			return true;
		}
	}

	IEnumerator mcDestroyEnemy()
	{
		yield return new WaitForSeconds(2.5f);
		Instantiate(aDefeatPuff, transform.position, transform.rotation);

		if (transform.parent == null)	
		{
			Destroy(gameObject, 1.0f);
		}
		else
		{
			Destroy(transform.parent.gameObject, 1.0f);
		}
	}

	IEnumerator	mcStunEnemy()
	{
		yield return new WaitForSeconds(aStunTime);
		aIsStunned		=	false;
		aCurrentAIState	=	eEnemyAIState.CHASING;
	}

}
