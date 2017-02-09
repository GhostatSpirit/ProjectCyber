using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
	public string enemyTag;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.transform.tag == enemyTag){
			GetComponent<PlayerMovement> ().moveEnabled = false;
			GetComponent<PlayerMovement> ().turnEnabled = false;
			GetComponent<SpriteRenderer> ().color = Color.red;
		}
	}
}
