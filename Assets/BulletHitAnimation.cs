using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletHitAnimation : MonoBehaviour {
	HealthSystem hs;
	public float delay = 0.5f;
	public SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		hs = GetComponent<HealthSystem> ();
		sr = GetComponent<SpriteRenderer> ();
	}

	void PlayAnimation(Transform trans){
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
