using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusPosReceiver : MonoBehaviour {
	public Vector3 desiredPos;
	public Quaternion desiredRot;

	public float moveSpeed = 3f;
	public float rotSpeed = 80f;

	public bool instantRot = false;

	public bool usingStraight = false;

	float rotSpeedFactor;
	Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {
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
		if(instantRot){
			transform.rotation = desiredRot;
		}
		else{
			transform.rotation = 
				Quaternion.Lerp (transform.rotation, desiredRot, Time.fixedDeltaTime * rotSpeed);
			//transform.rotation =
			//	Quaternion.RotateTowards (transform.rotation, desiredRot, finalRotSpeed);
		}

		// set position

		


		if(usingStraight){
			Vector3 newPos = 
				Vector3.Lerp (transform.parent.position, desiredPos, Time.fixedDeltaTime * moveSpeed);
			transform.parent.position = newPos;
		} else{
			Vector3 newPos = 
				Vector3.Lerp (transform.position, desiredPos, Time.fixedDeltaTime * moveSpeed);
			if(myRigidbody){
				myRigidbody.MovePosition (newPos);
			}
			else{
				transform.position = newPos;
			}
		}
//		if(myRigidbody){
//			myRigidbody.MovePosition (newPos);
//		}
//		else{
//			transform.position = newPos;
//		}
	}

}
