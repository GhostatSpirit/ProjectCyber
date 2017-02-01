using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour {

	public float deathDelay = 1.0f;

	bool isDead = false;

	public void LetDead(){
		if (!isDead) {
			Invoke ("Kill", deathDelay);
			// make sure that this object would be killed only once
			isDead = true;
		}
	}

	void Kill(){
		Destroy (transform.gameObject);
	}

}
