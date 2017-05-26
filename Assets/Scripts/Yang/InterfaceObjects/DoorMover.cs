using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMover : MonoBehaviour {

	public enum DoorType {Topleft, Topright, Bottomleft, Bottomright};
	public enum CameraAngle { FortyFive, Thirty };

	public DoorType doorType = DoorType.Topleft;
	public CameraAngle cameraAngle = CameraAngle.FortyFive;

	public enum DoorStatus {Opening, Closing};
	public DoorStatus doorStatus = DoorStatus.Closing;

	Vector3 closePos;
	Vector3 openPos;

	Vector3 targetPos{
		get{
			switch(doorStatus){
			case DoorStatus.Opening:
				return openPos;
			case DoorStatus.Closing:
				return closePos;
			default:
				return closePos;
			}

		}
	}

	float doorWidth = 0f;
	float moveDist = 0f;


	public float moveSpeed = 10f;
	[Range(0.5f, 1.5f)]
	public float moveDistanceFactor = 1f;

	SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		// assume that the current pos is the closed pos
		closePos = transform.position;
		sr = GetComponentInChildren<SpriteRenderer> ();
		if(sr){
			// calculate the width of this single side door
			doorWidth = sr.bounds.size.x / 2.0f;
		}
		moveDist = doorWidth * moveDistanceFactor;


		if (cameraAngle == CameraAngle.FortyFive) {
			switch (doorType) {
			case DoorType.Topleft:
				openPos = closePos + new Vector3 (-moveDist, moveDist, 0f);
				break;
			case DoorType.Bottomright:
				openPos = closePos + new Vector3 (moveDist, -moveDist, 0f);
				break;
			case DoorType.Topright:
				openPos = closePos + new Vector3 (moveDist, moveDist, 0f);
				break;
			case DoorType.Bottomleft:
				openPos = closePos + new Vector3 (-moveDist, -moveDist, 0f);
				break;
			}
		} else {
			switch (doorType) {
			case DoorType.Topleft:
				openPos = closePos + new Vector3 (-moveDist, moveDist / 2.0f, 0f);
				break;
			case DoorType.Bottomright:
				openPos = closePos + new Vector3 (moveDist, -moveDist / 2.0f, 0f);
				break;
			case DoorType.Topright:
				openPos = closePos + new Vector3 (moveDist, moveDist / 2.0f, 0f);
				break;
			case DoorType.Bottomleft:
				openPos = closePos + new Vector3 (-moveDist, -moveDist / 2.0f, 0f);
				break;
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = 
			Vector3.Lerp (transform.position, targetPos, Time.fixedDeltaTime * moveSpeed);
		transform.position = newPos;
	}

}
