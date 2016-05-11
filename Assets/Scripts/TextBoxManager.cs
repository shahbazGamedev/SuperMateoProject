using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
	public	GameObject	aTextbox;
	public	Text 		aText;

	public	TextAsset	aTextFile;
	public	string[]	aTextLines;

	public	int 		aCurrentLine;
	public	int 		aEndAtLine;

	public	bool		aIsActive;
	public	bool		aStopPlayerMovement;

	private	MattManager	aMattManager;
	private	AudioSource	aAudioSource;
	public	AudioClip	aOpen;
	public	AudioClip	aClose;
	public	AudioClip	aNext;

	private bool 		aIsTyping 		= false;
	private bool		aCancelTyping	= false;

	public	float		aTypeSpeed;

	private	char[]		aSeparator = {'\n'};

	void Start()
	{
		aAudioSource	=	GetComponent<AudioSource>();
		aMattManager	=	FindObjectOfType<MattManager>();

		if (aTextFile)
		{
			aTextLines 	=	aTextFile.text.Split(aSeparator);
		}

		if (aEndAtLine <= 0)
		{
			aEndAtLine	=	aTextLines.Length - 1;
		}

		if (aIsActive)
		{
			mpEnableTextBox();
		}
		else
		{
			mpDisableTextBox();
		}
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
					mpDisableTextBox();
				}
				else
				{
					aAudioSource.PlayOneShot(aNext);
					StartCoroutine(mcTextScroll(aTextLines[aCurrentLine]));
				}
			}
			else
			{
				if (!aCancelTyping)
				{
					aAudioSource.PlayOneShot(aNext);
					aCancelTyping = true;
				}
			}
		}
	}

	IEnumerator	mcTextScroll(string pLine)
	{
		int lLineLength	=	pLine.Length;
		int lLetter 	=	0;
		aText.text 		=	"";

		aIsTyping 		=	true;
		aCancelTyping 	=	false;

		while (aIsTyping && !aCancelTyping && (lLetter < lLineLength - 1))
		{
			aText.text 	+=	pLine[lLetter++];

			yield return new WaitForSeconds(aTypeSpeed);
		}

		aText.text 		=	pLine;
		aIsTyping 		= 	false;
		aCancelTyping	=	false;
	}

	public void mpEnableTextBox()
	{
		aAudioSource.PlayOneShot(aOpen);
		aTextbox.SetActive(true);
		aIsActive 	=	true;

		if (aStopPlayerMovement)
		{
			aMattManager.mpDisableMattMovement();
		}
		
		StartCoroutine(mcTextScroll(aTextLines[aCurrentLine]));
	}

	public void mpDisableTextBox()
	{
		aAudioSource.PlayOneShot(aClose);
		aTextbox.SetActive(false);
		aIsActive 	=	false;

		if (aStopPlayerMovement)
		{
			aMattManager.mpEnableMattMovement();
		}
	}

	public void mpLoadScript(TextAsset pText)
	{
		if (pText)
		{
			aTextLines 	=	pText.text.Split(aSeparator);
		}
	}
}