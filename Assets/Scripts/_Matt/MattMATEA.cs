using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MattMATEA : MattPhysics 
{
	private	Text			aNotificationText;
	public	GameObject[]	aEmotionObjects;

	//highest assignable value for an emotion
	private const int	aMaximumValue = 100;

	//current MATEA values
	private int[] 		aCurrentMATEA;
	private	eMatea		aDominantEmotion;

	private	float		aTristezaIncreaseRate;
	private	float		aNextTristezaIncrease;

	private	Miedo		aMiedoRef;
	private	Alegria		aAlegriaRef;
	private	Tristeza	aTristezaRef;
	private	Enojo		aEnojoRef;

	public void mpInitMatea()
	{
		aNotificationText	=	Utilities.aNotificationPanel.GetComponentInChildren<Text>();

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

	public bool mfEmotionIsEqualToDominantEmotion(eMatea pEmotion)
	{
		return (aDominantEmotion == pEmotion);
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

	public void mpApplyStreakEmotion(eMatea pEmotion, int pValue)
	{
		aCurrentMATEA[(int)pEmotion]	=	Mathf.Clamp(aCurrentMATEA[(int)pEmotion] + pValue, 0, aMaximumValue);

		if (aCurrentMATEA[(int)pEmotion] >= aMaximumValue)
		{
			mpResetMatea();
		}
		mpCalculateDominantEmotion();
	}

	protected void mpUpdateMatea()
	{
		if (mfMattIsNormal())
		{
			if ((aPositiveStreak % 2 == 0) && (aPositiveStreak > 0))
			{
				mpApplyStreakEmotion(eMatea.ALEGRIA, 20);
				aPositiveStreak = 0;
			}
			else if ((aNegativeStreak % 2 == 0) && (aNegativeStreak > 0))
			{
				mpApplyStreakEmotion(eMatea.TRISTEZA, 20);
				aNegativeStreak = 0;
			}
		}

		mpTristezaPeriodicIncrease();

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

	public void mpRespawnMatt()
	{
		mpDisableEmotions();
		aMattJustRespawned	=	true;
		mpInflictDamageToMatt(20.0f, Vector3.zero, 0);
	}

	private void mpTristezaPeriodicIncrease()
	{
		if (aCurrentState == eMattState.IDLE && aDominantEmotion != eMatea.TRISTEZA)
		{
			if (aMattCanMove)
			{
				if (Time.time > aNextTristezaIncrease)
				{
					aNextTristezaIncrease				=	Time.time + aTristezaIncreaseRate;
					aCurrentMATEA[(int)eMatea.TRISTEZA] =	Mathf.Clamp(aCurrentMATEA[(int)eMatea.TRISTEZA] + 4, 0, aMaximumValue);
					mpCalculateDominantEmotion();
				}
			}
		}
		else
		{
			aNextTristezaIncrease	=	Time.time + aTristezaIncreaseRate;
		}
	}

	/*
	 *
	 * EmCubes Logic
	 *
	 */
	IEnumerator mcToggleNotification()
	{
		Utilities.aNotificationPanel.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.8f);	
		Utilities.aNotificationPanel.gameObject.SetActive(false);
	}

	private string mfGetEmotionString(eMatea pEmotion)
	{
		switch (pEmotion)
		{
			case eMatea.MIEDO: 		return " Miedo";
			case eMatea.ALEGRIA: 	return " Alegría";
			case eMatea.TRISTEZA: 	return " Tristeza";
			case eMatea.ENOJO: 		return " Enojo";
			case eMatea.AMOR: 		return " Amor";
			default: 				return "null";
		}
	}

	public int mfEmCubeBonus(eMatea pRandomEmotion)
	{
		if (pRandomEmotion == aDominantEmotion)
		{
			pRandomEmotion	=	(eMatea)((int)eMatea.AMOR - (int)aDominantEmotion);
		}

		int lSignedResult	=	mfIncreaseEmotionByValue(pRandomEmotion, Random.Range(20, 50));

		if (lSignedResult > 0)
		{
			aNotificationText.text	=	"+" + lSignedResult.ToString() + mfGetEmotionString(pRandomEmotion);
		}
		else
		{
			aNotificationText.text	=	lSignedResult.ToString() + mfGetEmotionString(pRandomEmotion);
		}

		StartCoroutine(mcToggleNotification());

		return lSignedResult;
	}
}
