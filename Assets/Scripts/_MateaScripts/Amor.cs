using UnityEngine;
using System.Collections;

public class Amor : MonoBehaviour 
{
	private	StatusBadgeManager	aStatusBadgeManager;
	private	MattManager			aMattManager;

	[Range(0.1f, 2.0f)]
	public	float	aStrMultiplier;
	[Range(0.1f, 2.0f)]
	public	float	aSpdMultiplier;
	[Range(0.1f, 2.0f)]
	public	float	aDefMultiplier;

	private	GameObject	aAmorObject;

	void Start()
	{
		aStatusBadgeManager	=	GameObject.Find("_gameHUD").GetComponentInChildren<StatusBadgeManager>();
		aMattManager		=	GetComponent<MattManager>();
		mpInitAmor();
	}

	void mpInitAmor()
	{
		Utilities.mpMuteBgMusic();
		aMattManager.aBiorhythm	=	eMatea.AMOR;

		aAmorObject	=	Utilities.mfCreateEmotionObject(aMattManager.aBiorhythm, aMattManager.aEmotionObjects[(int)aMattManager.aBiorhythm], aMattManager.aMattCamera);

		aMattManager.mpEnableMultipliers(aStrMultiplier, aSpdMultiplier, aDefMultiplier);
		aStatusBadgeManager.mpSetValues(aStrMultiplier, aSpdMultiplier, aDefMultiplier);
	}

	void OnEnable()
	{
		if (aMattManager)
		{
			//always apply initialization on enable
			mpInitAmor();
		}
	}

	void OnDisable()
	{
		aMattManager.mpDisableMultipliers();
		aStatusBadgeManager.mpOKValues();

		aMattManager.mpFullRestore(); 

		aMattManager.aBiorhythm	=	eMatea.NORMAL;

		Utilities.mpPlayBgMusic();
		aAmorObject.GetComponent<AmorVisuals>().mpLerpDownAmorVisuals();
	}
}
