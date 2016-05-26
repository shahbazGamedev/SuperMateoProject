using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	public	string		aSelectionButtonName;

	private	Button[]	aButtons;
	private	int			aCurrentButton;
	private	int			aTotalButtons;

	public	Color		aSelectedColor;
	public	Color		aUnselectedColor;

	void Start()
	{
		aCurrentButton	=	0;

		aButtons		=	transform.GetComponentsInChildren<Button>();
		aTotalButtons	=	aButtons.Length;
	}

	private void mpColorButton(int pIndex, Color pColor)
	{
		aButtons[pIndex].GetComponent<Image>().color	=	pColor;
	}

	void Update()
	{
		if (Input.GetButtonDown("padDown"))
		{
			mpColorButton(aCurrentButton++, aUnselectedColor);
			aCurrentButton	=	aCurrentButton % aTotalButtons;
		}
		else if (Input.GetButtonDown("padUp"))
		{
			mpColorButton(aCurrentButton, aUnselectedColor);
			aCurrentButton	=	(aCurrentButton <= 0) ? (aTotalButtons - 1) : (aCurrentButton - 1); 
		}
		else if (Input.GetButtonDown(aSelectionButtonName))
		{
			aButtons[aCurrentButton].onClick.Invoke();
		}

		mpColorButton(aCurrentButton, aSelectedColor);

	}
}
