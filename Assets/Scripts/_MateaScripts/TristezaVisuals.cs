using UnityEngine;
using UnityStandardAssets.ImageEffects;
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

	private	ColorCorrectionCurves	aColorCurves;

	private	const	float	FOV_ACCELERATION 	= 	120.0f;
	private	const	float	BW_ACCELERATION 	= 	1.5f;

	private	const	float	MAX_FOV				=	120.0f;	
	private	const	float	MIN_FOV				=	60.0f;	
	private	const	float	MIN_COLOR_SATUR		=	0.125f;	

	void Start () 
	{
		aCamera			=	GetComponent<Camera>();

		aHiddenObjects	=	GameObject.FindGameObjectsWithTag("Hideable");
		aHiddenIndexes	=	new ArrayList();
		aObjectsCount	=	aHiddenObjects.Length;

		for (int i = 0; i < aObjectsCount; i++)
		{
			if (Utilities.mfExecuteRNG(90))
			{
				aHiddenIndexes.Add(i);
				aHiddenObjects[i].SetActive(false);
			}
		}

		aRainReference	=	(GameObject)Instantiate(aRainCloud, transform.position, transform.rotation);
		aRainReference.transform.SetParent(transform.parent);

		aColorCurves			=	GetComponent<ColorCorrectionCurves>();
		aColorCurves.enabled 	=	true;

		StartCoroutine(mcLerpUp());
	}

	IEnumerator mcLerpUp()
	{
		while (aColorCurves.blueChannel.keys[0].value < 1.0f)
		{
			aColorCurves.saturation	=	Utilities.mfApproach(MIN_COLOR_SATUR, aColorCurves.saturation, BW_ACCELERATION * Time.deltaTime);


			aColorCurves.blueChannel	=	AnimationCurve.Linear(0.0f,
																  Utilities.mfApproach(1.0f, aColorCurves.blueChannel.keys[0].value, 0.05f), 
																  1.0f, 
																  Utilities.mfApproach(1.0f, aColorCurves.blueChannel.keys[1].value, 0.05f));

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
		while (aColorCurves.saturation < 1.0f)
		{
			aColorCurves.saturation	=	Utilities.mfApproach(1.0f, aColorCurves.saturation, BW_ACCELERATION * Time.deltaTime);


			aColorCurves.blueChannel	=	AnimationCurve.EaseInOut(0.0f,
																  	 Utilities.mfApproach(0.0f, aColorCurves.blueChannel.keys[0].value, 0.05f), 
																 	 1.0f, 
																  	 1.0f);

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
