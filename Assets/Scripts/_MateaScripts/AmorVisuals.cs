using UnityEngine;
using System.Collections;

public class AmorVisuals : MonoBehaviour 
{

	public	GameObject		aHeartsCloud;
	private	GameObject		aHeartReference;

	private	FadeOutColor	aBlackWhite;

	private	const	float	BW_ACCELERATION 	= 	1.5f;
	private	const	float	MAX_INTENSITY_BW	=	2.5f;	

	void Start () 
	{	
		aHeartReference	=	(GameObject)Instantiate(aHeartsCloud, transform.position, transform.rotation);
		aHeartReference.transform.SetParent(transform.parent);

		aBlackWhite				=	GetComponent<FadeOutColor>();
		aBlackWhite.enabled 	=	true;
		aBlackWhite.intensity	=	0.0f;

		StartCoroutine(mcLerpUp());
	}

	IEnumerator mcLerpUp()
	{
		while (aBlackWhite.intensity < MAX_INTENSITY_BW)
		{
			aBlackWhite.intensity	=	Utilities.mfApproach(MAX_INTENSITY_BW, aBlackWhite.intensity, BW_ACCELERATION * Time.deltaTime);
			yield return null;
		}
	}

	public void mpLerpDownAmorVisuals()
	{
		StartCoroutine(mcLerpDown());
	}

	IEnumerator mcLerpDown()
	{
		while (aBlackWhite.intensity > 0.0f)
		{
			aBlackWhite.intensity	=	Utilities.mfApproach(0.0f, aBlackWhite.intensity, BW_ACCELERATION * Time.deltaTime);
			yield return null;
		}
		print("oh");
		aHeartReference.GetComponent<BokehBehaviour>().mpStopBokeh();

		MattManager	lManager	=	transform.root.Find("Character").GetComponent<MattManager>();

		if (lManager.mfEmotionIsEqualToDominantEmotion(eMatea.AMOR))
			transform.root.Find("Character").GetComponent<MattManager>().mpResetMatea();

		Destroy(gameObject);
	}
}
