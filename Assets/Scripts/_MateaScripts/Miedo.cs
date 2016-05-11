﻿using UnityEngine;
using System.Collections;

public class Miedo : MonoBehaviour 
{
	public	eMiedoPhase	aMiedoState;

	private	MattManager	aMattManager;

	[Range(0.1f, 2.0f)]
	public	float	aStrMultiplierStealth;
	[Range(0.1f, 2.0f)]
	public	float	aSpdMultiplierStealth;
	[Range(0.1f, 2.0f)]
	public	float	aDefMultiplierStealth;

	[Range(0.1f, 2.0f)]
	public	float	aStrMultiplierDefensive;
	[Range(0.1f, 2.0f)]
	public	float	aSpdMultiplierDefensive;
	[Range(0.1f, 2.0f)]
	public	float	aDefMultiplierDefensive;
	[Range(1.1f, 2.0f)]
	public	float	aMattDefensiveScale;

	private	GameObject	aMiedoObject;

	void Start()
	{
		aMattManager	=	GetComponent<MattManager>();
		mpInitMiedo();
	}

	void mpInitMiedo()
	{
		aMattManager.aBiorhythm	=	eMatea.MIEDO;

		aMiedoObject	=	Utilities.mfCreateEmotionObject(aMattManager.aBiorhythm, aMattManager.aEmotionObjects[(int)aMattManager.aBiorhythm], aMattManager.aMattCamera);

		mpActivateStealthMode();
	}

	public void mpActivateDefensiveMode()
	{
		aMiedoState	=	eMiedoPhase.DEFENSIVE;
		aMattManager.transform.localScale	*=	aMattDefensiveScale;
		aMattManager.mpEnableMultipliers(aStrMultiplierDefensive, aSpdMultiplierDefensive, aDefMultiplierDefensive);
	}

	public void mpActivateStealthMode()
	{
		aMiedoState	=	eMiedoPhase.STEALTH;
		aMattManager.mpEnableMultipliers(aStrMultiplierStealth, aSpdMultiplierStealth, aDefMultiplierStealth);
	}

	void OnEnable()
	{
		if (aMattManager)
		{
			//always apply initialization on enable
			mpInitMiedo();
		}
	}

	void OnDisable()
	{
		if (aMiedoState == eMiedoPhase.DEFENSIVE)
		{
			aMattManager.transform.localScale	/=	aMattDefensiveScale;
		}

		aMattManager.mpDisableMultipliers();
		aMattManager.aBiorhythm	=	eMatea.NORMAL;
		aMiedoObject.GetComponent<MiedoVisuals>().mpLerpDownMiedoVisuals();
	}
}