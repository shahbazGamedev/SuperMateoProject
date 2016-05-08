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

	//target filter camera
	private	Transform	aMattCamera;

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
			aMattCamera			=	transform.parent.FindChild("Camera");
		}
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
					((GameObject)Instantiate(aEmotionObjects[(int)aDominantEmotion], aMattCamera.position, aMattCamera.rotation)).transform.SetParent(aMattCamera);
					break;
				}
			}
		}
	}

	public void mpResetMatea()
	{
		aCurrentMATEA[(int)aDominantEmotion]	=	0;
		aDominantEmotion	=	eMatea.NORMAL;
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
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			aCurrentMATEA[0] = Mathf.Clamp(aCurrentMATEA[0] + 100, 0, aMaximumValue);
			mpCalculateDominantEmotion();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			aCurrentMATEA[1] = Mathf.Clamp(aCurrentMATEA[1] + 100, 0, aMaximumValue);
			mpCalculateDominantEmotion();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			aCurrentMATEA[2] = Mathf.Clamp(aCurrentMATEA[2] + 100, 0, aMaximumValue);
			mpCalculateDominantEmotion();
		}
	}
}
