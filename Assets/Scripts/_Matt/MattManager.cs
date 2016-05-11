using UnityEngine;
using System.Collections;

public class MattManager : MattMATEA  
{
	void Awake () 
	{
		mpInitBehaviour();
		mpInitStatus(100,50,25, 8.0f, 1.5f);
		mpInitPhysicsEngine();
		mpInitMatea();
	}

	void Update()
	{
		mpExecuteFSM();
		mpUpdateMatea();
	}

}
