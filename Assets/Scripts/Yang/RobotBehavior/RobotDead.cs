using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDead : MonoBehaviour {
	HealthSystem hs;
	PolyNavAgent agent;
	// Use this for initialization
	void Start () {
		hs = GetComponent<HealthSystem> ();
		agent = GetComponent<PolyNavAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(hs && agent){
			hs.OnObjectDead += (
			    (Transform trans) => {
				agent.enabled = false;
			}
			);
		}
	}
}
