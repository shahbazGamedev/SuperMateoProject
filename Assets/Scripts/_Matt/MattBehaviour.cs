using UnityEngine;
using System.Collections;

public class MattBehaviour : MonoBehaviour 
{
	public	int			aPositiveStreak;
	public	int			aNegativeStreak;

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


	public		AudioClip		aDamageSFX;
	public		AudioClip		aGrassLandingSFX;
	public		AudioClip		aStoneLandingSFX;

	public void mpResetStreaks()
	{
		aPositiveStreak	=	0;
		aNegativeStreak	=	0;
	}

	public void mpInitBehaviour()
	{
		mpResetStreaks();

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
			break;
		}
	}
}
