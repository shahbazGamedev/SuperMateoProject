using UnityEngine;
using System.Collections;

public class WalkCyclePlayer : MonoBehaviour 
{
	private	float	aNextStep;
	public	float	aWalkRate;

	public	AudioClip[]	aStepSFXs;
	private	int			aTotalClips;
	private	int			aCurrentClip;

	private	MattManager	aMattManager;

	void Start () 
	{
		aCurrentClip	=	0;
		aTotalClips		=	aStepSFXs.Length;

		aMattManager	=	GetComponent<MattManager>();
	}

	void Update ()
    {
    	if (aMattManager.aCurrentState == eMattState.RUNNING)
    	{
	        if (Time.time > aNextStep)
			{
	            aNextStep = Time.time + aWalkRate;

				aMattManager.aAudioSource.PlayOneShot(aStepSFXs[aCurrentClip]);
				aCurrentClip	=	++aCurrentClip % aTotalClips;
			}
    	}

	}
}
