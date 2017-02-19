using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeflect : MonoBehaviour {
	Rigidbody2D myRigidbody;
	//public AudioClip reflectSound;
	[HideInInspector] public float initialVelocity;
	//AudioSource myAudioSource;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		//myAudioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {

    }

    void OnCollisionEnter2D(Collision2D coll){
		ContactPoint2D contact = coll.contacts [0];
		Debug.Log ("Hit!");
		//Debug.DrawRay (contact.point, contact.normal, Color.white);
		// play the reflect sound
		//if (coll.transform.tag != "Player") {
			//myAudioSource.PlayOneShot (reflectSound);
		//}
		Vector2 newDirection = Vector2.Reflect (transform.up, contact.normal);
		transform.up = newDirection.normalized;
		myRigidbody.velocity = newDirection.normalized * initialVelocity;

		// draw a special kind of light at the contact point

	}

}
