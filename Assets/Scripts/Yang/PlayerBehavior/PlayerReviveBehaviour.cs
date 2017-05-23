using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReviveBehaviour : MonoBehaviour {
	PlayerControl control;
	HealthSystem hs;
	Rigidbody2D body;
	// Use this for initialization
	void Start () {
		control = GetComponent<PlayerControl> ();
		hs = GetComponent<HealthSystem> ();
		body = GetComponent<Rigidbody2D> ();

		if(control && hs){
			hs.OnObjectDead += StopControl;

			hs.OnObjectRevive += StartControl;
			hs.OnObjectRevive += StartSimulate;
		}
	}

	void StopControl(Transform trans){
		control.canControl = false;
	}

	void StartControl(Transform trans){
		control.canControl = true;
	}

	void StartSimulate(Transform trans){
		body.simulated = true;
		body.velocity = Vector3.zero;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
