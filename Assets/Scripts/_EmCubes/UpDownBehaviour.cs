using UnityEngine;
using System.Collections;

public class UpDownBehaviour : MonoBehaviour 
{
	public	float	aSpeed;
	public	float	aWaveHeight;

	private	float	aAngle;
 	private	Vector3	aInitialPosition;

	void Start () 
	{
		aInitialPosition	=	transform.position;
		StartCoroutine("moveUpAndDown");
	}

	IEnumerator moveUpAndDown()
	{
		aAngle	=	0.0f;

		while (aAngle < 360.0f)
		{
			transform.position = aInitialPosition + Vector3.up * aWaveHeight * Mathf.Sin(aAngle);
			aAngle += aSpeed * Time.deltaTime;
			yield return null;
		}

		StartCoroutine("moveUpAndDown");
	}
}
