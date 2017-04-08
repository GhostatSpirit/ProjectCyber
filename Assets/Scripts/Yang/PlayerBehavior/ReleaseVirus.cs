using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ReleaseVirus : MonoBehaviour {

	InputDevice myInputDevice;

	// a static object
	public Transform releasedVirusParent;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
		if (myInputDevice == null) {
			return;
		}
		
		if(myInputDevice.RightBumper.IsPressed){
			ReleaseIdleVirus ();
		}
	}

	void ReleaseIdleVirus(){
		// release all idle virus
		// get all childs which is a virus
		foreach(Transform child in transform){
			// does if have a objectIdentity and the identity is virus?
			ObjectIdentity oi = child.GetComponent<ObjectIdentity> ();
			if(oi && oi.objType == ObjectType.Virus){
				// is the virus Idle?
				VirusStateControl sc = child.GetComponent<VirusStateControl> ();
				if(sc){
					if(sc.virusState == VirusStateControl.VirusState.Idle){
						// change the state to "chase"
						sc.OnChaseStart += SetReleaseParent;
						sc.OnChaseStart += DisableLineRenderer;

						sc.virusState = VirusStateControl.VirusState.Chase;

						sc.OnChaseStart -= SetReleaseParent;
						sc.OnChaseStart -= DisableLineRenderer;
					}
				}
			}
		}
	}

	public void SetReleaseParent(Transform virusTrans){
		virusTrans.parent = releasedVirusParent;
	}

	public void DisableLineRenderer(Transform virusTrans){
		LineRenderer lr = virusTrans.GetComponentInChildren<LineRenderer> ();

		if(lr){
			lr.enabled = false;
		}else{
			Debug.Log ("cannot find linerenderer");
		}
	}
}
