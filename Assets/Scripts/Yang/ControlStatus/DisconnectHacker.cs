using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectHacker : MonoBehaviour {

	public float waitSeconds = 1f;
	public float disconnectDistance = 1f;

	public bool onlyCheckDistance = false;

	ControlStatus _cs;
	ControlStatus cs{
		get{
			if (!_cs)
				_cs = GetComponentInChildren<ControlStatus> ();
			return _cs;
		}
	}

	FieldOfView _fov;
	FieldOfView fov{
		get{
			if (!_fov)
				_fov = GetComponentInChildren<FieldOfView> ();
			return _fov;
		}
	}

	Transform _self;
	Transform self{
		get{
			if(_self == null){
				_self = transform;
				ControlLineNode node = GetComponentInChildren<ControlLineNode> ();
				if (node)
					_self = node.transform;
			}
			return _self;
		}
	}


	[ReadOnly]public bool canDisconnect = false;
	Coroutine disconnectCoroutine;

	public void StartDisconnectTimer(Transform objTrans){
		if (disconnectCoroutine == null) {
			disconnectCoroutine = StartCoroutine (disconnectTimerIE (waitSeconds));
		}
	}

	public void StopDisconnectTimer(Transform objTrans){
		canDisconnect = false;
		if (disconnectCoroutine != null) {
			StopCoroutine (disconnectCoroutine);
		}
	}

	IEnumerator disconnectTimerIE(float waitSeconds){
		yield return new WaitForSeconds (waitSeconds);
		canDisconnect = true;
		disconnectCoroutine = null;
	}

	// Use this for initialization
	void Start () {
		if (cs) {
//			Debug.Log ("action added");
			cs.OnLinkedByPlayer += StartDisconnectTimer;
			cs.OnCutByEnemy += StopDisconnectTimer;
		}

	}


	
	// Update is called once per frame
	void Update () {
		if(cs.controller == Controller.Hacker && canDisconnect){
//			Debug.Log ("checking hacker");
			// check if the target object is out of disconnect
			bool hackerInSight = false;
			if(fov){
				hackerInSight = fov.CheckTarget (cs.Hacker);
			}
			if(onlyCheckDistance){
				hackerInSight = true;
			}
			float dist = Vector3.Distance (cs.Hacker.position, self.position);
//			Debug.Log (dist);
			if(!hackerInSight || dist > disconnectDistance){
				cs.controller = Controller.None;
				canDisconnect = false;
			}
		}
	}
}
