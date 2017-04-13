using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayer : MonoBehaviour {

	public float thrust = 20f;

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionEnter2D(Collision2D coll){
//		Debug.Log (coll.transform);
		ObjectIdentity oi = coll.transform.GetComponent<ObjectIdentity> ();
		Rigidbody2D body = coll.transform.GetComponent<Rigidbody2D> ();
		if(oi && isPlayer(oi.objType)){
			// we collides a player, push it away from the boss

			Vector3 contact = coll.contacts [0].point;
			Vector3 dir = contact - transform.position;
			dir.Normalize ();

//			Debug.Log(dir);

			if(body){
//				Debug.Log ("add force");
				// coll.transform.GetComponent<PlayerMovement> ().moveEnabled = false;
				body.AddForceAtPosition (dir * thrust, contact, ForceMode2D.Impulse);
			}
		}
	}

	bool isPlayer(ObjectType type){
		if(type == ObjectType.AI || type == ObjectType.Hacker){
			return true;
		} else{
			return false;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
