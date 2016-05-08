using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarsMATEA : MonoBehaviour 
{
	public	GameObject	aMatt;

	//reference to each of the MATEA bars' transform components
	public	Transform	aMiedo;
	public	Transform	aAlegria;
	public	Transform	aTristeza;
	public	Transform	aEnojo;
	public	Transform	aAmor;

	//reference to the MATEA controller
	private	MattMATEA	aMateaController;

	private	float		aBarIncreaseSpeed;
	private	Vector3		aCachedScale;

	void Start()
	{
		aBarIncreaseSpeed 	= 0.02f;
		aCachedScale  		= new Vector3(1, 1, 1);
		aMateaController	= aMatt.GetComponent<MattManager>();
	}

	//Dinamically resize MATEA bars to current values
	void Update()
	{
		//cache and assign values
		aCachedScale.y		 =	Utilities.mfApproach( aMateaController.mfGetValue(eMatea.MIEDO) * 0.01f,    aMiedo.localScale.y,    aBarIncreaseSpeed);
		aMiedo.localScale	 =	aCachedScale;

		aCachedScale.y		 =	Utilities.mfApproach( aMateaController.mfGetValue(eMatea.ALEGRIA) * 0.01f,  aAlegria.localScale.y,  aBarIncreaseSpeed);
		aAlegria.localScale	 =	aCachedScale;

		aCachedScale.y		 =	Utilities.mfApproach( aMateaController.mfGetValue(eMatea.TRISTEZA) * 0.01f, aTristeza.localScale.y, aBarIncreaseSpeed);
		aTristeza.localScale =	aCachedScale;

		aCachedScale.y		 =	Utilities.mfApproach( aMateaController.mfGetValue(eMatea.ENOJO) * 0.01f,	aEnojo.localScale.y,    aBarIncreaseSpeed);
		aEnojo.localScale	 =	aCachedScale;

		aCachedScale.y		 =	Utilities.mfApproach( aMateaController.mfGetValue(eMatea.AMOR) * 0.01f,     aAmor.localScale.y,     aBarIncreaseSpeed);
		aAmor.localScale	 =	aCachedScale;
	}
}
