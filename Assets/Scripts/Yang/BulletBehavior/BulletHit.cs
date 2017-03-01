// shoot various bulletse when hits an enemy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour {

	public GameObject bulletPrefab;

	public string enemyTag = "Enemy";

	public float paralyzeTime = 2f;

	public int bulletCount = 3;
	public float deltaAngle = 10f;

	bool reproduced = false;


	void OnCollisionEnter2D(Collision2D coll){
		if(coll.transform.tag == enemyTag && coll.gameObject.layer != this.gameObject.layer){
			if (HasControlStatus (coll.transform)) {
				// the colliding object must have control status
				ControlStatus targetCS = coll.transform.GetComponent<ControlStatus> ();

				// verify that the enemy is not controlled by anything else
				if (NotControlled (coll.transform) == false) {
					// if the enemy is controlled by something else,
					// paralyze the target
					targetCS.Paralyze (paralyzeTime);

					// destory this bullet
					Destroy (transform.gameObject);
					return;
				}

				// if the enemy is not connected to anything right now...
				if (!reproduced) {
					// set the enemy to friend
					coll.gameObject.layer = this.gameObject.layer;
					coll.gameObject.tag = this.tag;
					// stop the enemy from chasing the player
					coll.transform.GetComponent<ChaseTarget> ().enabled = false;
					// reproduce the bullets
					reproduced = true;
					Invoke ("ReproduceBullets", 0.1f);
					transform.gameObject.SetActive (false);
					Destroy (gameObject, 0.2f);
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
}
