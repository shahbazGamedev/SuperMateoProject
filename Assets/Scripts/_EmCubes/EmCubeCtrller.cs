using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmCubeCtrller : MonoBehaviour 
{
	public	AudioClip	aUp;
	public	AudioClip	aDown;

	private	AudioSource	aAudioSource;

	private	bool		aAlreadyTaken;

	private	Text		aNotificationText;

	void Start()
	{
		aAudioSource		=	GetComponent<AudioSource>();
		aNotificationText	=	Utilities.aNotificationPanel.GetComponentInChildren<Text>();
		aAlreadyTaken		=	false;
	}

	IEnumerator mcToggleNotification()
	{
		Utilities.aNotificationPanel.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.8f);	
		Utilities.aNotificationPanel.gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			if (!aAlreadyTaken)
			{
				if (pOther.GetComponent<MattMATEA>().mfMattIsNormal())
				{
					//cache Matt's position
					int		lEmCubeResult	=	Random.Range(20, 50);
					string	lEmotionString	=	"";

					//pick a random emotion to edit
					switch (Utilities.mfGetRandomEmotion())
					{
					case eMatea.MIEDO: default:
						lEmCubeResult	=	pOther.GetComponent<MattMATEA>().mfIncreaseEmotionByValue(eMatea.MIEDO, lEmCubeResult);
						lEmotionString	=	" Miedo";
						break;
					case eMatea.ALEGRIA:
						lEmCubeResult	=	pOther.GetComponent<MattMATEA>().mfIncreaseEmotionByValue(eMatea.ALEGRIA, lEmCubeResult);
						lEmotionString	=	" Alegría";
						break;
					case eMatea.TRISTEZA:
						lEmCubeResult	=	pOther.GetComponent<MattMATEA>().mfIncreaseEmotionByValue(eMatea.TRISTEZA, lEmCubeResult);
						lEmotionString	=	" Tristeza";
						break;
					case eMatea.ENOJO:
						lEmCubeResult	=	pOther.GetComponent<MattMATEA>().mfIncreaseEmotionByValue(eMatea.ENOJO, lEmCubeResult);
						lEmotionString	=	" Enojo";
						break;
					case eMatea.AMOR:
						lEmCubeResult	=	pOther.GetComponent<MattMATEA>().mfIncreaseEmotionByValue(eMatea.AMOR, lEmCubeResult);
						lEmotionString	=	" Amor";
						break;
					}

					//write message and add proper sign
					if (lEmCubeResult > 0)
					{
						aAudioSource.PlayOneShot(aUp);
						lEmotionString	=	lEmotionString + " +" + lEmCubeResult;
					}
					else
					{
						aAudioSource.PlayOneShot(aDown);
						lEmotionString	=	lEmotionString + " " + lEmCubeResult;
					}
					aNotificationText.text	=	lEmotionString;

					StartCoroutine(mcToggleNotification());
				}

				aAlreadyTaken	=	true;
				GetComponentInChildren<MeshRenderer>().enabled	=	false;

				//destroy this cube
				Destroy(gameObject, 3.0f);
			}


		}
	}
}
