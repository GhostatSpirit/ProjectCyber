using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class ChaseTarget : MonoBehaviour {


	public Transform target;
	public float rotationSpeed = 90f;
	public float moveSpeed = 20f;

	Rigidbody2D myRigidbody;
	float rotSpeedFactor;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null){
			rotSpeedFactor = 0f;
			return;		// cannot find the target transform, wait for next frame
		}

		Vector2 point2Self = (Vector2)transform.position - (Vector2)target.transform.position;
		point2Self.Normalize ();

		// rotSpeedFactor = Vector3.Cross (point2Self, transform.up).z;
		rotSpeedFactor = -  Vector3.Cross (transform.up, point2Self).z;
		//Debug.Log (rotSpeedFactor);


//		float zAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90f;
//		Quaternion desiredRot = Quaternion.Euler (0f, 0f, zAngle);
//		transform.rotation = 
//			Quaternion.RotateTowards (transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);


	}

	void FixedUpdate(){
		myRigidbody.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
		//Debug.Log (transform.up);
		myRigidbody.angularVelocity = rotationSpeed * rotSpeedFactor;
	}
}
