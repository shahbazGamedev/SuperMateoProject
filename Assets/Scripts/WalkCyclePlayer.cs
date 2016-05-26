using UnityEngine;
using System.Collections;

public class WalkCyclePlayer : MonoBehaviour 
{
	private	float	aNextStep;
	public	float	aWalkRate;

	public	AudioClip[]	aStepSFXs;

	[SerializeField]
	private	int			aCurrentClip;
	[SerializeField]
	private	int			aCurrentStep;

	private	MattManager	aMattManager;
	public	eSurfaces	aCurrentSurface;

	void Start () 
	{
		aCurrentClip	=	0;
		aCurrentStep	=	0;

		aMattManager	=	GetComponent<MattManager>();
	}

	void Update ()
    {

    	if (aMattManager.aCurrentState == eMattState.RUNNING)
    	{
	        if (Time.time > aNextStep)
			{
	            aNextStep = Time.time + aWalkRate;

	            switch (aCurrentSurface)
				{
				case eSurfaces.GRASS: default:
					aCurrentClip	=	aCurrentStep % 4;
					break;
				case eSurfaces.STONE:
					aCurrentStep	=	aCurrentStep % 3;
					aCurrentClip	=	4 + aCurrentStep;

	            	break;
	            }
	            aCurrentStep	=	++aCurrentStep % 7;
				aMattManager.aAudioSource.PlayOneShot(aStepSFXs[aCurrentClip++]);
			}
    	}

	}
}
