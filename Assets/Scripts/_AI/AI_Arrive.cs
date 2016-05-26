using UnityEngine;
using System.Collections;

public class AI_Arrive : MonoBehaviour
{
	public	bool	aWalkOnly;

	public	float	aMaxAcceleration;

	public	float	aMaxSpeed;	
	private	float	aCurrentSpeed;

	public	float	aTargetRadius;
	public	float	aSlowRadius;

	private	float	aDistance;

	private	Vector3	aGoalVelocity;
	private	Vector3	aSteeringVector;

	private	EnemyManager	aEnemyManager;
	private	Miedo			aMiedoRef;

	void Start()
	{
		aEnemyManager	=	GetComponent<EnemyManager>();
		aMiedoRef		=	aEnemyManager.aTarget.GetComponent<Miedo>();
	}

	public void mpUpdateArriveAI()
	{
		if (aEnemyManager.aIsDefeated)
		{
			aEnemyManager.aCurrentAIState	=	eEnemyAIState.DIE;
		}
		else
		{
			if (!aEnemyManager.aIsStunned)
			{
				//if mateo goes STEALTH when chasing, then let him go return to WANDER.
				if (aMiedoRef)
				{
					if (aMiedoRef.enabled)
					{
						if (aMiedoRef.aMiedoState == eMiedoPhase.STEALTH)
						{
							aEnemyManager.aCurrentAIState	=	eEnemyAIState.WANDER;
							return;
						}
					}
				}

				if (!aEnemyManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)"))
				{
					aEnemyManager.rotationHelper.mpRotateViewVector((aEnemyManager.aTarget.transform.position - transform.position).normalized);
					aEnemyManager.rgbody.velocity	+=	mfGetArriveSteering(aEnemyManager.aTarget.transform.position);
				}
			}
		}
	}

	private Vector3 mfGetArriveSteering(Vector3 pTargetPosition)
	{
		aGoalVelocity	=	pTargetPosition - transform.position;
		aDistance		=	aGoalVelocity.magnitude;

		//check if we are there, return no direction
		if (aDistance < aTargetRadius)
		{
			aEnemyManager.aCurrentAIState	=	eEnemyAIState.ATTACK;
			return Vector3.zero;
		}
		else if (aDistance > aSlowRadius)
		{
			if (aWalkOnly)
				aEnemyManager.aCurrentAIState	=	eEnemyAIState.APPROACHING;
			else
				aEnemyManager.aCurrentAIState	=	eEnemyAIState.CHASING;

			aCurrentSpeed						=	aMaxSpeed;
		}
		else
		{
			aEnemyManager.aCurrentAIState	=	eEnemyAIState.APPROACHING;
			aCurrentSpeed					=	aMaxSpeed * (aDistance / aSlowRadius);
		}

		aGoalVelocity		=	aGoalVelocity.normalized * aCurrentSpeed;

		aSteeringVector 	=	aGoalVelocity - aEnemyManager.rgbody.velocity;
		aSteeringVector.y	=	0.0f;

		if (aSteeringVector.magnitude > aMaxAcceleration)
		{
			aSteeringVector	=	aSteeringVector.normalized * aMaxAcceleration;
		}

		return aSteeringVector;
	}

	public float distance
	{
		get { return aDistance;}
		set { aDistance	=	value;}
	}
}
