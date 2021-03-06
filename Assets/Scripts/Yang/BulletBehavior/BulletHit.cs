﻿// shoot various bulletse when hits an enemy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour {

	public GameObject bulletPrefab;

	//public ObjectType targetType = ObjectType.None;

	public float paralyzeTime = 2f;

	public int bulletCount = 2;
	public float deltaAngle = 8f;

	bool reproduced = false;

	[HideInInspector] public Vector3 initVelocity;

//	Rigidbody2D body;

	void Start(){
//		initVelocity = Vector3.zero;
//		body = GetComponent<Rigidbody2D> ();
	}


	void OnCollisionEnter2D(Collision2D coll){

		ObjectIdentity oi = coll.transform.GetComponentInParent<ObjectIdentity> ();
		if(oi == null){
			return;
		}
		//Debug.Log ("Bullet hits: " + oi.objType.ToString ());

		switch(oi.objType){
		case ObjectType.Virus:{
				// when the bullet hits a virus...
				if (coll.gameObject.layer == this.gameObject.layer) {
					return;
				}
				HitVirusBehaviour (coll);
				break;
			}
		case ObjectType.Robot:{
				HitRobotBehaviour (coll);
				break;
			}
		case ObjectType.Interface:{
				DefaultBehaviour (coll);
				break;
			}
		case ObjectType.LaserCannon:{
				DefaultBehaviour (coll);
				break;
			}
		case ObjectType.Roomba:{
				HitRoombaBehaviour (coll);
				break;
			}
		
		default:{
				DefaultBehaviour (coll);
				break;
			}
		}

	}

	void HitVirusBehaviour(Collision2D coll){
		if (HasControlStatus (coll.transform)) {
			// the colliding object must have control status
			ControlStatus targetCS = coll.transform.GetComponent<ControlStatus> ();

			// verify that the enemy is not controlled by anything else
			if (NotControlled (coll.transform) == false) {
				// if the enemy is controlled by something else,
				// paralyze the target
				VirusActions va = coll.transform.GetComponent<VirusActions> ();
				if(va){
					va.Paralyze (paralyzeTime);
				}
				// bullet will destroy by itself
				return;
			}

			// if the enemy is not connected to anything right now...
			if (!reproduced) {
				// the target is now acquired by the HACKER!
				targetCS.controller = Controller.Hacker;

				// modify layer so it wont collide
				coll.gameObject.layer = this.gameObject.layer;
				// stop the enemy from chasing the player
				// reproduce the bullets
				reproduced = true;

				ReproduceBullets ();
				Destroy (gameObject);
			}
		}
	}

	void DefaultBehaviour(Collision2D coll){
		//Debug.Log (coll.transform);
		ControlStatus cs = coll.transform.GetComponentInParent<ControlStatus> ();
		if(!cs){
			return;
		}
		if (NotControlled (coll.transform)){
			// it the door is not controlled by the boss...
			coll.transform.GetComponentInParent<ControlStatus> ().controller = Controller.Hacker;
		}
	}

	void HitRobotBehaviour(Collision2D coll){
		if(NotControlled(coll.transform)){
			coll.transform.GetComponent<ControlStatus> ().controller = Controller.Hacker;
		}
		else{
			coll.transform.GetComponent<Animator> ().SetTrigger ("paralyzed");
		}
	}

	void HitRoombaBehaviour(Collision2D coll){
		Transform targetTrans = coll.transform;

		ControlStatus cs = targetTrans.GetComponentInParent<ControlStatus> ();
		if(!cs){
			return;
		}
		if (NotControlled (targetTrans)){
			// it the door is not controlled by the boss...
			RoombaBehaviour roomba = targetTrans.GetComponent<RoombaBehaviour> ();
			if(roomba){
				Debug.Log (initVelocity);
				roomba.incomingVelocity = initVelocity;
			}

			targetTrans.GetComponentInParent<ControlStatus> ().controller = Controller.Hacker;
		}
			
	}
		


	void ReproduceBullets(){
		// instantiate the bullet prefabs

		float midAngleZ = transform.rotation.eulerAngles.z;
		float startAngleZ = midAngleZ - deltaAngle * (bulletCount - 1) / 2.0f;
		float endAngleZ = midAngleZ + deltaAngle * (bulletCount - 1) / 2.0f;

		for(float angleZ = startAngleZ; angleZ <= endAngleZ; angleZ += deltaAngle){
			Quaternion newBulletRot = transform.rotation;
			Vector3 shooterEuler = transform.rotation.eulerAngles;
			newBulletRot.eulerAngles = new Vector3 (shooterEuler.x, shooterEuler.y, angleZ);

			GameObject bulletObj = Instantiate (bulletPrefab, transform.position, newBulletRot) as GameObject;

			bulletObj.SetActive (true);
			bulletObj.GetComponent<ChaseTarget> ().target = null;

			// set init velocity of the bullet
			// bulletObj.GetComponent<Rigidbody2D> ().velocity = bulletObj.transform.up.normalized * initialVelocity;
			// tell the bulletObj the init velocity
			//bulletObj.GetComponent<BulletDeflect> ().initialVelocity = initialVelocity;
		}
			
	}

	// verify if a transform contains ControlStatus component
	bool HasControlStatus(Transform controllable){
		ControlStatus cs = controllable.GetComponent<ControlStatus> ();
		return (cs != null);
	}

	// verify if a transform's object is controlled
	// return false if it connot find ControlStatus on the transform
	bool NotControlled(Transform controllable){
		ControlStatus cs = controllable.GetComponentInParent<ControlStatus> ();
		if(cs == null){
			return false;
		}
		return (cs.controller == Controller.None);
	}

//	bool TypeMatches(Transform trans){
//		ObjectIdentity oi = trans.GetComponent<ObjectIdentity> ();
//		if(oi == null){
//			return false;
//		}
//		if(oi.objType == targetType){
//			return true;
//		}
//		else {
//			return false;
//		}
//	}
}
