using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Utilities : MonoBehaviour 
{
	public	GameObject	aHPCanvas;
	public	GameObject	aMateaCanvas;
	public	GameObject	aStatusCanvas;
	public	GameObject	aNotifPanel;
	static	public	GameObject	aNotificationPanel;
	static	public	GameObject	aHP;
	static	public	GameObject	aMatea;
	static	public	GameObject	aStatus;

	private	const	float		GRAVITY_FACTOR = 18.0f;

	void Awake()
	{
		//set increased gravity for faster falling
		Physics.gravity 	=	Vector3.down * GRAVITY_FACTOR;
		aNotificationPanel	=	aNotifPanel;
		aHP					=	aHPCanvas;
		aMatea				=	aMateaCanvas;
		aStatus				=	aStatusCanvas;
	}

	public static void mpGameOver()
	{
		Utilities.aHP.SetActive(false);
		Utilities.aMatea.SetActive(false);
		Utilities.aStatus.SetActive(false);
	}

	//basic lerp function
	public static float mfApproach(float pTarget, float pCurrent, float dt)
	{
		if ((pTarget - pCurrent) > dt)
			return pCurrent + dt;
		if ((pTarget - pCurrent) < -dt)
			return pCurrent - dt;

		return pTarget;
	}

	public static eMatea mfGetRandomEmotion()
	{
		return (eMatea)Random.Range(0, 5);
	}

	//Generates a random number, then randomly returns as positive or negative
	public static int mfGetRandomSignedNumber(int pMinValue, int pMaxValue)
	{
		int lNumber = Random.Range(pMinValue, pMaxValue + 1);

		switch (Random.Range(0, 2))
		{
		case 0:		return  lNumber;
		case 1:		return -lNumber;
		default:	return 0;
		}
	}

	public static bool mfExecuteRNG(int pMaxValidValue)
	{
		return (Random.Range(1, 100) <= pMaxValidValue);
	}

	public static GameObject mfCreateEmotionObject(eMatea pBiorhythm, GameObject pEmotionObject, Transform pCameraObject)
	{
		GameObject	lResult	=	(GameObject)Instantiate(pEmotionObject, pCameraObject.position, pCameraObject.rotation);
		lResult.transform.SetParent(pCameraObject);

		return lResult;
	}
}
