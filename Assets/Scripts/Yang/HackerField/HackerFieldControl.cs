using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class HackerFieldControl : MonoBehaviour {

	InputDevice myInputDevice;
	Animator animator;

	[ReadOnly]public bool chargeCanceled = false;

	// Use this for initialization
	void Start () {
		myInputDevice = GetComponentInParent<DeviceReceiver>().GetDevice();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		// update input device every frame
		myInputDevice = GetComponentInParent<DeviceReceiver>().GetDevice();

		// Debug.Log (this.chargeCanceled);
	}

	public bool wasButtonPressed{
		get{
			if (myInputDevice != null && myInputDevice.Action1.WasPressed) {
				return true;
			} else {
				return false;
			}
		}
	}

	public bool isButtonReleased{
		get{
			if (myInputDevice == null || !myInputDevice.Action1.IsPressed) {
				return true;
			} else {
				return false;
			}
		}
	}

	public void ReachedFirstFrame(){
		if(chargeCanceled){
			animator.SetTrigger ("exitCharge");
		}
		chargeCanceled = false;
	}

}
