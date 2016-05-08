using UnityEngine;
using System.Collections;

public class SpawnJump : MonoBehaviour 
{
	//physics variables
	public	float	aSpeed;
	public	float	aWaveHeight;

	private	float	aAngle;
 	private	Vector3	aInitialPosition;

	void Start () 
	{
		StartCoroutine("spawnJump");
	}

	IEnumerator spawnJump()
	{
		//disable collider so pickup can't be grabbed in midair
		GetComponent<BoxCollider>().enabled	=	false;
		//cache initial position and init angle
		aInitialPosition	=	transform.position;
		aAngle				=	0.0f;

		//move until object has not returned to initial Y value
		while (aAngle < 180.0f)
		{
			//make spawnable object move up and down in local space
			transform.position = aInitialPosition + Vector3.up * aWaveHeight * Mathf.Sin(aAngle * Mathf.Deg2Rad);
			aAngle += aSpeed * Time.deltaTime;
			yield return null;
		}

		//enable collider so the pickup can be grabbed by Matt
		GetComponent<BoxCollider>().enabled	=	true;

		//stop translation and fix position
		transform.position	=	aInitialPosition;
	}
}
