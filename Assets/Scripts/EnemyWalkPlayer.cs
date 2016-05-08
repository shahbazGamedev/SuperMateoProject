using UnityEngine;
using System.Collections;

public class EnemyWalkPlayer : MonoBehaviour 
{
	private	float	aNextStep;
	public	float	aWalkRate;

	public	AudioClip[]	aStepSFXs;
	private	int			aTotalClips;
	private	int			aCurrentClip;

	private	EnemyManager	aEnemyManager;

	void Start () 
	{
		aCurrentClip	=	0;
		aTotalClips		=	aStepSFXs.Length;

		aEnemyManager	=	GetComponent<EnemyManager>();
	}

	void Update ()
    {
        if (Time.time > aNextStep)
        {
			switch (aEnemyManager.aCurrentAnimState)
			{
			case eEnemyAnimState.RUNNING:
            	aNextStep = Time.time + aWalkRate * 0.7f;
				break;
			case eEnemyAnimState.WALKING: 
            	aNextStep = Time.time + aWalkRate;
				break;
			case eEnemyAnimState.WALKING_RIGHT: case eEnemyAnimState.WALKING_LEFT:
            	aNextStep = Time.time + aWalkRate * 0.68f;
				break;
			default:
				return;
			}

			aEnemyManager.audioSource.PlayOneShot(aStepSFXs[aCurrentClip]);
			aCurrentClip	=	++aCurrentClip % aTotalClips;
		}
	}
}
