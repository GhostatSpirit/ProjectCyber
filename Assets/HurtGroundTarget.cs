using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtGroundTarget : MonoBehaviour {
	public TypeDamagePair[] DamageOtherList;

	public float impluse = 100f;

	Dictionary<ObjectType, float> DamageOtherDict = new Dictionary<ObjectType, float>();
	// Use this for initialization
	void Start () {
		// translate the two arrays into dictionary to boost speed
		foreach (TypeDamagePair pair in DamageOtherList){
			DamageOtherDict.Add (pair.type, pair.damage);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
//		Rigidbody2D body = coll.transform.parent.GetComponent<Rigidbody2D> ();
//		body.mass = 10 * body.mass;

		HurtLogic (coll);
	}

	void OnCollisionStay2D(Collision2D coll){
		//		Rigidbody2D body = coll.transform.parent.GetComponent<Rigidbody2D> ();
		//		body.mass = 10 * body.mass;

		HurtLogic (coll);
	}
		

	// Update is called once per frame
	void HurtLogic (Collision2D coll) {
		ObjectIdentity oi = coll.collider.transform.parent.GetComponent<ObjectIdentity> ();
		if (!oi)
			return;
		ObjectType otherType = oi.objType;


		float damage;
		if( DamageOtherDict.TryGetValue(otherType, out damage)){
			HealthSystem hs = coll.collider.transform.parent.GetComponent<HealthSystem> ();
			if(hs){
				hs.Damage(damage * Time.fixedDeltaTime);
			}
			Rigidbody2D body = coll.collider.transform.parent.GetComponent<Rigidbody2D> ();
			Debug.Log (body);
			if(body){
				Vector3 contactPoint = coll.contacts [0].point;
				Vector3 newForce = (coll.collider.transform.position - transform.position).normalized;

				Debug.Log (newForce);

				body.AddForce (newForce * impluse,ForceMode2D.Impulse);
			}
		}

	}
}
