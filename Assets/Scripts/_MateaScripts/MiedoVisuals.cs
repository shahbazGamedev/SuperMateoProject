using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MiedoVisuals : MonoBehaviour 
{
	//condition length
	private	AudioSource		aAudioSource;
	public	AudioClip		aStealthAmbience;
	public	AudioClip		aDefensiveAmbience;

	//increase rate of filters properties
	private	const float		ACCELERATION 			= 	0.50f;
	private	const float		MAX_INTENSITY_OVERLAY	=	0.5f;	
	private	const float		MAX_INTENSITY_BW		=	1.00f;	

	//filter scripts used in this build up
	private	VignetteAndChromaticAberration	aScreenOverlay;
	private	NoiseAndScratches				aScratches;
	private	FadeOutColor					aBlackWhite;
	private	GlobalFog						aFog;

	void Start()
	{
		aScratches				=	GetComponent<NoiseAndScratches>();
		aScratches.enabled		=	true;
		aScratches.grainSize	=	0.1f;

		aScreenOverlay			=	GetComponent<VignetteAndChromaticAberration>();
		aScreenOverlay.enabled	=	true;
		aScreenOverlay.intensity=	0.0f;

		aBlackWhite				=	GetComponent<FadeOutColor>();
		aBlackWhite.enabled 	=	true;
		aBlackWhite.intensity	=	0.0f;

		aFog					=	GetComponent<GlobalFog>();
		aFog.enabled			=	true;
		aFog.height				=	-10.0f;

		aAudioSource		=	GetComponent<AudioSource>();

		aAudioSource.clip	=	aStealthAmbience;
		aAudioSource.Play();

		StartCoroutine(mcLerpUp());
	}

	public void mpChangeAmbienceToDefensive()
	{
		aAudioSource.Stop();
		aAudioSource.clip	=	aDefensiveAmbience;
		aAudioSource.Play();
		StartCoroutine(mcLerpUpDefensive());
	}

	IEnumerator mcLerpUp()
	{
		while (aBlackWhite.intensity < MAX_INTENSITY_BW || aScreenOverlay.intensity < MAX_INTENSITY_OVERLAY || aFog.height < 2.0f)
		{
			aScreenOverlay.intensity	=	Utilities.mfApproach(MAX_INTENSITY_OVERLAY, aScreenOverlay.intensity,	ACCELERATION * 0.75f * Time.deltaTime);
			aScreenOverlay.blur			=	Utilities.mfApproach(0.75f, aScreenOverlay.blur, ACCELERATION * Time.deltaTime);
			aScratches.grainSize		=	Utilities.mfApproach(2.25f, aScratches.grainSize, ACCELERATION * 0.5f * Time.deltaTime);
			aBlackWhite.intensity		= 	Utilities.mfApproach(MAX_INTENSITY_BW,	  aBlackWhite.intensity,    ACCELERATION * 1.5f * Time.deltaTime);
			aFog.height					=	Utilities.mfApproach(2.0f, aFog.height, ACCELERATION * 15.0f * Time.deltaTime);
			yield return null;
		}
	}
	IEnumerator mcLerpUpDefensive()
	{
		GetComponent<NoiseAndScratches>().enabled	=	false;
		while (aScreenOverlay.intensity > 0.45f)
		{
			aScreenOverlay.intensity	=	Utilities.mfApproach(0.45f, aScreenOverlay.intensity,	ACCELERATION * 0.75f * Time.deltaTime);

			yield return null;
		}
	}

	public void mpLerpDownMiedoVisuals()
	{
		StartCoroutine(mcLerpDown());
	}

	IEnumerator mcLerpDown()
	{
		GetComponent<NoiseAndScratches>().enabled	=	false;
		while (aBlackWhite.intensity > 0.0f || aScreenOverlay.intensity > 0.0f || aScreenOverlay.blur > 0.0f || aFog.height > -10.0f)
		{
			aScreenOverlay.intensity	= 	Utilities.mfApproach(0.0f, aScreenOverlay.intensity,	ACCELERATION * Time.deltaTime);
			aScreenOverlay.blur			=	Utilities.mfApproach(0.0f, aScreenOverlay.blur, ACCELERATION * Time.deltaTime);

			aBlackWhite.intensity		= 	Utilities.mfApproach(0.0f, aBlackWhite.intensity,		ACCELERATION * Time.deltaTime);
			aFog.height					=	Utilities.mfApproach(-10.0f, aFog.height, ACCELERATION * 15.0f * Time.deltaTime);
			aFog.heightDensity			=	Utilities.mfApproach(0.0001f, aFog.heightDensity, ACCELERATION * 15.0f * Time.deltaTime);

			aAudioSource.volume			=	Utilities.mfApproach(0.0f, aAudioSource.volume, ACCELERATION * Time.deltaTime);

			yield return null;
		}

		//disable effects and destroy this script
		aScreenOverlay.enabled	=	false;
		aBlackWhite.enabled		=	false;


		MattManager	lManager	=	transform.root.Find("Character").GetComponent<MattManager>();

		if (lManager.mfEmotionIsEqualToDominantEmotion(eMatea.MIEDO))
			transform.root.Find("Character").GetComponent<MattManager>().mpResetMatea();

		Destroy(gameObject);
	}
}
