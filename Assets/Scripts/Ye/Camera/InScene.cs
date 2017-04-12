using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


public class InScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<ProCamera2DTransitionsFX>().TransitionEnter();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
