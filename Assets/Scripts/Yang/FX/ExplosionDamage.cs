using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionDamage : MonoBehaviour {
	public LayerMask layerMask = Physics2D.AllLayers;

	public TypeDamagePair[] DamageOtherList;

	Dictionary<ObjectType, float> DamageOtherDict = new Dictionary<ObjectType, float>();

	Collider2D explosionArea;

	// Use this for initialization
	void Start () {
		explosionArea = GetComponent<Collider2D> ();

		foreach (TypeDamagePair pair in DamageOtherList){
			DamageOtherDict.Add (pair.type, pair.damage);
		}
	}

	void FixedUpdate(){
		
	}

	// check if there is a vison blocker between the target and this object
	bool HasVisionBlock(Transform target){
		float dist = Vector3.Distance (transform.position, target.position);
		Vector3 dir = target.transform.position - transform.position;
		dir.Normalize ();

		RaycastHit2D[] hits = 
			Physics2D.RaycastAll (transform.position, dir, dist);
		foreach(RaycastHit2D hit in hits){
			Transform hitTrans = hit.transform;


			ObjectIdentity oi = hitTrans.GetComponent<ObjectIdentity> ();
			if (oi && oi.isVisionBlocker()) {
				return true;
			}

			PolyNavObstacle obstacle = hitTrans.GetComponent<PolyNavObstacle> ();
			if(obstacle){
				return true;
			}

		}
		return false;
	}
}

