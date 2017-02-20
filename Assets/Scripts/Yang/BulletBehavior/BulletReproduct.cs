// shoot various bulletse when hits an enemy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReproduct : MonoBehaviour {

	public GameObject bulletPrefab;

	public string enemyTag = "Enemy";

	public int bulletCount = 3;
	public float deltaAngle = 10f;

	bool reproduced = false;


	void OnCollisionEnter2D(Collision2D coll){
		if(coll.transform.tag == enemyTag && coll.gameObject.layer != this.gameObject.layer){
			// TODO: verify that the enemy is not controlled by anything elese
			// if the enemy is not connected to anything right now...
			if (!reproduced) {
				// set the enemy to friend
				coll.gameObject.layer = this.gameObject.layer;
				coll.gameObject.tag = this.tag;
				// reproduce the bullets
				reproduced = true;
				Invoke ("ReproduceBullets", 0.1f);
				transform.gameObject.SetActive (false);
				Destroy (gameObject, 0.2f);
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
			GameObject bulletObj = Instantiate (bulletPrefab, transform.position, newBulletRot);
			bulletObj.SetActive (true);

			// set init velocity of the bullet
			// bulletObj.GetComponent<Rigidbody2D> ().velocity = bulletObj.transform.up.normalized * initialVelocity;
			// tell the bulletObj the init velocity
			//bulletObj.GetComponent<BulletDeflect> ().initialVelocity = initialVelocity;
		}
			
	}
		
}
