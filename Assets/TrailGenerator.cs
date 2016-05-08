using UnityEngine;
using System.Collections;

public class TrailGenerator : MonoBehaviour
{
	public	Transform[]	aTrailPoints;
	private	int			aTotalPoints;

	void Start()
	{
		int lIdx 		= 	0;
		aTotalPoints	=	transform.childCount;

		aTrailPoints	=	new Transform[aTotalPoints];

		foreach (Transform lTransform in transform)
		{
			lTransform.name			=	lIdx.ToString();
			aTrailPoints[lIdx++] 	=	lTransform;

		}
	}

	void Update()
	{
		for (int i = 0; i < aTotalPoints; i++)
		{
			if ((i+1) != aTotalPoints)
				Debug.DrawLine(aTrailPoints[i].position, aTrailPoints[i+1].position);
		}
	}
}
