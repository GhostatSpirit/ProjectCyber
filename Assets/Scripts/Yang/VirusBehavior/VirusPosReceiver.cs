using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusPosReceiver : MonoBehaviour {
	public Vector3 desiredPos;
	public Quaternion desiredRot;

	public float moveSpeed = 3f;
	public float rotSpeed = 80f;

	public bool instantRot = false;
	public bool instantMove = false;

	public bool usingStraight = false;

	FieldOfView fov;

	float rotSpeedFactor;
	Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {
		fov = GetComponent<FieldOfView> ();

		if(transform.parent != null){
			desiredPos = transform.parent.position;
		}
		else{
			desiredPos = Vector3.zero;
		}
		desiredRot = Quaternion.Euler(0f, 0f, 0f);

		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		Vector2 point2Desired = (Vector2)desiredPos - (Vector2)transform.position;
		point2Desired.Normalize ();

		// rotSpeedFactor = Vector3.Cross (point2Self, transform.up).z;
		rotSpeedFactor = -  Vector3.Cross (transform.up, point2Desired).z;
		if(rotSpeedFactor < 0f){
			rotSpeedFactor = -rotSpeedFactor;
		}
	}

	void FixedUpdate(){
		// set rotation
		// float finalRotSpeed = rotSpeed * rotSpeedFactor * Time.fixedDeltaTime;
		if (!usingStraight) {
			if (instantRot) {
				transform.rotation = desiredRot;
			} else {
				transform.rotation = 
					Quaternion.Lerp (transform.rotation, desiredRot, Time.fixedDeltaTime * rotSpeed);
				//transform.rotation =
				//	Quaternion.RotateTowards (transform.rotation, desiredRot, finalRotSpeed);
			}
		}
		else {
			if(fov){
				Quaternion targetRot;
				if (instantRot) {
					targetRot = desiredRot;
				} else {
					targetRot = 
						Quaternion.Lerp (transform.rotation, desiredRot, Time.fixedDeltaTime * rotSpeed);
					//transform.rotation =
					//	Quaternion.RotateTowards (transform.rotation, desiredRot, finalRotSpeed);
				}
				Vector3 newFacing = targetRot * Vector3.up;
				fov.facing = newFacing;
			}
		}

		// set position
		Vector3 newPos;
		if (instantMove) {
			newPos = desiredPos;
		} else {
			newPos = 
				Vector3.Lerp (transform.position, desiredPos, Time.fixedDeltaTime * moveSpeed);
		}
		if(myRigidbody){
			myRigidbody.MovePosition (newPos);
		}
		else{
			transform.position = newPos;
		}

//		if(myRigidbody){
//			myRigidbody.MovePosition (newPos);
//		}
//		else{
//			transform.position = newPos;
//		}
	}

}
