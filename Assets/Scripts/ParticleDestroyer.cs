using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour 
{
	ParticleSystem	aSystem;

	void Start()
	{
		aSystem	=	GetComponent<ParticleSystem>();
	}

	void Update()
	{
		if (!aSystem.IsAlive())
		{
			Destroy(gameObject);
		}
	}
}
