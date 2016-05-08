using UnityEngine;
using System.Collections;

public class AI_Creeper : MonoBehaviour 
{
	public bool 	aResetCircleAfterSeek;

	private	EnemyManager	aEnemyManager;

	private	AI_Arrive		aArriveAI;
	private	AI_Wander		aWanderAI;

	public	float			aAttackRate;
	private	float			aNextAttackSound;

	void Start()
	{
		aEnemyManager	=	GetComponent<EnemyManager>();
		aArriveAI		=	GetComponent<AI_Arrive>();
		aWanderAI		=	GetComponent<AI_Wander>();
	}

	void FixedUpdate()
	{
		mpProcessAI();
		mpProcessAnimations();
	}

	void mpProcessAI()
	{
		switch (aEnemyManager.aCurrentAIState)
		{
		case eEnemyAIState.WANDER:
			//walk around circle
			aWanderAI.mpExecute();
			if (aEnemyManager.rightDotValue > 0.2)
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING_RIGHT;
			}
			else if (aEnemyManager.rightDotValue < -0.2)
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING_LEFT;
			}
			else
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING;
			}
			break;

		case eEnemyAIState.CHASING:
			aArriveAI.mpExecute();

			if (aArriveAI.distance >= 20.0f)
			{
				aArriveAI.distance				=	0.0f;
				aEnemyManager.aCurrentAIState	=	eEnemyAIState.WANDER;

				if (aResetCircleAfterSeek)
					aWanderAI.mpResetCircle();
			}
			else
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.RUNNING;
			}
			break;

		case eEnemyAIState.APPROACHING:
			aArriveAI.mpExecute();

			aEnemyManager.mpUpdateRightVector(aEnemyManager.aTarget.transform.position);

			if (aEnemyManager.rightDotValue > 0.5f)
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING_RIGHT;
			}
			else if (aEnemyManager.rightDotValue < -0.5f)
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING_LEFT;
			}
			else
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING;
			}
			break;

		case eEnemyAIState.ATTACK:
			aArriveAI.mpExecute();

			if (Vector3.Dot(transform.forward, (aEnemyManager.aTarget.transform.position - transform.position).normalized) > aEnemyManager.aAttackFrontDotValue)
			{
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.ATTACKING;
			}
			else
			{
				aEnemyManager.mpUpdateRightVector(aEnemyManager.aTarget.transform.position);

				if (aEnemyManager.rightDotValue > 0.8f)
				{
					aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING_RIGHT;
				}
				else if (aEnemyManager.rightDotValue < -0.8f)
				{
					aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING_LEFT;
				}
			}
			break;

		case eEnemyAIState.DIE:	 default:	
			aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.DIE;
			break;
		}
	}

	void mpPlayAttackSFX()
	{
		if (aEnemyManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)"))
		{
			if (Time.time > aNextAttackSound)
			{
				aNextAttackSound	=	Time.time + aAttackRate;
				aEnemyManager.audioSource.PlayOneShot(aEnemyManager.aAttackSFX, 0.8f);
			}
		}
	}

	void mpProcessAnimations()
	{
		switch (aEnemyManager.aCurrentAnimState)
		{
		case eEnemyAnimState.IDLE: default:	
			aEnemyManager.animator.SetInteger("State", 0); 
			break;
		case eEnemyAnimState.WALKING: 		
			mpPlayAttackSFX();
			aEnemyManager.animator.SetInteger("State", 1); 
			break;
		case eEnemyAnimState.RUNNING: 		
			aEnemyManager.animator.SetInteger("State", 2); 
			break;
		case eEnemyAnimState.ATTACKING:		
			mpPlayAttackSFX();
			aEnemyManager.animator.SetInteger("State", 3); 
			break;
		case eEnemyAnimState.HIT: 			
			aEnemyManager.animator.SetInteger("State", 4); 
			break;
		case eEnemyAnimState.DIE:			
			aEnemyManager.animator.SetInteger("State", 5); 
			break;
		case eEnemyAnimState.WALKING_RIGHT: 	
			aEnemyManager.animator.SetInteger("State", 6); 
			break;
		case eEnemyAnimState.WALKING_LEFT: 	
			aEnemyManager.animator.SetInteger("State", 7); 
			break;
		}
	}
}
