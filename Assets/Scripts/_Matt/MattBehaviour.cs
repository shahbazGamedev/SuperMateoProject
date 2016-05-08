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

	public void mpInitBehaviour()
	{
		//define component references
		aRotationHelper	=	GetComponent<ForwardRotate>();
		aRgbody			=	GetComponent<Rigidbody>();
		aCollider		=	GetComponent<Collider>();
		aMattTransform	=	transform;

		aAnimator		=	GetComponent<Animator>();
		aAudioSource	=	GetComponent<AudioSource>();
	}

	public void mpExecuteFSM()
	{
		switch (aCurrentState)
		{
		case eMattState.IDLE:
			aAnimator.SetInteger("State", 0);
			break;
		case eMattState.WALKING:
			break;
		case eMattState.RUNNING:
			aAnimator.SetInteger("State", 1);
			break;
		case eMattState.ATTACKING:
			aAnimator.SetInteger("State", 2);
			break;
		case eMattState.JUMPING:
			break;
		case eMattState.FALLING:
			break;
		case eMattState.DEATH:
			print("DEATH: " + Time.time);
			//Destroy(transform.parent.gameObject);
			break;
		}
	}
}
