using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class FieldOfView : MonoBehaviour
{
	// radius of the float area
	public float radius = 1.5f;

	[Range(0f,180f)]
	public float angle = 180f;

	//[Range(0f,360f)]
	//public float rotationOffset = 0f;

	// a vector3 points to where this object is facing
	// if facing is Vector3.zero or null, we will use transform.up instead
	public Vector3 facing{
		get{
			if(m_facing == Vector3.zero){
				return transform.up;
			}
			else{
				return m_facing;
			}
		}
		set{
			m_facing = value.normalized;
		}
	}

	Vector3 m_facing = Vector3.zero;


	// run this only in unity editor mode
//	#if UNITY_EDITOR
//	void OnEnable ()
//	{
//		facing = Quaternion.AngleAxis(rotationOffset, Vector3.forward) * transform.up;
//
//	}
//
//	void Update(){
//		facing = Quaternion.AngleAxis(rotationOffset, Vector3.forward) * transform.up;
//
//	}
//	#endif

	// scan for possible targets in sight (using FieldOfView)
	// return the closest target
	// if no possible targets could be found, return null
	public Transform ScanTargetInSight(List<Transform> targets){
		Transform new_target = null;

		if (targets.Count != 0) {
			float dist = Mathf.Infinity;
			foreach (Transform trans in targets) {
				if(CheckTarget(trans)){
					// check if vision blocker
					if(HasVisionBlock(trans)){
						continue;
					}
						
					float newDist = Vector3.Distance (this.transform.position, trans.position);
					if (newDist < dist) {
						dist = newDist;
						new_target = trans;
					}
				}
			}
		} else {
			new_target = null;
		}

//		if(new_target == null){
//			Debug.Log ("not in sight");
//		}else{
//			Debug.Log ("target in sight");
//		}

		return new_target;
	}
		

	public Transform ScanTargetInSight(ObjectType[] targetTypes, float angleFactor = 1f){
		Transform new_target = null;
		LayerMask virusLayerMask = Physics2D.GetLayerCollisionMask (gameObject.layer);

		Collider2D[] hitColliders = 
			Physics2D.OverlapCircleAll (transform.position, radius, virusLayerMask);

		if (hitColliders.Length != 0) {
			// we actually hit something here
			float dist = Mathf.Infinity;
			foreach(Collider2D coll in hitColliders){
				Transform target = coll.transform;
				// check if the target's type falls into targettypes
				ObjectIdentity oi = target.GetComponent<ObjectIdentity> ();
				if (!oi || !targetTypes.Contains (oi.objType)) {
					continue;
				}
				// check the target's angle
				bool inAngle = false;
				if(angleFactor == 1f){
					inAngle = CheckAngle (target);
				} else {
					inAngle = CheckPartialAngel (target, angleFactor);
				}

				if(!inAngle){
					continue;
				}
				// check if vision blocker
				if(HasVisionBlock(target)){
					continue;
				}

				// now it is a valid target, if its distance is smaller, use it instead
				float newDist = 
					Vector3.Distance(this.transform.position, target.transform.position);
				if(newDist < dist){
//					Debug.Log (newDist);
//					Debug.Log (target);
//
					dist = newDist;
					new_target = target;
				}

			}
		}
		else{
			return null;
		}

//		Debug.Log (new_target);

		return new_target;
	}


	public Transform ScanTargetInSight(List<ObjectType> targetTypes, float angleFactor = 1f){
		Transform new_target = null;
		LayerMask virusLayerMask = Physics2D.GetLayerCollisionMask (gameObject.layer);

		Collider2D[] hitColliders = 
			Physics2D.OverlapCircleAll (transform.position, radius, virusLayerMask);

		if (hitColliders.Length != 0) {
			// we actually hit something here
			float dist = Mathf.Infinity;
			foreach(Collider2D coll in hitColliders){
				Transform target = coll.transform;
				// check if the target's type falls into targettypes
				ObjectIdentity oi = target.GetComponent<ObjectIdentity> ();
				if (!oi || !targetTypes.Contains (oi.objType)) {
					continue;
				}
				// check the target's angle
				bool inAngle = false;
				if(angleFactor == 1f){
					inAngle = CheckAngle (target);
				} else {
					inAngle = CheckPartialAngel (target, angleFactor);
				}

				if(!inAngle){
					continue;
				}
				// check if vision blocker
				if(HasVisionBlock(target)){
					continue;
				}

				// now it is a valid target, if its distance is smaller, use it instead
				float newDist = 
					Vector3.Distance(this.transform.position, target.transform.position);
				if(newDist < dist){
					//					Debug.Log (newDist);
					//					Debug.Log (target);
					//
					dist = newDist;
					new_target = target;
				}

			}
		}
		else{
			return null;
		}

		//		Debug.Log (new_target);

		return new_target;
	}


	bool CheckPartialAngel(Transform target, float factor){
		if (factor <= 0f) {
			Debug.LogError ("angle factor is below zero.");
			return false;
		}
		if (factor > 1f) {
			factor = 1f;
		}

		float partialAngle = angle * factor;

		// check if it is in angle
		Vector3 dirToTarget = target.position - this.transform.position;
		dirToTarget.Normalize ();
		Quaternion rot = Quaternion.FromToRotation (facing, dirToTarget);


		float angleZ = rot.eulerAngles.z;
		if(angleZ > 180f){
			angleZ -= 360f;
		}
		//		Debug.Log (target);
		//		Debug.Log ("Angle: " + angleZ.ToString ());

		if(Mathf.Abs(angleZ) > partialAngle / 2.0f){
			return false;
		}
		else{
			return true;
		}	
	}

	// check if a target is in the fov angle
	bool CheckAngle(Transform target){
		// check if it is in angle
		Vector3 dirToTarget = target.position - this.transform.position;
		dirToTarget.Normalize ();
		Quaternion rot = Quaternion.FromToRotation (facing, dirToTarget);


		float angleZ = rot.eulerAngles.z;
		if(angleZ > 180f){
			angleZ -= 360f;
		}
		//		Debug.Log (target);
		//		Debug.Log ("Angle: " + angleZ.ToString ());

		if(Mathf.Abs(angleZ) > angle / 2.0f){
			return false;
		}
		else{
			return true;
		}
	}


	// check if a given target is in sight
	bool CheckTarget(Transform target){
		float newDist = Vector3.Distance (this.transform.position, target.position);
		if(newDist > radius){
			return false;
		}
		// now the target is in radius
		// check if it is in angle
		Vector3 dirToTarget = target.position - this.transform.position;
		dirToTarget.Normalize ();
		Quaternion rot = Quaternion.FromToRotation (facing, dirToTarget);

		float angleZ = rot.eulerAngles.z;
		if(angleZ > 180f){
			angleZ -= 360f;
		}
//		Debug.Log (target);
//		Debug.Log ("Angle: " + angleZ.ToString ());

		if(Mathf.Abs(angleZ) > angle / 2.0f){
			return false;
		}
		else{
			return true;
		}
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
				
		}

		return false;
	}


}

public class TargetMeta{
	public Transform target;
	public float distance;
	public float deltaAngle;

	TargetMeta(Transform _target, float _dist, float _deltaAng){
		target = _target;
		distance = _dist;
		deltaAngle = _deltaAng;
	}

}