using UnityEngine;
using System.Collections;

public class MattManager : MattMATEA  
{
	void Awake () 
	{
		mpInitBehaviour();
		mpInitStatus(100,50,25,8);
		mpInitPhysicsEngine();
		mpInitMatea();
	}

	void Update()
	{
		mpExecuteFSM();
		mpUpdateMatea();
	}

}
