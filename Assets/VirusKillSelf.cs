using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusKillSelf : MonoBehaviour {
	HealthSystem hs;

	void Start(){
		hs = GetComponent<HealthSystem>();
	}

	// Use this for initialization
//	void OnCollision2D(Collision2D coll){
//		ObjectIdentity oi = coll.transform.GetComponent<ObjectIdentity> ();
//
//		if(!oi || oi.objType == ObjectType.Wall){
//			hs.InstantDead ();
//		}
//	}
}
