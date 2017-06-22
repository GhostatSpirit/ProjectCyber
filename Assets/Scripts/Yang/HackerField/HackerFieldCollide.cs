using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerFieldCollide : MonoBehaviour {

	public LayerMask targetMask;
	public int maxHitCount = 10;

	Collider2D fieldCollider;
	ContactFilter2D filter;

	public float paralyzeTime = 2f;

	public string friendlyVirusLayerName = "PlayerHover";

	// Use this for initialization
	void Start () {
		fieldCollider = GetComponent<Collider2D> ();

		filter = new ContactFilter2D ();
		filter.useTriggers = false;
		filter.useLayerMask = true;
		filter.useDepth = false;
		filter.useNormalAngle = false;
		filter.SetLayerMask (targetMask);
	}

	void OnEnable(){
		
	}


	void FixedUpdate(){
		Collider2D[] hitColliders = new Collider2D[maxHitCount];
		int hitCount = fieldCollider.OverlapCollider (filter, hitColliders);

		//Debug.Log (hitCount);

		for(int i = 0; i < hitCount; ++i){
			//Debug.Log ("int i is: " + i.ToString());
			Collider2D hitCollider = hitColliders [i];


			ObjectIdentity oi = hitCollider.transform.GetComponentInParent<ObjectIdentity> ();
			if(oi == null){
				Debug.Log (hitCollider.transform.parent);
				continue;
			}

			//Debug.Log ("ATField hits: " + oi.objType.ToString ());

			switch(oi.objType){
			case ObjectType.Virus:{
					// when the bullet hits a virus...
					if (hitCollider.gameObject.layer == this.gameObject.layer) {
						return;
					}
					HitVirusBehaviour (hitCollider);
					break;
				}
			case ObjectType.Robot:{
					HitRobotBehaviour (hitCollider);
					break;
				}
			case ObjectType.Interface:{
					DefaultBehaviour (hitCollider);
					break;
				}
			case ObjectType.LaserCannon:{
					DefaultBehaviour (hitCollider);
					break;
				}
			case ObjectType.Roomba:{
					HitRoombaBehaviour (hitCollider);
					break;
				}

			default:{
					DefaultBehaviour (hitCollider);
					break;
				}
			}
		}


	}


	void HitVirusBehaviour(Collider2D coll){
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
			// the target is now acquired by the HACKER!
			targetCS.controller = Controller.Hacker;

			// modify layer so it wont collide
			coll.gameObject.layer = LayerMask.NameToLayer(friendlyVirusLayerName);

		}
	}

	void DefaultBehaviour(Collider2D coll){
		//Debug.Log (coll.transform);
		ControlStatus cs = coll.transform.GetComponentInParent<ControlStatus> ();


		if(!cs){
			return;
		}
//		Debug.Log (cs.controller.ToString ());

		if (NotControlled (coll.transform)){
			// it the door is not controlled by the boss...
			coll.transform.GetComponentInParent<ControlStatus> ().controller = Controller.Hacker;
		}
	}

	void HitRobotBehaviour(Collider2D coll){
		if(NotControlled(coll.transform)){
			coll.transform.GetComponent<ControlStatus> ().controller = Controller.Hacker;
		}
		else{
			coll.transform.GetComponent<Animator> ().SetTrigger ("paralyzed");
		}
	}

	void HitRoombaBehaviour(Collider2D coll){
		Transform targetTrans = coll.transform;

		ControlStatus cs = targetTrans.GetComponentInParent<ControlStatus> ();
		if(!cs){
			return;
		}
		if (NotControlled (targetTrans)){
			// it the door is not controlled by the boss...
			RoombaBehaviour roomba = targetTrans.GetComponent<RoombaBehaviour> ();
			if(roomba){
				roomba.incomingVelocity = (targetTrans.position - transform.position).normalized;
			}

			targetTrans.GetComponentInParent<ControlStatus> ().controller = Controller.Hacker;
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
}
