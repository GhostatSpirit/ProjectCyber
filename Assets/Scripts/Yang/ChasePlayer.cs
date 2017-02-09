using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class ChasePlayer : MonoBehaviour {

	public Transform target;
	public float rotationSpeed = 90f;
	public float moveSpeed = 20f;

	Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null){
			return;		// cannot find the target transform, wait for next frame
		}

		Vector3 dir = target.position - transform.position;
		dir.Normalize ();
		float zAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90f;
		Quaternion desiredRot = Quaternion.Euler (0f, 0f, zAngle);
		transform.rotation = 
			Quaternion.RotateTowards (transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
	}

	void FixedUpdate(){
		myRigidbody.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
	}
}
