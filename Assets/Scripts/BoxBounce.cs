using UnityEngine;
using System.Collections;

public class BoxBounce : MonoBehaviour 
{
	//reward to instantiate on box-smash
	public	GameObject	aPickupGO;
	public	AudioClip	aBounce;
	public	AudioClip	aSmash;

	//number of jumps needed to smash this box
	public	int			aMaxHits;
	private	int			aCurrentHits;

	private	AudioSource		aAudioSource;
	private	BoxCollider[]	aColliders;
	private	MeshRenderer	aMeshRenderer;

	void Start()
	{
		aAudioSource	=	GetComponent<AudioSource>();
		aMeshRenderer	=	transform.root.FindChild("BoxMesh").GetComponent<MeshRenderer>();
		aCurrentHits 	=	0;
	}

	void OnTriggerEnter(Collider pOther)
	{
		if (pOther.tag == "Matt")
		{
			//check if Matt is falling down when hitting the box
			if (Vector3.Dot(Vector3.down, pOther.transform.position - transform.position) < -0.8f)
			{
				aAudioSource.PlayOneShot(aBounce);

				//make Matt bounce up
				pOther.gameObject.GetComponent<MattManager>().mpMakeMattBounce(16.0f);

				if (++aCurrentHits >= aMaxHits)
				{
					//smash this box by hiding it first
					aColliders	=	transform.root.GetComponentsInChildren<BoxCollider>();
					foreach (BoxCollider lBox in aColliders)
					{
						lBox.enabled	=	false;
					}

					aMeshRenderer.enabled	=	false;

					//play smash sound
					aAudioSource.PlayOneShot(aSmash);

					//destroy after sfx is finished
					Destroy(transform.root.gameObject, aSmash.length);

					//execute rng to determine if this box should pop a pickup
					if (Utilities.mfExecuteRNG(50))
						Instantiate(aPickupGO, transform.position, Quaternion.identity);
				}
			}
			
		}
	}
}
