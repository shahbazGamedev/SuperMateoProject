using UnityEngine;
using System.Collections;

public class MattMATEA : MattPhysics 
{
	public	GameObject[]	aEmotionObjects;

	//highest assignable value for an emotion
	private int	aMaximumValue = 100;

	//current MATEA values
	private int[] 	aCurrentMATEA;
	private	eMatea	aDominantEmotion;

	private	float	aTristezaIncreaseRate;
	private	float	aNextTristezaIncrease;

	private	Miedo		aMiedoRef;
	private	Alegria		aAlegriaRef;
	private	Tristeza	aTristezaRef;
	private	Enojo		aEnojoRef;

	public void mpInitMatea()
	{
		if (aCurrentMATEA == null)
		{
			aCurrentMATEA 		= 	new int[5];
			aCurrentMATEA[0]	=	0;
			aCurrentMATEA[1] 	=	0;
			aCurrentMATEA[2] 	=	0;
			aCurrentMATEA[3] 	=	0;
			aCurrentMATEA[4] 	=	0;

			aDominantEmotion	=	eMatea.NORMAL;
		}

		aMiedoRef		=	GetComponent<Miedo>();
		aAlegriaRef		=	GetComponent<Alegria>();
		aTristezaRef	=	GetComponent<Tristeza>();
		aEnojoRef		=	GetComponent<Enojo>();

		aTristezaIncreaseRate	=	3.5f;
		aNextTristezaIncrease	=	aTristezaIncreaseRate;
	}

	private void mpCalculateDominantEmotion()
	{
		if (mfMattIsNormal())
		{
			for (int i = 0; i < 5; i++)
			{
				if (aCurrentMATEA[i] >= aMaximumValue)
				{
					aDominantEmotion	=	(eMatea)i;
					mpEnableEmotion();
					break;
				}
			}
		}
	}

	public void mpDisableEmotions()
	{
		aMiedoRef.enabled		=	false;
		aAlegriaRef.enabled		=	false;
		aTristezaRef.enabled	=	false;
		aEnojoRef.enabled		=	false;
	}

	private void mpEnableEmotion()
	{
		switch (aDominantEmotion)
		{
		case eMatea.MIEDO: 
			aMiedoRef.enabled		=	true;
			break;
		case eMatea.ALEGRIA: 
			aAlegriaRef.enabled		=	true;
			break;
		case eMatea.TRISTEZA: 
			aTristezaRef.enabled	=	true;
			break;
		case eMatea.ENOJO:
			aEnojoRef.enabled		=	true;
			break;
		}
	}

	public void mpResetMatea()
	{
		mpDisableEmotions();

		if (aDominantEmotion != eMatea.NORMAL)
		{
			aCurrentMATEA[(int)aDominantEmotion]	=	0;
			aDominantEmotion						=	eMatea.NORMAL;
		}
	}

	public bool mfMattIsNormal()
	{
		return (aDominantEmotion == eMatea.NORMAL);
	}

	//GET access function
	public int mfGetValue(eMatea pEmotion)
	{
		return aCurrentMATEA[(int)pEmotion];
	}

	public int mfIncreaseEmotionByValue(eMatea pModifiedEmotion, int pValue)
	{
		if (mfMattIsNormal())
		{
			if (aCurrentMATEA[(int)pModifiedEmotion] > 0)
			{
				if (Utilities.mfExecuteRNG(50))
					pValue	*=	-1;
			}

			//keep value within set interval
			aCurrentMATEA[(int)pModifiedEmotion] = Mathf.Clamp(aCurrentMATEA[(int)pModifiedEmotion] + pValue, 0, aMaximumValue);
			mpCalculateDominantEmotion();

			return pValue;
		}

		return 0;
	}

	protected void mpUpdateMatea()
	{
		if (aCurrentState == eMattState.IDLE && aDominantEmotion != eMatea.TRISTEZA)
		{
			if (Time.time > aNextTristezaIncrease)
			{
				aNextTristezaIncrease	=	Time.time + aTristezaIncreaseRate;
				aCurrentMATEA[(int)eMatea.TRISTEZA] = Mathf.Clamp(aCurrentMATEA[(int)eMatea.TRISTEZA] + 4, 0, aMaximumValue);
				mpCalculateDominantEmotion();
			}
		}
		else
		{
			aNextTristezaIncrease	=	Time.time + aTristezaIncreaseRate;
		}

		if (Input.GetAxisRaw("rightTrigger") > 0)
		{
			if (Input.GetButtonDown("padUp"))
			{
				mpDisableEmotions();
				aDominantEmotion	=	eMatea.MIEDO;
				aMiedoRef.enabled 	=	true;
			}
			else if (Input.GetButtonDown("padRight"))
			{
				mpDisableEmotions();
				aDominantEmotion	=	eMatea.ALEGRIA;
				aAlegriaRef.enabled =	true;
			}
			else if (Input.GetButtonDown("padDown"))
			{
				mpDisableEmotions();
				aDominantEmotion		=	eMatea.TRISTEZA;
				aTristezaRef.enabled 	=	true;
			}
			else if (Input.GetButtonDown("padLeft"))
			{
				mpDisableEmotions();
				aDominantEmotion	=	eMatea.ENOJO;
				aEnojoRef.enabled 	=	true;
			}
		}
		else if (Input.GetAxisRaw("leftTrigger") > 0)
		{
			mpResetMatea();
		}
	}
}
