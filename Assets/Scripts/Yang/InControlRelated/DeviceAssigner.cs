using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InControl;
public class DeviceAssigner : MonoBehaviour {

	public int playerCount = 2;
	// array for storing devices to be assigned to players
	[HideInInspector] public InputDevice[] playerDevices;
	int attachedDeviceCount = 0;

	// Use this for initialization
	void Start () {

		InputManager.OnDeviceAttached += DeviceAttached;
		InputManager.OnDeviceDetached += DeviceDetached;

		// initialize all elems in playerDevices as null
		playerDevices = new InputDevice[playerCount];
		for (int i = 0; i < playerDevices.Length; ++i){
			playerDevices[i] = null;
		}
		// try to find attached devices in InputManager.Devices
		int playerDeviceIter = 0;
		foreach(InputDevice manDevice in InputManager.Devices){
			if(manDevice.IsAttached){
				// this device is currently attached,
				// add it to playerDevices
				playerDevices [playerDeviceIter] = manDevice;
				attachedDeviceCount++;
				playerDeviceIter++;
				if(playerDeviceIter >= playerCount){
					// collected enough devices
					break;
				}
			}
		}
		Debug.Log ("Start with " + attachedDeviceCount.ToString() + " device(s) attached");
	}

	void DeviceAttached(InputDevice device){
//		Debug.Log ("Attach detected");
//		Debug.Log ("Device Name: " + device.Name);
//		Debug.Log ("Device Mata: " + device.Meta);
//		Debug.Log ("Device GUID: " + device.GUID);
//		Debug.Log ("Device Count: " + InputManager.Devices.Count.ToString());
		if(attachedDeviceCount == playerCount){
			// already have enough devices
			return;
		}
		for(int i = 0; i < playerDevices.Length; ++i){
			// find for detached device in playerDevices with
			// the same Name and Meta
			if(playerDevices[i] == null){
				continue;
			}
			if(playerDevices[i].Name == device.Name && 
				playerDevices[i].Meta == device.Meta &&
				playerDevices[i].IsAttached == false){
				// replace this detached device with the new device
				playerDevices [i] = device;
				return;
			}
		}
		//Debug.Log ("Reached here");

		// this device is not in playerDevices[] yet
		// let add this new device
		// find null device
		int nullDeviceIndex = -1; 
		for(int i = 0; i < playerDevices.Length; ++i){
			if(playerDevices[i] == null && nullDeviceIndex == -1){
				nullDeviceIndex = i;
				break;
			}
		}
		if(nullDeviceIndex != -1){
			// replace null device by this device
			//Debug.Log (nullDeviceIndex);
			playerDevices [nullDeviceIndex] = device;
			attachedDeviceCount++;
		} 
//		else if(UnattachedDeviceIndex != -1){
//			// replace unattached device by this device
//			playerDevices [UnattachedDeviceIndex] = device;
//		}

		// Debug.Log (playerDevices);
	}

	void DeviceDetached(InputDevice device){
//		Debug.Log ("Detach detected");
//		Debug.Log ("Device Name: " + device.Name);
//		Debug.Log ("Device Mata: " + device.Meta);
//		Debug.Log ("Device GUID: " + device.GUID);
		attachedDeviceCount--;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public InputDevice GetPlayerDevice(int playerIndex){
		if(playerIndex < 0 || playerIndex > playerCount - 1){
			// playerIndex out of bound
			//Debug.Log ("playerIndex out of bound: " + playerIndex.ToString());
			return null;
		}
		if (playerCount == 1) {
			return InputManager.ActiveDevice;
		} else {
			return playerDevices [playerIndex];
		}
	}
}
