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

	[Range(0f,360f)]
	public float angle = 180f;

	public bool ignoreVisionBlock = false;

	// if has control status, we would only find enemy targets
	ControlStatus cs;
	//[Range(0f,360f)]
	//public float rotationOffset = 0f;

	public bool includeUncontrolledTarget = false;

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

	public bool useInitialFacing = false;
	public Direction initialDirection = Direction.UP;

	void Start(){
		cs = GetComponent<ControlStatus> ();
		if(useInitialFacing)
			facing = Direction2Vector (initialDirection);
	}
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
					if(HasVisionBlock(trans))  continue;
					if (IsDead (trans))  continue;
					if (!IsEnemy (trans))  continue;
						
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
				if(HasVisionBlock(target))  continue;
				if (IsDead (target))  continue;
				if (!IsEnemy (target))  continue;

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
				if(HasVisionBlock(target))  continue;
				if (IsDead (target))  continue;
				if (!IsEnemy (target))  continue;

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
	public bool CheckTarget(Transform target){
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
		if(ignoreVisionBlock){
			return false;
		}

		float dist = Vector3.Distance (transform.position, target.position);
		Vector3 dir = target.transform.position - transform.position;
		dir.Normalize ();

		RaycastHit2D[] hits = 
			Physics2D.RaycastAll (transform.position, dir, dist);
		foreach(RaycastHit2D hit in hits){
			Transform hitTrans = hit.transform;
			if(hit.collider.isTrigger || hitTrans == transform){
				continue;
			}

			ObjectIdentity oi = hitTrans.GetComponent<ObjectIdentity> ();
			if (oi) {
				return oi.isVisionBlocker ();
			}

			if(hit.collider && hit.collider.isTrigger == false && 
				hit.collider.GetComponent<SFPolygon>()){
				return true;
			}


			PolyNavObstacle obstacle = hitTrans.GetComponent<PolyNavObstacle> ();
			if(obstacle){
				return true;
			}
				
		}

		return false;
	}


	bool IsEnemy(Transform target){
		bool isEnemy = true;
		ControlStatus targetCS = target.GetComponent<ControlStatus> ();
		if(cs && targetCS){
			if(targetCS.controller == Controller.None){
				if (!includeUncontrolledTarget) {
					return false;
				} else{
					return true;
				}
			}
			if(cs.controller == targetCS.controller && cs.controller != Controller.None){
				isEnemy = false;
			}
		}
		return isEnemy;
	}

	bool IsDead(Transform target){
		bool isDead = false;
		HealthSystem targetHS = target.GetComponent<HealthSystem> ();
		if(targetHS && targetHS.IsDead()){
			isDead = true;
		}
		return isDead;
	}

	// translate a Direction enum to a normalized Vector3
	Direction Vector2Direction(Vector2 vec){
		if(vec.magnitude == 0f){
			Debug.Log ("Warning: vec.magnitude == 0f");
			return Direction.RIGHT;
		}

		Vector2 rightVector = new Vector2 (1f, 0f);

		float angle = Vector2.Angle (rightVector, vec);

		if(vec.y < 0f){
			angle = 360f - angle;
		}
		// play "going upright" animation if angle between 22.5° and 67.5°
		if (angle > 22.5f && angle <= 67.5f)
		{
			return Direction.UPRIGHT;// up
		}
		// play "going up" animation if angle between 67.5° and 112.5°
		else if (angle > 67.5f && angle <= 112.5f)
		{
			return Direction.UP;// left
		}
		// play "going upleft" animation if angle between 225° and 315°
		else if (angle > 112.5f && angle <= 157.5f)
		{
			return Direction.UPLEFT;// down
		}
		else if (angle > 157.5f && angle <= 202.5f){
			return Direction.LEFT;
		}
		else if (angle > 202.5f && angle <= 247.5f){
			return Direction.DOWNLEFT;
		}
		else if (angle > 247.5f && angle <= 292.5f){
			return Direction.DOWN;
		}
		else if (angle > 292.5f && angle <= 357.5f){
			return Direction.DOWNRIGHT;
		}
		else{
			return Direction.RIGHT;
		}

	}

	Vector2 Direction2Vector(Direction dir){
		Vector2 dirvec = Vector2.zero;

		switch (dir) {
		case Direction.DOWN:
			dirvec = Vector2.down;
			break;
		case Direction.DOWNLEFT:
			dirvec = Vector2.down + Vector2.left;
			break;
		case Direction.DOWNRIGHT:
			dirvec = Vector2.down + Vector2.right;
			break;
		case Direction.LEFT:
			dirvec = Vector2.left;
			break;
		case Direction.RIGHT:
			dirvec = Vector2.right;
			break;
		case Direction.UP:
			dirvec = Vector2.up;
			break;
		case Direction.UPLEFT:
			dirvec = Vector2.up + Vector2.left;
			break;
		case Direction.UPRIGHT:
			dirvec = Vector2.up + Vector2.right;
			break;
		default:
			dirvec = Vector2.down;
			break;
		}

		return dirvec.normalized;
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