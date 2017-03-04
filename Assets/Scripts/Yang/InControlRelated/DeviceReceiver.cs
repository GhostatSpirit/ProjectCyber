using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

// this script is for receiving the device assigned by
// DeviceAssigner.cs

public class DeviceReceiver : MonoBehaviour {
	
	public int playerIndex = 0;
	public Transform deviceAssigner;
	InputDevice playerDevice;

	[HideInInspector] public float someFloat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		playerDevice = deviceAssigner.
			GetComponent<DeviceAssigner>().GetPlayerDevice(playerIndex);
	}

	public InputDevice GetDevice(){
		return playerDevice;
	}
}
