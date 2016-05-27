using UnityEngine;
using System.Collections;

public class AudioFadeInOut : MonoBehaviour 
{
	public	float		aAcceleration;
	private	AudioSource	aSource;

	void Start()
	{
		aSource		=	GetComponent<AudioSource>();
		StartCoroutine(mcFadeIn());
	}

	IEnumerator mcFadeIn()
	{
		while (aSource.volume < 1.0f)
		{
			aSource.volume	=	Utilities.mfApproach(1.0f, aSource.volume, aAcceleration * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator mcFadeOut()
	{

		while (aSource.volume > 0.0f)
		{
			aSource.volume	=	Utilities.mfApproach(0.0f, aSource.volume, aAcceleration * Time.deltaTime);
			yield return null;
		}

		Destroy(gameObject);
	}

	public void mpFadeOutVolume()
	{
		StopAllCoroutines();
		StartCoroutine(mcFadeOut());
	}
}
