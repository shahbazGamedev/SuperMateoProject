using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmCubeCtrller : MonoBehaviour 
{
	public	AudioClip	aUp;
	public	AudioClip	aDown;

	private	AudioSource	aAudioSource;

	private	bool		aAlreadyTaken;

	void Start()
	{
		aAudioSource		=	GetComponent<AudioSource>();
		aAlreadyTaken		=	false;
	}

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			if (!aAlreadyTaken)
			{
				//write message and add proper sign
				if (pOther.GetComponent<MattMATEA>().mfEmCubeBonus(Utilities.mfGetRandomEmotion()) > 0)
					aAudioSource.PlayOneShot(aUp);
				else
					aAudioSource.PlayOneShot(aDown);

				aAlreadyTaken									=	true;
				GetComponentInChildren<MeshRenderer>().enabled	=	false;

				//destroy this cube
				Destroy(gameObject, 3.0f);
			}
		}
	}
}
