using UnityEngine;
using System.Collections;

public class TristezaVisuals : MonoBehaviour 
{
	public	GameObject		aRainCloud;
	private	GameObject		aRainReference;

	//reference to MattStatus
	private	Camera			aCamera;

	private	GameObject[]	aHiddenObjects;
	private	ArrayList		aHiddenIndexes;
	private	int				aObjectsCount;

	private	FadeOutColor	aBlackWhite;

	private	const	float	FOV_ACCELERATION 	= 	120.0f;
	private	const	float	BW_ACCELERATION 	= 	1.5f;

	private	const	float	MAX_FOV				=	120.0f;	
	private	const	float	MIN_FOV				=	60.0f;	
	private	const	float	MAX_INTENSITY_BW	=	0.85f;	

	void Start () 
	{
		aCamera			=	GetComponent<Camera>();

		aHiddenObjects	=	GameObject.FindGameObjectsWithTag("Hideable");
		aHiddenIndexes	=	new ArrayList();
		aObjectsCount	=	aHiddenObjects.Length;

		for (int i = 0; i < aObjectsCount; i++)
		{
			if (Utilities.mfExecuteRNG(80))
			{
				aHiddenIndexes.Add(i);
				aHiddenObjects[i].SetActive(false);
			}
		}

		aRainReference	=	(GameObject)Instantiate(aRainCloud, transform.position, transform.rotation);
		aRainReference.transform.SetParent(transform.parent);

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
		while (aCamera.fieldOfView < MAX_FOV)
		{
			aCamera.fieldOfView		=	Utilities.mfApproach(MAX_FOV, aCamera.fieldOfView, FOV_ACCELERATION * Time.deltaTime);
			yield return null;
		}
	}

	public void mpLerpDownTristezaVisuals()
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
		while (aCamera.fieldOfView > MIN_FOV)
		{
			aCamera.fieldOfView		=	Utilities.mfApproach(MIN_FOV, aCamera.fieldOfView, FOV_ACCELERATION * Time.deltaTime);
			yield return null;
		}

		foreach (int lIndex  in aHiddenIndexes) 
		{
			aHiddenObjects[lIndex].SetActive(true);	
		}

		aRainReference.GetComponent<RainBehaviour>().mpStopRain();
		transform.root.Find("Character").GetComponent<MattManager>().mpResetMatea();
		Destroy(gameObject);
	}
}
