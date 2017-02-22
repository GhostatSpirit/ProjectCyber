using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.GetComponent<PlayerMovement>() != null){
			// target is a player
			coll.gameObject.GetComponent<PlayerMovement> ().enabled = false;
			coll.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		}
	}
}
