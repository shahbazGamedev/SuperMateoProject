using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class EnojoVisuals : MonoBehaviour 
{
	private	ScreenOverlay	aOverlay;
	private	ContrastEnhance	aContrast;
	private	NoiseAndGrain	aGrain;

	private	const	float	MAX_INTENSITY_OV	=	1.0f;	
	private	const	float	ACCELERATION 		= 	1.5f;

	private	const	float	MAX_INTENSITY_CONT	=	1.0f;	

	private	const	float	MAX_INTENSITY_GRAIN	=	-7.0f;	

	void Start () 
	{	
		aOverlay			=	GetComponent<ScreenOverlay>();
		aContrast			=	GetComponent<ContrastEnhance>();
		aGrain				=	GetComponent<NoiseAndGrain>();

		aOverlay.enabled 	=	true;
		aOverlay.intensity	=	0.0f;

		aContrast.enabled	=	true;
		aContrast.intensity	=	0.0f;

		aGrain.enabled				=	true;
		aGrain.intensityMultiplier	=	0.0f;

		StartCoroutine(mcLerpUp());
	}

	IEnumerator mcLerpUp()
	{
		while ((aOverlay.intensity < MAX_INTENSITY_OV) || (aContrast.intensity < MAX_INTENSITY_CONT) || (aGrain.intensityMultiplier > MAX_INTENSITY_GRAIN))
		{
			aOverlay.intensity			=	Utilities.mfApproach(MAX_INTENSITY_OV, aOverlay.intensity, ACCELERATION * Time.deltaTime);
			aContrast.intensity			=	Utilities.mfApproach(MAX_INTENSITY_CONT, aContrast.intensity, ACCELERATION * Time.deltaTime);
			aGrain.intensityMultiplier	=	Utilities.mfApproach(MAX_INTENSITY_GRAIN, aGrain.intensityMultiplier, ACCELERATION * 3.0f * Time.deltaTime);
			yield return null;
		}
	}

	public void mpLerpDownAlegriaVisuals()
	{
		StartCoroutine(mcLerpDown());
	}

	IEnumerator mcLerpDown()
	{
		while ((aOverlay.intensity > 0.0f) || (aContrast.intensity > 0.0f) || (aGrain.intensityMultiplier < 0.0f))
		{
			aOverlay.intensity			=	Utilities.mfApproach(0.0f, aOverlay.intensity, ACCELERATION * Time.deltaTime);
			aContrast.intensity			=	Utilities.mfApproach(0.0f, aContrast.intensity, ACCELERATION * Time.deltaTime);
			aGrain.intensityMultiplier	=	Utilities.mfApproach(0.0f, aGrain.intensityMultiplier, ACCELERATION * 3.0f * Time.deltaTime);
			yield return null;
		}

		MattManager	lManager	=	transform.root.Find("Character").GetComponent<MattManager>();

		if (lManager.mfEmotionIsEqualToDominantEmotion(eMatea.ENOJO))
			transform.root.Find("Character").GetComponent<MattManager>().mpResetMatea();

		Destroy(gameObject);
	}
}
