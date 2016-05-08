using UnityEngine;
using System.Collections;

public class MattPhysics : MattStatus
{
	//physics variables
	private	Vector3		aVelocity;
	private Vector3		aGoalVelocity;

	[Range(8.0f, 20.0f)]
	public	float		aMaxSpeed;
	private	float		aCurrentSpeed;

	private	float		aMovementSpeed;

	[Range(1.5f, 8.0f)]
	public	float		aMaxAcceleration; 

	[Range(0.8f, 3.0f)]
	public	float		aGravity;

	[Range(18.0f, 20.0f)]
	public	float		aJumpHeight;
	private	float 		aDistToGround;

	private	bool		aMattCanMove	=	true;

	private	RaycastHit	aHit;

	public void mpInitPhysicsEngine() 
	{
		aDistToGround		=	aCollider.bounds.extents.y;

		//set values to their "sweet spots"
		aMaxSpeed			=	8.0f;
		aMaxAcceleration	=	1.5f;
		aGravity			=	0.8f;
		aJumpHeight			=	18.0f;
	}

	void FixedUpdate()
	{
		if (aMattCanMove)
		{
			//update speed according to status bonus or penalty
			if (aBiorhythm == eStatus.ALTERED)
			{
				aCurrentSpeed	=	aStatusSpeed.aCurrent;
			}
			else
			{
				aCurrentSpeed	=	aMaxSpeed;
			}

			//handle input
			aGoalVelocity.x	=	Input.GetAxisRaw("Horizontal");
			aGoalVelocity.z	=	Input.GetAxisRaw("Vertical");

			//get and scale direction
			aGoalVelocity	=	aGoalVelocity.normalized * aCurrentSpeed;

			//cache rigidbody velocity
			aVelocity		=	aRgbody.velocity;

				//velocity interpolation
				aVelocity.x		=	Utilities.mfApproach(aGoalVelocity.x, aVelocity.x, aMaxAcceleration);
				aVelocity.z		=	Utilities.mfApproach(aGoalVelocity.z, aVelocity.z, aMaxAcceleration);

				//handle jumping
				if (mfMattIsGrounded())
				{
					//jumping
					if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetButtonDown("A"))
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

				if (aMovementSpeed >= aCurrentSpeed * 0.5f)
				{
					aCurrentState	=	eMattState.RUNNING;
				}
				else if (aMovementSpeed > 0.0f)
				{
					aCurrentState	=	eMattState.WALKING;
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
		if (Physics.Raycast(transform.position, Vector3.down, out aHit, aDistToGround + 0.1f))
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
		return (aVelocity.magnitude >= 0.5f * aCurrentSpeed);
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
