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
					mpToggleEmotion(true);
					break;
				}
			}
		}
	}

	private void mpToggleEmotion(bool pValue)
	{
		switch (aDominantEmotion)
		{
		case eMatea.MIEDO: 
			aMiedoRef.enabled		=	pValue;
			break;
		case eMatea.ALEGRIA: 
			aAlegriaRef.enabled		=	pValue;
			break;
		case eMatea.TRISTEZA: 
			aTristezaRef.enabled	=	pValue;
			break;
		case eMatea.ENOJO:
			aEnojoRef.enabled		=	pValue;
			break;
		}
	}

	public void mpResetMatea()
	{
		mpToggleEmotion(false);

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
				aCurrentMATEA[2]		+=	5;
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
				aDominantEmotion	=	eMatea.MIEDO;
				mpToggleEmotion(true);
			}
			else if (Input.GetButtonDown("padRight"))
			{
				aDominantEmotion	=	eMatea.ALEGRIA;
				mpToggleEmotion(true);
			}
			else if (Input.GetButtonDown("padDown"))
			{
				aDominantEmotion	=	eMatea.TRISTEZA;
				mpToggleEmotion(true);
			}
			else if (Input.GetButtonDown("padLeft"))
			{
				aDominantEmotion	=	eMatea.ENOJO;
				mpToggleEmotion(true);
			}
		}
		else if (Input.GetAxisRaw("leftTrigger") > 0)
		{
			mpResetMatea();
		}
	}
}
