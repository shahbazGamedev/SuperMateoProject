using UnityEngine;
using System.Collections;

public class MattPhysics : MattStatus
{
	//physics variables
	private	Vector3		aVelocity;
	private Vector3		aGoalVelocity;

	private	float		aMovementSpeed;

	[Range(0.8f, 3.0f)]
	public	float		aGravity;

	[Range(18.0f, 28.0f)]
	public	float		aJumpHeight;

	public	bool		aMattCanMove;
	public	bool		aMattJustRespawned;
	public	bool		aMattCanAttack;

	public	bool		aUseKeyboard;

	private	RaycastHit		aHit;
	private	WalkCyclePlayer	aWalkManager;


	public void mpInitPhysicsEngine() 
	{
		aMattCanMove		=	true;
		aMattCanAttack		=	true;
		aMattJustRespawned	=	false;
		aWalkManager		=	GetComponent<WalkCyclePlayer>();
	}

	void FixedUpdate()
	{
		if (Input.GetButtonDown("start"))
		{
			aStatusHP.aCurrent	=	aStatusHP.aBase;
		}

		if (aMattCanMove)
		{
			//handle input
			if (aUseKeyboard)
			{
				aGoalVelocity.x	=	Input.GetAxisRaw("Horizontal");
				aGoalVelocity.z	=	Input.GetAxisRaw("Vertical");
			}
			else
			{
				aGoalVelocity.x	=	Input.GetAxisRaw("leftJoystickX");
				aGoalVelocity.z	=	Input.GetAxisRaw("leftJoystickY");
			}

			//get and scale direction
			aGoalVelocity	=	aGoalVelocity.normalized * aStatusSpeed.aCurrent;

			//cache rigidbody velocity
			aVelocity		=	aRgbody.velocity;

				//velocity interpolation
				aVelocity.x	=	Utilities.mfApproach(aGoalVelocity.x, aVelocity.x, aStatusAcceleration.aCurrent);
				aVelocity.z	=	Utilities.mfApproach(aGoalVelocity.z, aVelocity.z, aStatusAcceleration.aCurrent);

				//handle jumping
				if (mfMattIsGrounded())
				{
					//jumping
					if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetButtonDown("A") )
					{
						aCurrentState	=	eMattState.JUMPING;
						aVelocity.y		=	aJumpHeight;
					}
				}
				else
				{
					//make Matt fall down
					if (aVelocity.y < 0)
					{
						aCurrentState	=	eMattState.FALLING;
					}

					aVelocity.y	-=	aGravity;
				}

				//rotate Matt towards his velocity vector
				aRotationHelper.mpRotateViewVector(aVelocity);

			//set rigidbody velocity
			aRgbody.velocity =	aVelocity;

			//evaluate animation state for movement
			if ((aCurrentState != eMattState.JUMPING) && (aCurrentState != eMattState.FALLING) && (aCurrentState != eMattState.ATTACKING))
			{
				aVelocity.y		=	0;
				aMovementSpeed	=	aVelocity.magnitude;

				if (aMovementSpeed > 0.0f)
				{
					aCurrentState	=	eMattState.RUNNING;
				}
				else
				{
					aCurrentState	=	eMattState.IDLE;
				}
			}
		}
	}

	public bool mfMattIsGrounded()
	{
		//if there is any collider below then return true
		if (Physics.Raycast(transform.position, Vector3.down, out aHit, aCollider.bounds.extents.y + 0.1f))
		{
			switch (aHit.transform.tag)
			{
			case "Grass": default:
				aWalkManager.aCurrentSurface	=	eSurfaces.GRASS; 
				break;
			case "Stone": 
				aWalkManager.aCurrentSurface	=	eSurfaces.STONE; 
				break;
			case "Enemy": case "DeadVolume":
				return false;
			}

			if (aCurrentState == eMattState.FALLING)
			{
				if (!aMattJustRespawned)
				{
					switch (aWalkManager.aCurrentSurface)
					{
					case eSurfaces.GRASS: default:
						aAudioSource.PlayOneShot(aGrassLandingSFX);
						break;
					case eSurfaces.STONE: 
						aAudioSource.PlayOneShot(aStoneLandingSFX);
						break;
					}
				}
				else
				{
					aMattJustRespawned = false;
				}

				aCurrentState	=	eMattState.IDLE;
			}
			return true;

		}

   		return false;
	}

	public bool mfMattIsRunning()
	{
		//define if Matt's speed is greater than a set value
		return (aVelocity.magnitude >= 0.5f * aStatusSpeed.aCurrent);
	}

	public void mpMakeMattBounce(float pBounceHeight)
	{
		//make Matt jump even if he is not grounded
		aVelocity 			= 	aRgbody.velocity;
		aCurrentState		=	eMattState.JUMPING;
		aVelocity.y 		= 	pBounceHeight;
		aRgbody.velocity 	= 	aVelocity;
	}

	public void mpDisableMattMovement()
	{
		aMattCanMove		=	false;
		mpStopMatt();
	}

	public void mpStopMatt()
	{
		aRgbody.velocity	=	Vector3.zero;
		aCurrentState		=	eMattState.IDLE;
	}

	public void mpEnableMattMovement()
	{
		aMattCanMove		=	true;
	}

	public float velocity_Y
	{
		get { return aVelocity.y;}
	}
}
