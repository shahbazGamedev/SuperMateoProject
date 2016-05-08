using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour 
{
	private	static	Vector3		aSpawnPosition;
	public			Transform	aMatt;

	private	static	Transform	aCharacter;
	private	static	Transform	aCamera;

	private	static	Vector3		aOffsetFromCharacter;

	private	static	MattCamera	aMattCamera;

	void Start()
	{
		aCharacter				=	aMatt.FindChild("Character");

		aCamera					=	aMatt.FindChild("Camera");
		aMattCamera				=	aCamera.GetComponent<MattCamera>();

		aSpawnPosition			=	aCharacter.transform.position;

		aOffsetFromCharacter	=	aCamera.transform.position - aCharacter.transform.position;

	}

	public static void mpSetNewCheckPoint(Vector3 pSpawnPosition)
	{
		aSpawnPosition	=	pSpawnPosition;
	}

	public static void mpLerpCameraOverlay()
	{
		aMattCamera.mpLerpOverlay();
	}

	public static void mpSendMattToCheckpoint()
	{
		aCharacter.transform.position	=	aSpawnPosition;
		aCamera.transform.position		=	aSpawnPosition + aOffsetFromCharacter;
	}
}
