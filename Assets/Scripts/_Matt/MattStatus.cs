using UnityEngine;
using System.Collections;

public class cStatus 
{
	public	int	aBase;
	public	int	aCurrent;

	public cStatus(int pBase)
	{
		aBase	=	pBase;
		mpResetToBaseValues();
	}

	public void mpResetToBaseValues()
	{
		aCurrent = aBase;
	}
}

public class MattStatus : MattBehaviour
{
	public	eStatus	aBiorhythm;

	//Matt HP variables
	protected	cStatus	aStatusHP;

	protected	cStatus	aStatusStrength;
	protected	cStatus	aStatusDefense;
	protected	cStatus	aStatusSpeed;

	//sets base values and inits with these inputs
	public void mpInitStatus(int pHP, int pStrength, int pDefense, int pSpeed)
	{
		aStatusHP		=	new cStatus(pHP);

		aStatusStrength	=	new cStatus(pStrength);
		aStatusDefense	=	new cStatus(pDefense);
		aStatusSpeed	=	new cStatus(pSpeed);

		aBiorhythm	=	eStatus.NORMAL;
	}

	public void mpInflictDamageToMatt(int pDamage, Vector3 pHitDirection)
	{
		//calculate dealt damage
		int	lDealtDamage = (pDamage - aStatusDefense.aCurrent);

		//validation: avoid "HP recover" when substracting negative values.
		if (lDealtDamage > 0)
		{
			//inflict damage
			aStatusHP.aCurrent = Mathf.Clamp(aStatusHP.aCurrent - lDealtDamage, 0, aStatusHP.aBase);

			if (aStatusHP.aCurrent <= 0)
			{
				aCurrentState	=	eMattState.DEATH;
				aRgbody.AddForce(pHitDirection);
			}
			else
			{
				aRgbody.AddForce(pHitDirection);
			}
		}
	}

	public void mpRecoverHPByAmount(int pAmount)
	{
		aStatusHP.aCurrent = Mathf.Clamp(aStatusHP.aCurrent + pAmount, 0, aStatusHP.aBase);
	}

	public float currentHP
	{
		get{ return aStatusHP.aCurrent;}
	}
	public int currentDefense
	{
		get{ return aStatusDefense.aCurrent;}
		set{ aStatusDefense.aCurrent = value;}
	}
	public int currentSpeed
	{
		get{ return aStatusSpeed.aCurrent;}
		set{ aStatusSpeed.aCurrent = value;}
	}
	public int currentStrength
	{
		get{ return aStatusStrength.aCurrent;}
		set{ aStatusStrength.aCurrent = value;}
	}
}
