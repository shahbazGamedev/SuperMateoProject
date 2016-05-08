using UnityEngine;
using System.Collections;

public class AI_Seek : MonoBehaviour 
{	
	public	float	aMaxAcceleration;

	private	Vector3	aSteeringVector;

	public Vector3 mfGetSeekSteering(Vector3 pTargetPosition)
	{
		aSteeringVector	=	pTargetPosition - transform.position;

		aSteeringVector	=	aSteeringVector.normalized * aMaxAcceleration;

		Debug.DrawRay(transform.position, aSteeringVector);

		return aSteeringVector;
	}
}
