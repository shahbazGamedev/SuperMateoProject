using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlideshowTextManager : MonoBehaviour 
{
	private	IntroSlidePlay	aPlayer;

	private	GameObject	aTextbox;
	private	Text 		aText;

	private	string		aCurrentText;

	public	TextAsset	aTextFile;
	private	string[]	aTextLines;

	public	int 		aCurrentLine;
	private	int 		aEndAtLine;

	public	bool		aIsActive;

	private bool 		aIsTyping 		=	false;
	private bool		aCancelTyping	=	false;

	private	bool		aUseCenterBox;

	public	float		aTypeSpeed;

	private	char[]	aSeparator = {'*'};

	public void mpSetTextbox(GameObject pTextbox)
	{
		if (aTextbox)
		{
			mpDisableTextBox();
		}

		aTextbox	=	pTextbox;
		aText		=	aTextbox.GetComponentInChildren<Text>();

		mpWaitBeforeActivating(1.0f);
	}

	void Start()
	{
		aPlayer	=	GetComponent<IntroSlidePlay>();

		if (aTextFile)
		{
			aTextLines 	=	aTextFile.text.Split(aSeparator);
		}

		if (aEndAtLine <= 0)
		{
			aEndAtLine	=	aTextLines.Length - 1;
		}
	}	

	public void mpWaitBeforeActivating(float pTime)
	{
		StopAllCoroutines();
		StartCoroutine(mcWait(pTime));
	}

	IEnumerator mcWait(float pTime)
	{
		yield return new WaitForSeconds(pTime);
		mpEnableTextBox();
	}

	void Update()
	{
		if (!aIsActive)
			return;

		if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A"))
		{
			if (!aIsTyping)
			{

				if (++aCurrentLine > aEndAtLine)
				{
					aPlayer.mpFadeOut();
				}
				else
				{
					aPlayer.mpGetNextImage();
					StartCoroutine(mcTextScroll(aTextLines[aCurrentLine]));
				}
			}
			else
			{
				if (!aCancelTyping)
				{
					aCancelTyping = true;
				}
			}
		}
	}

	IEnumerator	mcTextScroll(string pLine)
	{
		int lLineLength	=	pLine.Length;
		int lLetter 	=	0;

		aText.text	=	"";

		aIsTyping 		=	true;
		aCancelTyping 	=	false;

		while (aIsTyping && !aCancelTyping && (lLetter < lLineLength - 1))
		{
			aText.text 	+=	pLine[lLetter++];
			yield return new WaitForSeconds(aTypeSpeed);
		}

		aText.text		=	pLine;

		aIsTyping 		= 	false;
		aCancelTyping	=	false;
	}

	public void mpEnableTextBox()
	{
		aIsActive 	=	true;
		aTextbox.SetActive(aIsActive);

		StartCoroutine(mcTextScroll(aTextLines[aCurrentLine]));
	}

	public void mpDisableTextBox()
	{
		aIsActive 	=	false;
		aTextbox.SetActive(aIsActive);
	}
}