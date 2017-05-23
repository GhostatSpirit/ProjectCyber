using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCollide : MonoBehaviour {
	public float enableColliderDelay = 0.1f;

	public bool changeColor = false;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (enableColliderDelay);
		GetComponent<Collider2D> ().enabled = true;
		if (changeColor) {
			GetComponent<SpriteRenderer> ().color = Color.red;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
