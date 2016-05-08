using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour 
{
	public	GameObject		aTarget;
	[Range(0.8f, 0.99f)]
	public	float			aAttackFrontDotValue;

	public	eEnemyAIState	aCurrentAIState;
	public	eEnemyAnimState	aCurrentAnimState;

	private ForwardRotate	aRotationHelper;
	private	Rigidbody		aRgbody;

	protected	Vector3		aRightVector;
	private	float			aRightDotValue;

	protected	Animator		aAnimator;
	protected	AudioSource		aAudioSource;

	public		AudioClip		aSpotLaughSFX;
	public		AudioClip		aAttackSFX;

	//Combat variables
	public	GameObject	aDefeatPuff;
	public	int			aStunTime;

	public void mpInitBehaviour()
	{
		aRotationHelper		=	GetComponent<ForwardRotate>();	
		aRgbody				=	GetComponent<Rigidbody>();
		aAnimator			=	GetComponent<Animator>();
		aAudioSource		=	GetComponent<AudioSource>();
	}

	public void mpUpdateRightVector(Vector3 pTargetPosition)
	{
		aRightVector	=	Vector3.Cross(Vector3.up, transform.forward);
		aRightDotValue	=	Vector3.Dot(aRightVector, (pTargetPosition - transform.position).normalized);
	}

	public ForwardRotate rotationHelper
	{
		get { return aRotationHelper;}
	}

	public Rigidbody rgbody
	{
		get { return aRgbody;}
	}

	public Animator animator
	{
		get { return aAnimator;}
	}

	public AudioSource audioSource
	{
		get { return aAudioSource;}
	}

	public float rightDotValue
	{
		get { return aRightDotValue;}
	}
}
