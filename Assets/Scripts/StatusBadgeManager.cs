using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBadgeManager : MonoBehaviour 
{
	public	Sprite	aIncreaseSprite;
	public	Sprite	aDecreaseSprite;
	public	Sprite	aOKSprite;

	public	Image	aStrength;
	public	Image	aDefense;
	public	Image	aSpeed;

	private	Animation	aStrAnim;
	private	Animation	aDefAnim;
	private	Animation	aSpdAnim;

	void Start()
	{
		aStrAnim	=	aStrength.GetComponent<Animation>();
		aDefAnim	=	aDefense.GetComponent<Animation>();
		aSpdAnim	=	aSpeed.GetComponent<Animation>();
	}

	private void mpSetAnimations(Animation pAnim, float pValue)
	{
		if (pValue > 1.0f)
		{
			pAnim.Play("statusIncrease");
		}
		else if (pValue == 1.0f)
		{
			pAnim.Play("statusOK");
		}
		else
		{
			pAnim.Play("statusDecrease");
		}
	}

	public void mpSetValues(float pStrength, float pSpeed, float pDefense)
	{
		aStrength.sprite	=	mfGetSprite(pStrength);
		mpSetAnimations(aStrAnim, pStrength);

		aDefense.sprite		=	mfGetSprite(pDefense);
		mpSetAnimations(aDefAnim, pDefense);

		aSpeed.sprite		=	mfGetSprite(pSpeed);
		mpSetAnimations(aSpdAnim, pSpeed);
	}

	public void mpOKValues()
	{
		mpSetValues(1, 1, 1);
	}

	private Sprite mfGetSprite(float pValue)
	{
		if (pValue > 1.0f)
		{
			return aIncreaseSprite;
		}
		else if (pValue == 1.0f)
		{
			return aOKSprite;
		}
		else
		{
			return aDecreaseSprite;
		}
	}
}
