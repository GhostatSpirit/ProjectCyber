using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLaserUpdate : MonoBehaviour {
	LineRenderer renderer;
	// Use this for initialization
	void Start () {
		renderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(renderer){
			// draw a aim laser between transform.pos and parent.transform.pos
			renderer.useWorldSpace = true;
			Vector3[] positions = new Vector3[2];
			positions [0] = transform.position;
			positions [1] = transform.parent.position;
			renderer.SetPositions (positions);
		}
	}
}
