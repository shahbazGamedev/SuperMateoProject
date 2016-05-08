﻿using UnityEngine;
using System.Collections;

public class AI_FixedPath : MonoBehaviour 
{
	public	Transform[]		aPointCollection;

	private	int				aDirection;
	private	int				aTotalPoints;
	private	int				aCurrentPoint;

	private	AI_Arrive		aArriveAI;
	private	EnemyManager	aEnemyManager;

	void Start()
	{
		aArriveAI		=	GetComponent<AI_Arrive>();
		aEnemyManager	=	GetComponent<EnemyManager>();

		aDirection		=	1;
		aCurrentPoint	=	0;
		aTotalPoints	=	aPointCollection.Length;

		aEnemyManager.aTarget	=	aPointCollection[aCurrentPoint].gameObject;	

		Vector3	lTempPosition;

		for (int i = 0; i < aTotalPoints; i++)
		{
			lTempPosition					= 	aPointCollection[i].position;
			lTempPosition.y					=	transform.position.y;
			aPointCollection[i].position	=	lTempPosition;
		}

		aEnemyManager.aCurrentAIState	=	eEnemyAIState.CHASING;
	}

	void FixedUpdate () 
	{
		mpProcessAI();
		mpProcessAnimations();
	}

	void mpProcessAI()
	{
		switch (aEnemyManager.aCurrentAIState)
		{
		case eEnemyAIState.CHASING: 
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
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.RUNNING;
			}

			break;

		case eEnemyAIState.APPROACHING:
			aArriveAI.mpExecute();
				aEnemyManager.aCurrentAnimState	=	eEnemyAnimState.WALKING;
			break;

		case eEnemyAIState.ATTACK:
			if ((aCurrentPoint + aDirection) >= aTotalPoints)
			{
				aDirection	=	-1;
			}
			else if ((aCurrentPoint + aDirection) < 0)
			{
				aDirection	=	1;
			}

			aCurrentPoint	+=	aDirection;

			aEnemyManager.aTarget			=	aPointCollection[aCurrentPoint].gameObject;	
			aEnemyManager.aCurrentAIState	=	eEnemyAIState.APPROACHING;		
			break;

		case eEnemyAIState.DIE:	default: 
			break;
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
			aEnemyManager.animator.SetInteger("State", 1); 
			break;
		case eEnemyAnimState.RUNNING: 		
			aEnemyManager.animator.SetInteger("State", 2); 
			break;
		case eEnemyAnimState.ATTACKING:		
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