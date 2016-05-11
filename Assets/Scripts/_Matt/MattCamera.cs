using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class MattCamera : MonoBehaviour 
{
	private	float	aCurrentHeight;
	private float 	aHeight;

	[Range(25.0f, 80.0f)]
	//how fast does this camera follow its target?
	public	float	aFollowUpSpeed;
	private	float	aCurrentFollowUpSpeed;

	//camera position offset values
	private	Vector3	aOffsetFromTarget;

	//this flag activates whenever Matt is looking toward the player
	private bool	aPushCameraBackwards;

	//cache variables
	private	Vector3		aNextPosition;

	//reference to Matt's physics script
	private	MattPhysics	aMattPhysics;

	//this camera target-lookat
	private	Transform	aMattTransform;
	//facing vectors dot product value
	private	float		aFacingDirection;

	private	ScreenOverlay	aScreenOverlay;

	void Start()
	{
		aMattTransform	=	transform.parent.FindChild("Character");
		aMattPhysics	=	aMattTransform.GetComponent<MattPhysics>();
		aScreenOverlay	=	GetComponent<ScreenOverlay>();

		//init camera and define reference to Matt's physics script
		aCurrentHeight			=	0.8f;					
		aPushCameraBackwards	=	true;
		aOffsetFromTarget		=	new Vector3(0.0f, 3.5f, 6.0f);
	}

	IEnumerator mcLerpAlpha()
	{
		while (aScreenOverlay.intensity < 1.0f)
		{
			aScreenOverlay.intensity	=	Utilities.mfApproach(1.0f, aScreenOverlay.intensity, Time.deltaTime * 4.5f);
			yield return null;
		}

		while (aScreenOverlay.intensity > 0.0f)
		{
			aScreenOverlay.intensity	=	Utilities.mfApproach(0.0f, aScreenOverlay.intensity, Time.deltaTime * 2.5f);
			yield return null;
		}
	}

	public void mpLerpOverlay()
	{
		StartCoroutine(mcLerpAlpha());
	}

	void Update() 
	{	
		if (Input.GetButtonDown("home"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}

		//get current facing direction
		aFacingDirection = Vector3.Dot(transform.forward, aMattTransform.forward);

		//check if Matt is looking towards the player
		if (aFacingDirection < -0.5f)
		{
			//check if Matt is running
			if (aMattPhysics.mfMattIsRunning())
			{
				//move camera so the player can see what is behind Matt
				aPushCameraBackwards = true;
			}
		}
		else
		{
			//validation: Matt can walk on the X axis without moving the camera
			if (aPushCameraBackwards)
			{
				if (aFacingDirection > 0.25f)
				{
					aPushCameraBackwards = false;
				}
			}
			else
			{
				aPushCameraBackwards = false;
			}
		}

		//modify camera tilt and follow up if Matt jumps
		if (aMattPhysics.mfMattIsGrounded())
		{
			aCurrentFollowUpSpeed	=	aFollowUpSpeed;
		}
		else
		{
			if (aMattPhysics.velocity_Y < 0.0f)
			{
				aCurrentFollowUpSpeed	=	aFollowUpSpeed * 10.0f;
			}
		}

		//translate camera to desired position.
		if (aPushCameraBackwards)
		{
			aNextPosition	=	aMattTransform.position + Vector3.up * aOffsetFromTarget.y * 1.25f + Vector3.back * aOffsetFromTarget.z * 1.5f;
			aHeight			=	1.5f;
		}
		else
		{
			aNextPosition	=	aMattTransform.position + Vector3.up * aOffsetFromTarget.y + Vector3.back * aOffsetFromTarget.z;
			aHeight			=	0.8f;
		}


		//transform values interpolation
		transform.position	=	Vector3.MoveTowards	(transform.position, aNextPosition, aCurrentFollowUpSpeed * Time.deltaTime);
		aCurrentHeight		=	Utilities.mfApproach(aHeight, aCurrentHeight, 4.0f * Time.deltaTime);

		transform.LookAt(aMattTransform.position + Vector3.up * aCurrentHeight);

	}
}
