
using UnityEngine;
using System.Collections;

public class AI_Wander : MonoBehaviour 
{
	[Range(3.0f, 20.0f)]
	public	float	aTargetCircleRadius;

	private	Vector3	aTargetCirclePivot;
	private	Vector3	aTargetPosition;
	private	float	aCurrentAngle;

	public	float	aMaxAcceleration;

	public	float	aMaxSpeed;	
	private	float	aCurrentSpeed;

	public	float	aTargetRadius;
	public	float	aSlowRadius;

	private	float	aDistance;

	private	Vector3	aGoalVelocity;
	private	Vector3	aSteeringVector;

	private	EnemyManager	aEnemyManager;

	void Start()
	{
		aEnemyManager	=	GetComponent<EnemyManager>();
		mpResetCircle();
	}

	public void mpExecute()
	{
		aEnemyManager.mpUpdateRightVector(aTargetPosition);
		aEnemyManager.rgbody.velocity	+=	mfGetWanderSteering();

		aEnemyManager.rotationHelper.mpRotateViewVector((aTargetPosition - transform.position).normalized);
	}

	public void mpResetCircle()
	{
		aTargetCirclePivot	=	transform.position;
		mpCalculateNewPositionOnCircle();
	}

	void mpCalculateNewPositionOnCircle()
	{
		aCurrentAngle	=	Random.Range(0, 360) * Mathf.Deg2Rad;
		aTargetPosition	=	aTargetCirclePivot + new Vector3(2.0f * Mathf.Cos(aCurrentAngle), 0, Mathf.Sin(aCurrentAngle)) * aTargetCircleRadius;
	}

	private Vector3 mfGetWanderSteering()
	{
		aGoalVelocity	=	aTargetPosition - transform.position;
		aDistance		=	aGoalVelocity.magnitude;

		//check if we are there, return no direction
		if (aDistance < aTargetRadius)
		{
			mpCalculateNewPositionOnCircle();
			return Vector3.zero;
		}

		//if we are outside the slowRadius, then go max speed, else slow down
		if (aDistance > aSlowRadius)
		{
			aCurrentSpeed	=	aMaxSpeed;
		}
		else
		{
			aCurrentSpeed	=	aMaxSpeed * (aDistance / aSlowRadius);
		}

		//set direction
		aGoalVelocity		=	aGoalVelocity.normalized * aCurrentSpeed;

		//accelerate
		aSteeringVector 	=	aGoalVelocity - aEnemyManager.rgbody.velocity;
		aSteeringVector.y	=	0.0f;

		if (aSteeringVector.magnitude > aMaxAcceleration)
		{
			aSteeringVector	=	aSteeringVector.normalized * aMaxAcceleration;
		}

		aEnemyManager.aCurrentAIState	=	eEnemyAIState.WANDER;

		return aSteeringVector;
	}
}
