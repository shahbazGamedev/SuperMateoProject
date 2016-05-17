using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour 
{
	//trajectory points
	private	Transform[]	aPointCollection;


	private	int			aTotalPoints;

	//min distance tolerance value
	public	float		aMinDistanceToPoint;
	//next point to move towards
	private	int			aCurrentPoint;

	//physics variables
	[SerializeField]
	private	Vector3		aGoalVelocity;
	[SerializeField]
	private	Vector3		aVelocity;
	public	float		aSpeed;
	public	float		aAcceleration;

	//wait time before the platform starts moving again
	public	int			aWaitTime;

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.transform.tag == "Matt")
		{
			if (Vector3.Dot(aVelocity.normalized, Vector3.down) > 0.5f)
			{
				//Reverse direction so platform does not squash Matt!s
				aCurrentPoint	=	++aCurrentPoint % aTotalPoints;
			}
		}
	}

	void OnCollisionEnter(Collision pOther)
	{
		if (pOther.transform.tag == "Matt")
		{
			//parent Matt whenever he steps into the platform
			pOther.transform.parent.SetParent(transform);
		}
	}
	void OnCollisionExit(Collision pOther)
	{
		if (pOther.transform.tag == "Matt")
		{
			//unparent Matt whenever he steps out the platform
			pOther.transform.parent.SetParent(null);
		}
	}

	void Start()
	{
		Transform	lTrajPoints	=	transform.parent.FindChild("TrajectoryPoints");
		int 		lCurrentIdx	=	0;

		aTotalPoints			=	lTrajPoints.childCount;
		aPointCollection		=	new Transform[aTotalPoints];
		aCurrentPoint			=	0;

		foreach (Transform lTransform in lTrajPoints)
		{
			aPointCollection[lCurrentIdx++]	=	lTransform;
		}

		StartCoroutine(mcMovePlatform());
	}

	IEnumerator mcMovePlatform()
	{
		while ((aPointCollection[aCurrentPoint].position - transform.position).magnitude > aMinDistanceToPoint)
		{
			//update platform position
			aGoalVelocity	=	(aPointCollection[aCurrentPoint].position - transform.position).normalized * aSpeed;

			aVelocity.x		=	Utilities.mfApproach(aGoalVelocity.x, aVelocity.x, aAcceleration);
			aVelocity.y		=	Utilities.mfApproach(aGoalVelocity.y, aVelocity.y, aAcceleration);
			aVelocity.z		=	Utilities.mfApproach(aGoalVelocity.z, aVelocity.z, aAcceleration);

			transform.position	+=	aVelocity * Time.deltaTime;

			yield return null;
		}

		//go to the next trajectory point and wait
		aVelocity		=	Vector3.zero;
		aCurrentPoint	=	++aCurrentPoint % aTotalPoints;

		//wait for the cube to be ready
		yield return new WaitForSeconds(aWaitTime);

		StartCoroutine(mcMovePlatform());
	}	
}
