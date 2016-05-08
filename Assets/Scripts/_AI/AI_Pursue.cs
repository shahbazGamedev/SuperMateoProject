using UnityEngine;
using System.Collections;


public class AI_Pursue : AI_Seek 
{
//	public	float	aMaxPrediction;
//	private	float	aPrediction;
//
//	private	Vector3	aDirection;
//	private	float	aDistance;
//
//	private	float	aCurrentSpeed;
//
//	protected Vector3 mfGetPursuitSteering(Vector3 pTargetPosition, Vector3 pTargetVelocity)
//	{
//		aDirection	=	pTargetPosition - transform.position;
//		aDistance	=	aDirection.magnitude;
//
//		aCurrentSpeed	=	aRgbody.velocity.magnitude;
//
//		if (aCurrentSpeed <= (aDistance / aMaxPrediction))
//		{
//			aPrediction	=	aMaxPrediction;
//		}
//		else
//		{
//			aPrediction	=	aDistance / aCurrentSpeed;
//		}
//
//		pTargetPosition	+=	pTargetVelocity * aPrediction;
//
//		return mfGetSeekSteering(pTargetPosition);
//	}
}
