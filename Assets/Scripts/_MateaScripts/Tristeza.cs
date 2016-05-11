using UnityEngine;
using System.Collections;

public class Tristeza : MonoBehaviour 
{
	private	MattManager	aMattManager;

	[Range(0.1f, 2.0f)]
	public	float	aStrMultiplier;
	[Range(0.1f, 2.0f)]
	public	float	aSpdMultiplier;
	[Range(0.1f, 2.0f)]
	public	float	aDefMultiplier;

	private	GameObject	aTristezaObject;

	void Start()
	{
		aMattManager	=	GetComponent<MattManager>();
		mpInitTristeza();
	}

	void mpInitTristeza()
	{
		aMattManager.aBiorhythm	=	eMatea.TRISTEZA;

		aTristezaObject	=	Utilities.mfCreateEmotionObject(aMattManager.aBiorhythm, aMattManager.aEmotionObjects[(int)aMattManager.aBiorhythm], aMattManager.aMattCamera);

		aMattManager.mpEnableMultipliers(aStrMultiplier, aSpdMultiplier, aDefMultiplier);
	}

	void OnEnable()
	{
		if (aMattManager)
		{
			//always apply initialization on enable
			mpInitTristeza();
		}
	}

	void OnDisable()
	{
		aMattManager.mpDisableMultipliers();
		aMattManager.aBiorhythm	=	eMatea.NORMAL;

		aTristezaObject.GetComponent<TristezaVisuals>().mpLerpDownTristezaVisuals();
	}
}
