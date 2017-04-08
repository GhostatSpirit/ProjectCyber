using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCollide : MonoBehaviour {

	public float enableColliderDelay = 0.1f;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (enableColliderDelay);
		GetComponent<Collider2D> ().enabled = true;
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void OnCollisionExit2D(){
		GetComponent<Collider2D> ().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
