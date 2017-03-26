using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusPosManager : MonoBehaviour {

	// the max number of virus this object could control at a time
	public int maxVirusCount = 4;

	// the distance that the virus children would keep with this object
	public float spreadRadius = 3f;

	// the angle that the virus would spread
	[Range(0, 360)]
	public float intervalAngle = 20f;

	public Vector3 facing;

	// the amount of virus that is alive for now
	int virusCount = 0;

	List<Transform> virusList;

	// Use this for initialization
	void Start () {
		virusList = new List<Transform> ();
		virusList.Clear ();
		facing = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		virusList.Clear ();
		// get all childs which is a virus
		foreach(Transform child in transform){
			// does if have a objectIdentity and the identity is virus?
			ObjectIdentity oi = child.GetComponent<ObjectIdentity> ();
			if(oi && oi.objType == ObjectType.Virus){
				// append it to the virus list
				virusList.Add(child);
			}
		}
		virusCount = virusList.Count;
		if(virusCount == 0){
			return;
		}
		// set the facing vector
		if(facing == Vector3.zero){
			facing = transform.up;
		}
		facing.Normalize ();

		// set the pos and rotation for each enemy
		float startAngle = 0f;
		if (virusCount != 1) {
			startAngle = - intervalAngle * (virusCount - 1) / 2.0f;
		}
				
		// initialize rotCursor
		Quaternion rotCursor = Quaternion.FromToRotation(Vector3.up, facing);
		rotCursor *= Quaternion.Euler (0f, 0f, startAngle);

		//Debug.Log (rotCursor.eulerAngles);

		foreach(Transform virus in virusList){
			VirusPosReceiver receiver = virus.GetComponent<VirusPosReceiver> ();
			if(receiver){
				// set the desiredRotation
				receiver.desiredRot = rotCursor;
				// set the desiredPosition
				Vector3 dirToVirus = rotCursor * Vector3.up;
				dirToVirus.Normalize ();
				Vector3 newPos = transform.position + dirToVirus * spreadRadius;
				//Debug.Log (newPos);
				receiver.desiredPos = newPos;
			}

			rotCursor = Quaternion.AngleAxis (intervalAngle, Vector3.forward) * rotCursor;
		}

	}



}
