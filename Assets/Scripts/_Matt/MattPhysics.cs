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

	private	bool		aMattCanMove	=	true;

	private	RaycastHit	aHit;

	public void mpInitPhysicsEngine() 
	{
		//set values to their "sweet spots"
		aGravity	=	0.8f;
		aJumpHeight	=	18.0f;
	}

	void FixedUpdate()
	{
		if (aMattCanMove)
		{
			//handle input
			aGoalVelocity.x	=	Input.GetAxisRaw("leftJoystickX");
			aGoalVelocity.z	=	Input.GetAxisRaw("leftJoystickY");

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
			if (aHit.transform.tag == "Enemy" || aHit.transform.tag == "DeadVolume")
			{
				return false;
			}
			else
			{
				if (aCurrentState == eMattState.FALLING)
				{
					aCurrentState	=	eMattState.IDLE;
				}
				return true;
			}
		}

		//WARNING: triggers can be detected and may return true
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
		aVelocity = aRgbody.velocity;
			aVelocity.y = pBounceHeight;
		aRgbody.velocity = aVelocity;
	}

	public void mpDisableMattMovement()
	{
		aMattCanMove		=	false;
		aRgbody.velocity	=	Vector3.zero;
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
