﻿using UnityEngine;
using System.Collections;

public class AlegriaController : MonoBehaviour 
{
	public	int				aEmotionDuration;

	public	GameObject		aBokehCloud;
	private	GameObject		aBokehReference;

	private	FadeOutColor	aBlackWhite;

	private	const	float	BW_ACCELERATION 	= 	 1.5f;
	private	const	float	MAX_INTENSITY_BW	=	-2.5f;	

	void Start () 
	{	
		aBokehReference	=	(GameObject)Instantiate(aBokehCloud, transform.position, transform.rotation);
		aBokehReference.transform.SetParent(transform.parent);

		aBlackWhite				=	GetComponent<FadeOutColor>();
		aBlackWhite.enabled 	=	true;
		aBlackWhite.intensity	=	0.0f;

		StartCoroutine(mcExecuteEmotion());
	}

	IEnumerator mcExecuteEmotion()
	{
		while (aBlackWhite.intensity > MAX_INTENSITY_BW)
		{
			aBlackWhite.intensity	=	Utilities.mfApproach(MAX_INTENSITY_BW, aBlackWhite.intensity, BW_ACCELERATION * Time.deltaTime);
			yield return null;
		}

		yield return new WaitForSeconds(aEmotionDuration);


		while (aBlackWhite.intensity < 0.0f)
		{
			aBlackWhite.intensity	=	Utilities.mfApproach(0.0f, aBlackWhite.intensity, BW_ACCELERATION * Time.deltaTime);
			yield return null;
		}

		aBokehReference.GetComponent<BokehBehaviour>().mpStopBokeh();
		transform.root.Find("Character").GetComponent<MattManager>().mpResetMatea();
		Destroy(gameObject);
	}
}