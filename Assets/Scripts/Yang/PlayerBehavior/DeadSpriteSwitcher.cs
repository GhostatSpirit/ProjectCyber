using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSpriteSwitcher : MonoBehaviour {
	public Sprite deadSprite;

	HealthSystem hs;
	SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		hs = GetComponent<HealthSystem> ();
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(deadSprite == null){
			return;
		}

		if(hs.IsDead() && sr.sprite != deadSprite){
			sr.sprite = deadSprite;
		}
	}
}
