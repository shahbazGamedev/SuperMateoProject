using UnityEngine;
using System.Collections;

public class PerlinShake : MonoBehaviour 
{
	//how long does the shaking last?
	private float aDuration;
	//how fast this camera should shake?
	private float aSpeed;
	//shaking abruptness
	private float aMagnitude;

	//cache variables
	float	aTimeElapsed;
	float	aRandomStart;
	float	aPercentComplete;
	float	aDampValue;
	float	aAlpha;
	Vector3	aPositionDelta;

	//start shaking by specifying input values
	public void mpInitShake(float pDuration, float pSpeed, float pMagnitude) 
	{
		aDuration 	= pDuration;
		aSpeed 		= pSpeed;
		aMagnitude 	= pMagnitude;
		StartCoroutine("Shake");
	}

	IEnumerator Shake() 
	{
		aTimeElapsed 	=	0.0f;
		aPositionDelta	=	Vector3.zero;
		aRandomStart	=	Random.Range(-1000.0f, 1000.0f);
		
		while (aTimeElapsed < aDuration)
		{
			aTimeElapsed	+=	Time.deltaTime;			
			
			aPercentComplete =	aTimeElapsed / aDuration;			

			aDampValue	=	1.0f - Mathf.Clamp(2.0f * aPercentComplete - 1.0f, 0.0f, 1.0f);

			aAlpha	=	aRandomStart + aSpeed * aPercentComplete;

			aPositionDelta.x	=	Mathf.PerlinNoise(aAlpha, 0.0f) * 2.0f - 1.0f;
			aPositionDelta.y	=	Mathf.PerlinNoise(0.0f, aAlpha) * 2.0f - 1.0f;

			aPositionDelta	*=	aMagnitude * aDampValue;

			transform.position	+=	aPositionDelta;
				
			yield return null;
		}
	}
}
