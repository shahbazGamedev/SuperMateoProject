using UnityEngine;
using System.Collections;

public class cStatus 
{
	public	float	aBase;
	public	float	aCurrent;

	public cStatus(float pBase)
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
	public	eMatea	aBiorhythm;

	//Matt HP variables
	protected	cStatus	aStatusHP;

	protected	cStatus	aStatusStrength;
	protected	cStatus	aStatusDefense;
	protected	cStatus	aStatusSpeed;
	protected	cStatus	aStatusAcceleration;
	protected	cStatus	aStatusHitResistance;

	private		float	aDealtDamage;

	//sets base values and inits with these inputs
	public void mpInitStatus(float pHP, float pStrength, float pDefense, float pSpeed, float pAcceleration)
	{
		aStatusHP		=	new cStatus(pHP);

		aStatusStrength			=	new cStatus(pStrength);
		aStatusDefense			=	new cStatus(pDefense);
		aStatusSpeed			=	new cStatus(pSpeed);
		aStatusAcceleration		=	new cStatus(pAcceleration);
		aStatusHitResistance	=	new cStatus(1.0f);

		aBiorhythm		=	eMatea.NORMAL;
	}

	public void mpInflictDamageToMatt(float pDamage, Vector3 pHitDirection, int pDirectHit=1)
	{
		//calculate dealt damage
		aDealtDamage 	=	(pDamage - aStatusDefense.aCurrent * pDirectHit);

		pHitDirection	*=	aStatusHitResistance.aCurrent;

		//validation: avoid "HP recover" when substracting negative values.
		if (aDealtDamage > 0)
		{
			//inflict damage
			aStatusHP.aCurrent = Mathf.Clamp(aStatusHP.aCurrent - aDealtDamage, 0, aStatusHP.aBase);
			aAudioSource.PlayOneShot(aDamageSFX);

			if (aStatusHP.aCurrent <= 0)
			{
				aCurrentState	=	eMattState.DEATH;
				transform.root.FindChild("Camera").GetComponent<MattCamera>().mpLerpGameOver();
				Utilities.mpGameOver();
			}

			aRgbody.AddForce(pHitDirection);
		}
	}

	public void mpEnableMultipliers(float pStrength, float pSpeed, float pDefense)
	{
		aStatusStrength.aCurrent		=	aStatusStrength.aBase * pStrength;
		aStatusDefense.aCurrent			=	aStatusDefense.aBase * pDefense;
		aStatusSpeed.aCurrent			=	aStatusSpeed.aBase * pSpeed;
	}

	public void mpDisableMultipliers()
	{
		aStatusStrength.aCurrent	=	aStatusStrength.aBase;
		aStatusDefense.aCurrent		=	aStatusDefense.aBase;
		aStatusSpeed.aCurrent		=	aStatusSpeed.aBase;
	}

	public void mpMultiplyAcceleration(float pAcceleration, float pHitResistance)
	{
		aStatusAcceleration.aCurrent	=	aStatusAcceleration.aBase * pAcceleration;
		aStatusHitResistance.aCurrent	=	aStatusHitResistance.aBase * pHitResistance;
	}

	public void mpRestoreAcceleration()
	{
		aStatusAcceleration.aCurrent	=	aStatusAcceleration.aBase;
		aStatusHitResistance.aCurrent	=	aStatusHitResistance.aBase;
	}

	public void mpRecoverHPByAmount(int pAmount)
	{
		aStatusHP.aCurrent = Mathf.Clamp(aStatusHP.aCurrent + pAmount, 0, aStatusHP.aBase);
	}

	public float currentHP
	{
		get{ return aStatusHP.aCurrent;}
	}
	public float currentDefense
	{
		get{ return aStatusDefense.aCurrent;}
		set{ aStatusDefense.aCurrent = value;}
	}
	public float currentSpeed
	{
		get{ return aStatusSpeed.aCurrent;}
		set{ aStatusSpeed.aCurrent = value;}
	}
	public float currentStrength
	{
		get{ return aStatusStrength.aCurrent;}
		set{ aStatusStrength.aCurrent = value;}
	}
}
