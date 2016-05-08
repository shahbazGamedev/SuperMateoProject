using UnityEngine;
using System.Collections;

public class EnemyStatus : EnemyBehaviour 
{
	public		int		aMaxHP;
	protected	int		aHP;

	public		float	aHitForce;
	public		int		aStrength;

	protected	bool	aIsDefeated;
	public		bool	aIsStunned;

	public void mpInitStatus()
	{
		aHP			=	aMaxHP;

		aIsDefeated	=	false;
		aIsStunned	=	false;
	}

	public void mpInflictDamage(int pDamage, Vector3 pHitDirection)
	{
		aHP = Mathf.Clamp(aHP - pDamage, 0, aMaxHP);

		if (aHP <= 0)
		{
			aIsDefeated	=	true;

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
		else
		{
			rgbody.AddForce(pHitDirection);

			StartCoroutine(mcStunForSeconds());
		}
	}

	IEnumerator	mcStunForSeconds()
	{
		aIsStunned	=	true;

		yield return new WaitForSeconds(aStunTime);

		aIsStunned	=	false;
	}

}
