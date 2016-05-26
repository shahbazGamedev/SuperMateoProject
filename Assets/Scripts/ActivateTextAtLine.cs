using UnityEngine;
using System.Collections;

public class ActivateTextAtLine : MonoBehaviour
{
	public 	TextBoxManager 	aManager;
	public	TextAsset 		aText;

	public	int 	aStartLine;
	public 	int 	aEndLine;
	public 	bool	aRequireButtonPress;
	private bool	aWaitForPress;
	public	bool	aDestroyWhenActive;

	void Start()
	{
		aManager 	=	FindObjectOfType<TextBoxManager>();
	}

	void Update()
	{
		if (aWaitForPress)
		{
			if (Input.GetButtonDown("X"))
			{
				mpExecute();
			}
		}
	}

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			if (aRequireButtonPress)
			{
				aWaitForPress = true;
				return;
			}

			mpExecute();
		}
	}

	void OnTriggerExit(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			aWaitForPress = false;
		}
	}

	void mpExecute()
	{
		if (aManager.aIsActive)
		{
			return;
		}

		aManager.mpLoadScript(aText, aStartLine, aEndLine);

		if (aDestroyWhenActive)
		{
			Destroy(gameObject);
		}
	}
}