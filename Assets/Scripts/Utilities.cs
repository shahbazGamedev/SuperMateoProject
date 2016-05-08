using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour 
{
	static	public	GameObject	aMattCamera;
	private	const	float		GRAVITY_FACTOR = 18.0f;

	void Awake()
	{
		//set increased gravity for faster falling
		Physics.gravity =	Vector3.down * GRAVITY_FACTOR;
		aMattCamera		= 	GameObject.FindGameObjectWithTag("MainCamera");
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
}
