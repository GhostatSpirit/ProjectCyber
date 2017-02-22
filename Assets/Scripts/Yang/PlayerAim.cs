using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerAim : MonoBehaviour {
	// replaced with InControl
	//public string horizontalAxisName = "Horizontal";
	//public string verticalAxisName = "Vertical";

	public enum Stick {LEFTSTICK, RIGHTSTICK};

	public Stick aimStick = Stick.RIGHTSTICK;
	public int playerIndex = 0;
	public Transform deviceAssigner;
	InputDevice myInputDevice;

	Vector2 moveVector;
	public float radialDeadZone = 0.1f;

	float angularVelocity = 8f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		myInputDevice = deviceAssigner.
			GetComponent<DeviceAssigner>().GetPlayerDevice(playerIndex);

		if(myInputDevice == null){
			return;
		}

		float horizontal = 0f, vertical = 0f;
		// get the axis values, construct a vector and normalize it
		if(aimStick == Stick.LEFTSTICK){
			horizontal = myInputDevice.LeftStickX;
			vertical = myInputDevice.LeftStickY;
		} else if( aimStick == Stick.RIGHTSTICK){
			horizontal = myInputDevice.RightStickX;
			vertical = myInputDevice.RightStickY;
		}


		moveVector = new Vector3(horizontal, vertical, 0f);


		if (moveVector.magnitude > radialDeadZone) {
			var currentRot = Quaternion.LookRotation (Vector3.forward, moveVector);
			transform.rotation = Quaternion.Lerp (transform.rotation, currentRot,
				Time.deltaTime * angularVelocity);
		}
//		if(moveVector.magnitude != 0f){
//			transform.up = moveVector;
//		}
	}
}
