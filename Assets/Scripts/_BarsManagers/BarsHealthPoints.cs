using UnityEngine;
using System.Collections;

public class BarsHealthPoints : MonoBehaviour 
{
	public	GameObject	aMatt;

	//reference to the HP bar transform values
	public	Transform	aHPBar;

	//reference to the stats manager
	private	MattStatus	aController;

	private	float	aHPIncreaseSpeed;
	private	Vector3	aCachedNewScale;

	void Start()
	{
		aCachedNewScale  	= new Vector3(1, 1, 1);
		aController	  		= aMatt.GetComponent<MattManager>();
		aHPIncreaseSpeed 	= 0.02f;
	}

	//Dinamically resize health bar to current HP
	void Update()
	{
		//cache and assign value
		aCachedNewScale.x	=	Utilities.mfApproach(aController.currentHP * 0.01f, aCachedNewScale.x, aHPIncreaseSpeed);
		aHPBar.localScale	=	aCachedNewScale;
	}
}
