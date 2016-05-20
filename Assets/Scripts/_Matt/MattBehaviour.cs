using UnityEngine;
using System.Collections;

public class MattBehaviour : MonoBehaviour 
{
	public	eMattState		aCurrentState;

	//reference to the rotation view script
	protected	ForwardRotate	aRotationHelper;

	//reference to attached components
	protected	Rigidbody		aRgbody;
	protected	Collider		aCollider;

	protected	Transform		aMattTransform;

	[HideInInspector]
	public		Animator		aAnimator;
	[HideInInspector]
	public		AudioSource		aAudioSource;
	[HideInInspector]
	public		Transform		aMattCamera;


	public		AudioClip		aGrassLandingSFX;
	public		AudioClip		aStoneLandingSFX;

	public void mpInitBehaviour()
	{
		//define component references
		aRotationHelper	=	GetComponent<ForwardRotate>();
		aRgbody			=	GetComponent<Rigidbody>();
		aCollider		=	GetComponent<Collider>();
		aMattTransform	=	transform;

		aAnimator		=	GetComponent<Animator>();
		aAudioSource	=	GetComponent<AudioSource>();
		aMattCamera		=	transform.root.FindChild("Camera");
	}

	public void mpExecuteFSM()
	{
		switch (aCurrentState)
		{
		case eMattState.IDLE:
			aAnimator.SetInteger("State", 0);
			break;
		case eMattState.RUNNING:
			aAnimator.SetInteger("State", 1);
			break;
		case eMattState.ATTACKING:
			aAnimator.SetInteger("State", 2);
			break;
		case eMattState.JUMPING:
			aAnimator.SetInteger("State", 3);
			break;
		case eMattState.FALLING:
			aAnimator.SetInteger("State", 4);
			break;
		case eMattState.DEATH:
			print("DEATH: " + Time.time);
			//Destroy(transform.parent.gameObject);
			break;
		}
	}
}
