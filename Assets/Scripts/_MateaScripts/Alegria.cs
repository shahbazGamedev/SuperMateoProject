using UnityEngine;
using System.Collections;

public class Alegria : MonoBehaviour 
{
	private	StatusBadgeManager	aStatusBadgeManager;
	private	MattManager			aMattManager;

	[Range(0.1f, 2.0f)]
	public	float	aStrMultiplier;
	[Range(1.0f, 3.0f)]
	public	float	aSpdMultiplier;
	[Range(0.1f, 1.0f)]
	public	float	aAccMultiplier;
	[Range(0.1f, 1.0f)]
	public	float	aHitResMultiplier;
	[Range(0.1f, 2.0f)]
	public	float	aDefMultiplier;

	private	GameObject	aAlegriaObject;
	private	MattCamera	aCameraScript;

	void Start()
	{
		aStatusBadgeManager	=	GameObject.Find("_gameHUD").GetComponentInChildren<StatusBadgeManager>();
		aMattManager		=	GetComponent<MattManager>();
		aCameraScript		=	aMattManager.aMattCamera.GetComponent<MattCamera>();
		mpInitAlegria();
	}

	void mpInitAlegria()
	{
		Utilities.mpMuteBgMusic();
		aMattManager.aBiorhythm	=	eMatea.ALEGRIA;

		aAlegriaObject	=	Utilities.mfCreateEmotionObject(aMattManager.aBiorhythm, aMattManager.aEmotionObjects[(int)aMattManager.aBiorhythm], aMattManager.aMattCamera);

		aCameraScript.aFollowUpSpeed	=	80.0f;

		aMattManager.mpEnableMultipliers(aStrMultiplier, aSpdMultiplier, aDefMultiplier);
		aStatusBadgeManager.mpSetValues(aStrMultiplier, aSpdMultiplier, aDefMultiplier);
		aMattManager.mpMultiplyAcceleration(aAccMultiplier, aHitResMultiplier);
		aMattManager.aAnimator.speed	=	2.0f;

		StartCoroutine(mcBeginTimer(5.0f));
	}

	IEnumerator mcBeginTimer(float pTime)
	{
		yield return new WaitForSeconds(pTime);
		this.enabled	=	false;
	}

	void OnEnable()
	{
		if (aMattManager)
		{
			//always apply initialization on enable
			mpInitAlegria();
		}
	}

	void OnDisable()
	{
		aMattManager.mpDisableMultipliers();
		aStatusBadgeManager.mpOKValues();

		aMattManager.mpRestoreAcceleration();
		aMattManager.aBiorhythm			=	eMatea.NORMAL;
		aMattManager.aAnimator.speed	=	1.0f;

		Utilities.mpPlayBgMusic();

		aAlegriaObject.GetComponent<AlegriaVisuals>().mpLerpDownAlegriaVisuals();
		aCameraScript.aFollowUpSpeed	=	25.0f;
	}
}
