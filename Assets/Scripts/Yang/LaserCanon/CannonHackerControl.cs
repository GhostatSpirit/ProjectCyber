using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CannonHackerControl : MonoBehaviour {

	public InputDevice myInputDevice;

	public bool pressedExit{
		get{
			if (myInputDevice == null)
				return false;
			else
				return myInputDevice.Action2.IsPressed;
		}
	}

	public bool pressedShoot{
		get{
			if (myInputDevice == null)
				return false;
			else
				return myInputDevice.RightTrigger.IsPressed;
		}
	}

	public Vector3 direction{
		get{
			if (myInputDevice == null)
				return Vector3.zero;
			
			float horizontal = myInputDevice.LeftStickX;
			float vertical = myInputDevice.LeftStickY;

			Vector3 _direction = new Vector3 (horizontal, vertical, 0f);

			if (_direction.magnitude > 1f) {
				_direction.Normalize ();
			}

			return _direction;
		}
	}

	// Use this for initialization
	void Start () {
		if(GetComponent<DeviceReceiver>())
			myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
	}
	
	// Update is called once per frame
	void Update () {
		myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
	}
}
