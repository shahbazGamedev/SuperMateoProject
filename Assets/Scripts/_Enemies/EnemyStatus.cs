using UnityEngine;
using System.Collections;

public class EnemyStatus : EnemyBehaviour 
{
	public		float		aMaxHP;
	[SerializeField]
	protected	float		aCurrentHP;

	public		float	aHitForce;
	public		int		aStrength;

	protected	bool	aIsDefeated;

	public		float	aStunTime;
	public		bool	aIsStunned;

	public void mpInitStatus()
	{
		aCurrentHP	=	aMaxHP;

		aIsDefeated	=	false;
		aIsStunned	=	false;
	}

	public void mpInflictDamage(float pDamage, Vector3 pHitDirection)
	{
		aCurrentHP = Mathf.Clamp(aCurrentHP - pDamage, 0, aMaxHP);

		if (aCurrentHP <= 0)
		{
			aIsDefeated		=	true;
			mpDestroyEnemy();
			aCurrentAIState	=	eEnemyAIState.DIE;
			aAudioSource.PlayOneShot(aDieSFX);
			transform.root.FindChild("Vision").gameObject.SetActive(false);
		}
		else
		{
			aCurrentAIState	=	eEnemyAIState.HIT;
			aAudioSource.PlayOneShot(aHitSFX);
			rgbody.AddForce(pHitDirection);
		}
	}

	public void mpDestroyEnemy()
	{
		StartCoroutine(mcDestroyEnemy());
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

	public void mpStunEnemy(eEnemyAIState pPostStunState)
	{
		StartCoroutine(mcStunEnemy(pPostStunState));
	}

	IEnumerator	mcStunEnemy(eEnemyAIState pPostStunState)
	{
		aIsStunned	=	true;

		yield return new WaitForSeconds(aStunTime);

		aCurrentAIState		=	pPostStunState;
		aIsStunned			=	false;
	}

}
