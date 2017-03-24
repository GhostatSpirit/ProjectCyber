using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour {

	Transform lockedTargetTransform;
	ChaseTarget chaser;

	public ObjectType targetType = ObjectType.None;

	void Start(){
		lockedTargetTransform = null;
		chaser = transform.parent.GetComponent<ChaseTarget> ();
		if(chaser == null){
			Debug.Log("FindTarget: parent missing ChaseTarget");
		}
	}
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other){
		if(lockedTargetTransform == null && TypeMatches(other.transform)){
			//Debug.Log ("entering!");
			// enemy enters our sight, let's lock it
			lockedTargetTransform = other.transform;
			if(chaser != null){
				chaser.target = lockedTargetTransform;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(lockedTargetTransform == null && TypeMatches(other.transform)){
			//Debug.Log ("entering!");
			// enemy enters our sight, let's lock it
			lockedTargetTransform = other.transform;
			if(chaser != null){
				chaser.target = lockedTargetTransform;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.transform == lockedTargetTransform){
			//Debug.Log ("leaving!");
			// target leaving our sight, reset everything
			lockedTargetTransform = null;
			if(chaser != null){
				//Debug.Log("set null!");
				chaser.target = null;
			}
		}
	}

	void Update(){
		if(lockedTargetTransform == null){
			return;
		}

		if(lockedTargetTransform.gameObject.layer == this.gameObject.layer){
			//Debug.Log ("target becomes friend");
			lockedTargetTransform = null;
			if(chaser != null){
				//Debug.Log("set null!");
				chaser.target = null;
			}
		}
	}

	bool TypeMatches(Transform trans){
		ObjectIdentity oi = trans.GetComponent<ObjectIdentity> ();
		if(oi == null){
			return false;
		}
		if(oi.objType == targetType){
			return true;
		}
		else {
			return false;
		}
	}
}
