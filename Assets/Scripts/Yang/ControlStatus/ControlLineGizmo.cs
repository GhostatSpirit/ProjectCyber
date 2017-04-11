using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ControlLineGizmo : MonoBehaviour {
	ControlStatus cs;

	// Use this for initialization
	void Start () {
		cs = GetComponent<ControlStatus> ();
	}
	
	void OnDrawGizmos() {
		cs = GetComponent<ControlStatus> ();
		if (cs == null) {
			return;
		}
		switch(cs.controller){
		case Controller.Boss:
			{
				Gizmos.color = Color.red;
				Transform target = cs.Boss;
				if(!target){
					return;
				}
				ControlLineNode node = cs.Boss.GetComponentInChildren<ControlLineNode> ();
				if (node) {
					target = node.transform;
				}
				Gizmos.DrawLine (transform.position, target.position);
				break;
			}
		case Controller.Hacker:
			{
				Gizmos.color = Color.cyan;
				Transform target = cs.Hacker;
				if(!target){
					return;
				}
				ControlLineNode node = cs.Boss.GetComponentInChildren<ControlLineNode> ();
				if (node) {
					target = node.transform;
				}
				Gizmos.DrawLine (transform.position, target.position);
				break;
			}
		}

	}
}
