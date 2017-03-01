using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class ChaseTarget : MonoBehaviour {
	/* A definition of all the target group this object could chase. */
	public enum Target {Player, Boss, None};
	public Target targetGroup = Target.Player;

	public List<Transform> playerTargets;
	public List<Transform> bossTargets;

	// the ONE specific this object would chase at a specific moment
	public Transform target;

	public float rotationSpeed = 90f;
	public float moveSpeed = 20f;

	Rigidbody2D myRigidbody;
	float rotSpeedFactor;

	Transform PickTarget(){
		/* pick the specfic target that:
		 * 1. belongs to targetGroup
		 * 2. is closest to this object
		 * 3. if targetGroup is none, return null
		 */
		Transform new_target = null;

		if (targetGroup == Target.Player || targetGroup == Target.Boss) {
			List<Transform> targets;
			if(targetGroup == Target.Player){
				targets = playerTargets;
			} else{
				targets = bossTargets;
			}

			if (targets.Count != 0) {
				float dist = Mathf.Infinity;
				foreach (Transform trans in targets) {
					float newDist = Vector3.Distance (this.transform.position, trans.position);
					if (newDist < dist) {
						new_target = trans;
					}
				}
			} else {
				new_target = null;
			}
		} else{
			new_target = null;
		}
		return new_target;
	}

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		target = PickTarget ();

		if(target == null){
			rotSpeedFactor = 0f;
			return;		// cannot find the target transform, wait for next frame
		}

		Vector2 point2Self = (Vector2)transform.position - (Vector2)target.transform.position;
		point2Self.Normalize ();

		// rotSpeedFactor = Vector3.Cross (point2Self, transform.up).z;
		rotSpeedFactor = -  Vector3.Cross (transform.up, point2Self).z;
		if(rotSpeedFactor < 0f){
			rotSpeedFactor = -rotSpeedFactor;
		}
		//Debug.Log (rotSpeedFactor);


//		float zAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90f;
//		Quaternion desiredRot = Quaternion.Euler (0f, 0f, zAngle);
//		transform.rotation = 
//			Quaternion.RotateTowards (transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);


	}

	void FixedUpdate(){
		myRigidbody.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;

		// update rotation of this object
		Vector2 point2Target = (Vector2)target.transform.position - (Vector2)transform.position;
		point2Target.Normalize ();
		float zAngle = Mathf.Atan2 (point2Target.y, point2Target.x) * Mathf.Rad2Deg - 90f;
		Quaternion desiredRot = Quaternion.Euler (0f, 0f, zAngle);

		float finalRotSpeed = rotationSpeed * rotSpeedFactor * Time.fixedDeltaTime;
		transform.rotation =
			Quaternion.RotateTowards (transform.rotation, desiredRot, finalRotSpeed);

		//myRigidbody.angularVelocity = rotationSpeed * rotSpeedFactor;
	}
}
