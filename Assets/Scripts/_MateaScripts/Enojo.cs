using UnityEngine;
using System.Collections;

public class Enojo : MonoBehaviour 
{
	private	MattManager	aMattManager;

	[Range(0.1f, 2.0f)]
	public	float		aStrMultiplier;
	[Range(1.0f, 3.0f)]
	public	float		aSpdMultiplier;
	[Range(0.1f, 2.0f)]
	public	float		aDefMultiplier;

	private	GameObject	aEnojoObject;

	void Start()
	{
		aMattManager	=	GetComponent<MattManager>();
		mpInitEnojo();
	}

	void mpInitEnojo()
	{
		aMattManager.aBiorhythm	=	eMatea.ENOJO;

		aEnojoObject	=	Utilities.mfCreateEmotionObject(aMattManager.aBiorhythm, aMattManager.aEmotionObjects[(int)aMattManager.aBiorhythm], aMattManager.aMattCamera);

		aMattManager.mpEnableMultipliers(aStrMultiplier, aSpdMultiplier, aDefMultiplier);
	}

	void OnEnable()
	{
		if (aMattManager)
		{
			//always apply initialization on enable
			mpInitEnojo();
		}
	}

	void OnDisable()
	{
		aMattManager.mpDisableMultipliers();
		aMattManager.aBiorhythm	=	eMatea.NORMAL;

		aEnojoObject.GetComponent<EnojoVisuals>().mpLerpDownAlegriaVisuals();
	}
}
