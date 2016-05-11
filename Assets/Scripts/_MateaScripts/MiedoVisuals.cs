using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MiedoVisuals : MonoBehaviour 
{
	//condition length
	public	GameObject	aMiedoAmbience;
	private	GameObject	aMiedoAmbienceReference;

	//increase rate of filters properties
	private	const float		ACCELERATION 			= 	0.50f;
	private	const float		MAX_INTENSITY_OVERLAY	=	0.2f;//0.75f;	
	private	const float		MAX_BLUR_AMOUNT			=	0.5f;//0.80f;	
	private	const float		MAX_INTENSITY_BW		=	1.00f;	

	//filter scripts used in this build up
	private	MotionBlur		aMotionBlur;
	private	ScreenOverlay	aScreenOverlay;
	private	FadeOutColor	aBlackWhite;

	void Start()
	{
		aMotionBlur				=	GetComponent<MotionBlur>();
		aMotionBlur.enabled		=	true;
		aMotionBlur.blurAmount	=	0.0f;

		aScreenOverlay			=	GetComponent<ScreenOverlay>();
		aScreenOverlay.enabled	=	true;
		aScreenOverlay.intensity=	0.0f;

		aBlackWhite				=	GetComponent<FadeOutColor>();
		aBlackWhite.enabled 	=	true;
		aBlackWhite.intensity	=	0.0f;

		aMiedoAmbienceReference	=	(GameObject)Instantiate(aMiedoAmbience, transform.position, transform.rotation);

		StartCoroutine(mcLerpUp());
	}

	IEnumerator mcLerpUp()
	{
		while (aBlackWhite.intensity < MAX_INTENSITY_BW)
		{
			aScreenOverlay.intensity	= Utilities.mfApproach(MAX_INTENSITY_OVERLAY, aScreenOverlay.intensity,	ACCELERATION * 0.75f * Time.deltaTime);
			aMotionBlur.blurAmount		= Utilities.mfApproach(MAX_BLUR_AMOUNT, 	  aMotionBlur.blurAmount,	ACCELERATION * 1.8f * Time.deltaTime);
			aBlackWhite.intensity		= Utilities.mfApproach(MAX_INTENSITY_BW,	  aBlackWhite.intensity,    ACCELERATION * 1.5f * Time.deltaTime);

			yield return null;
		}
	}

	public void mpLerpDownMiedoVisuals()
	{
		StartCoroutine(mcLerpDown());
	}

	IEnumerator mcLerpDown()
	{

		while (aScreenOverlay.intensity > 0.0f)
		{
			aScreenOverlay.intensity	= Utilities.mfApproach(0.0f, aScreenOverlay.intensity,	ACCELERATION * 0.75f * Time.deltaTime);
			aMotionBlur.blurAmount		= Utilities.mfApproach(0.0f, aMotionBlur.blurAmount,   	ACCELERATION * Time.deltaTime);
			aBlackWhite.intensity		= Utilities.mfApproach(0.0f, aBlackWhite.intensity,		ACCELERATION * Time.deltaTime);
			yield return null;
		}

		//disable effects and destroy this script
		aMotionBlur.enabled		=	false;
		aScreenOverlay.enabled	=	false;
		aBlackWhite.enabled		=	false;

		transform.root.Find("Character").GetComponent<MattManager>().mpResetMatea();
		aMiedoAmbienceReference.GetComponent<AudioFadeInOut>().mpFadeOutVolume();

		Destroy(gameObject);
	}
}
