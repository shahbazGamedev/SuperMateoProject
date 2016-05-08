using UnityEngine;
using System.Collections;

public class EmCubesSpawner : MonoBehaviour 
{
	//wait time for another cube to pop up
	public	int		aWaitTime;

	//game object to instantiate
	public	GameObject	aEmCubeGO;

	//reference to the last instantiated cube
	private	GameObject	aCurrentCube;

	void Start()
	{
		StartCoroutine(mcCreateCube());
	}

	IEnumerator mcCreateCube()
	{
		//wait for the cube to be ready
		yield return new WaitForSeconds(aWaitTime);

		//pop out cube!
		//create and parent this cube to its spawner
		aCurrentCube	=	(GameObject)Instantiate(aEmCubeGO, transform.position, Quaternion.identity);
		aCurrentCube.transform.SetParent(transform);

		//instantiate only if the current cube was taken and if the spawner is not busy
		while (aCurrentCube)
		{
			yield return null;
		}
		StartCoroutine(mcCreateCube());
	}
}
