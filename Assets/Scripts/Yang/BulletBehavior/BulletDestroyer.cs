using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour {

    public int maxDeflectCount = 5;
    public float maxDistance = 100f;
	//public Transform shotFrom;

    private Vector2 initialPos;
//	private int deflectCount = 0;

	// Use this for initialization
	void Start () {
        initialPos = transform.position;
	}
	
	void FixedUpdate () {
		float dist = Vector2.Distance (initialPos, transform.position);
		if(dist >= maxDistance){
			DestroyBullet ();
		}
	}

    void OnCollisionEnter2D(Collision2D coll) {
		if(coll.transform.tag != "Enemy"){
			Destroy (this.gameObject);
		}
    }

	void DestroyBullet(){
		// call the shooter player object that this bullet will be destroyed
		Destroy (transform.gameObject);
	}

}
