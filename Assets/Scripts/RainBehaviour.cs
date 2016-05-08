using UnityEngine;
using System.Collections;

public class RainBehaviour : MonoBehaviour 
{
	public	float	aMaxRate;
	public	float	aAcceleration;

	private	ParticleSystem.EmissionModule	aEmission;
	private	ParticleSystem.MinMaxCurve		aRate;

	private	AudioFadeInOut	aRainSFXManager;

	void Start()
	{
		aRainSFXManager		=	GetComponent<AudioFadeInOut>();
		aEmission			=	transform.GetChild(0).GetComponent<ParticleSystem>().emission;
		aRate				=	new ParticleSystem.MinMaxCurve();

		aRate.constantMax	=	0.1f;
		aEmission.rate 		=	aRate;

		StartCoroutine(mcMakeRain());
	}

	IEnumerator mcMakeRain()
	{
		while (aRate.constantMax < aMaxRate)
		{
			aRate.constantMax	=	Utilities.mfApproach(aMaxRate, aRate.constantMax, aAcceleration * Time.deltaTime);
			aEmission.rate 		= 	aRate;
			yield return null;
		}
	}

	IEnumerator mcStopRain()
	{
		while (aRate.constantMax > 0.0f)
		{
			aRate.constantMax	=	Utilities.mfApproach(0.0f, aRate.constantMax, aAcceleration * Time.deltaTime);
			aEmission.rate 		=	aRate;
			yield return null;
		}
	}

	public void mpStopRain()
	{
		aRainSFXManager.mpFadeOutVolume();
		StartCoroutine(mcStopRain());
	}
}