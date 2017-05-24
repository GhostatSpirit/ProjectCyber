using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class ChaseTarget : MonoBehaviour {
	// the ONE specific this object would chase at a specific moment
	public Transform target;

	public float rotationSpeed = 90f;
	public float moveSpeed = 20f;

	Rigidbody2D myRigidbody;
	float rotSpeedFactor;

	public bool constantRotation = false;

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
		if(rotSpeedFactor < 0f){
			rotSpeedFactor = -rotSpeedFactor;
		}
		//Debug.Log (rotSpeedFactor);


//		float zAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90f;
//		Quaternion desiredRot = Quaternion.Euler (0f, 0f, zAngle);
//		transform.rotation = 
//			Quaternion.RotateTowards (transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);


	}

	void FixedUpdate(){
		myRigidbody.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;

		if(target == null){
			return;
		}
		// update rotation of this object
		Vector2 point2Target = (Vector2)target.transform.position - (Vector2)transform.position;
		point2Target.Normalize ();
		float zAngle = Mathf.Atan2 (point2Target.y, point2Target.x) * Mathf.Rad2Deg - 90f;
		Quaternion desiredRot = Quaternion.Euler (0f, 0f, zAngle);

		float finalRotSpeed;
		// float finalRotSpeed = rotationSpeed * rotSpeedFactor * Time.fixedDeltaTime;

		if(constantRotation){
			finalRotSpeed = rotationSpeed * Time.fixedDeltaTime;
		} else{
			finalRotSpeed = rotationSpeed * rotSpeedFactor * Time.fixedDeltaTime;
		}

		transform.rotation =
			Quaternion.RotateTowards (transform.rotation, desiredRot, finalRotSpeed);

//		Quaternion finalRot = 
//			Quaternion.RotateTowards (transform.rotation, desiredRot, finalRotSpeed);
//		myRigidbody.MoveRotation (finalRot.eulerAngles.z);
//

		//myRigidbody.angularVelocity = rotationSpeed * rotSpeedFactor;
	}
}
