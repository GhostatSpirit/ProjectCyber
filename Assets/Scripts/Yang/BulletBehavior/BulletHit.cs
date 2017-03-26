// shoot various bulletse when hits an enemy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour {

	public GameObject bulletPrefab;

	public ObjectType targetType = ObjectType.None;

	public float paralyzeTime = 2f;

	public int bulletCount = 2;
	public float deltaAngle = 10f;

	bool reproduced = false;


	void OnCollisionEnter2D(Collision2D coll){
		if(TypeMatches(coll.transform) && coll.gameObject.layer != this.gameObject.layer){
			if (HasControlStatus (coll.transform)) {
				// the colliding object must have control status
				ControlStatus targetCS = coll.transform.GetComponent<ControlStatus> ();

				// verify that the enemy is not controlled by anything else
				if (NotControlled (coll.transform) == false) {
					// if the enemy is controlled by something else,
					// paralyze the target
					targetCS.Paralyze (paralyzeTime);
					// bullet will destroy by itself
					return;
				}

				// if the enemy is not connected to anything right now...
				if (!reproduced) {
					// the target is now acquired by the HACKER!
					targetCS.controller = Controller.Hacker;

					// set the target's 

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
		ControlStatus cs = controllable.GetComponent<ControlStatus> ();
		if(cs == null){
			return false;
		}
		return (cs.controller == Controller.None);
	}

	bool TypeMatches(Transform trans){
		ObjectIdentity oi = trans.GetComponent<ObjectIdentity> ();
		if(oi == null){
			return false;
		}
		if(oi.objType == targetType){
			return true;
		}
		else {
			return false;
		}
	}
}
