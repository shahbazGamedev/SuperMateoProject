using UnityEngine;
using System.Collections;

public class BokehBehaviour : MonoBehaviour 
{
	public	float	aMaxRate;
	public	float	aAcceleration;

	private	ParticleSystem 					aSystem;
	private	ParticleSystem.EmissionModule	aEmission;
	private	ParticleSystem.MinMaxCurve		aRate;

	private	AudioFadeInOut	aSFXManager;

	void Start()
	{
		aSFXManager	=	GetComponent<AudioFadeInOut>();
		aSystem		=	transform.GetChild(0).GetComponent<ParticleSystem>();
		aEmission	=	aSystem.emission;
		aRate		=	new ParticleSystem.MinMaxCurve();

		aRate.constantMax	=	0.1f;
		aEmission.rate 		=	aRate;

		StartCoroutine(mcMakeBokeh());
	}

	IEnumerator mcMakeBokeh()
	{
		while (aRate.constantMax < aMaxRate)
		{
			aRate.constantMax	=	Utilities.mfApproach(aMaxRate, aRate.constantMax, aAcceleration * Time.deltaTime);
			aEmission.rate 		= 	aRate;
			yield return null;
		}
	}

	IEnumerator mcStopBokeh()
	{
		while (aRate.constantMax > 0.0f)
		{
			aRate.constantMax		=	Utilities.mfApproach(0.0f, aRate.constantMax, aAcceleration * Time.deltaTime);
			aEmission.rate 			=	aRate;
			aSystem.startLifetime	=	1.0f;
			yield return null;
		}
	}

	public void mpStopBokeh()
	{
		aSFXManager.mpFadeOutVolume();
		StartCoroutine(mcStopBokeh());
	}
}
