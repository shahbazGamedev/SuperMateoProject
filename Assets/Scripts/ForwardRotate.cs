using UnityEngine;
using System.Collections;

public class ForwardRotate : MonoBehaviour 
{
	//how fast a gameobject rotates toward its velocity vector
	[Range(1.0f, 35.0f)]
	public	float	aRotationSpeed;

	public void mpRotateViewVector(Vector3 pVelocity)
	{
		//lock rotations on the X and Z axis
		pVelocity.y			=	0.0f;

		//enable rotation
		transform.rotation	=	Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, pVelocity, aRotationSpeed * Time.deltaTime, 0.0f));
	}
}
