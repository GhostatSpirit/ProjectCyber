using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusManager : MonoBehaviour {

	public GameObject virusPrefab;
	public Transform hacker;



	// the amount of virus that would be respawned at one time
	public int respawnCount = 4;

	public float respawnRadius = 3f;

	public float respawnDelay = 5f;

	public float rotateFacingSpeed = 10f;
	// the amount of virus that is alive for now
	public int controlCount{
		get{
			int count = 0;
			foreach(Transform child in transform){
				// does if have a objectIdentity and the identity is virus?
				ObjectIdentity oi = child.GetComponentInChildren<ObjectIdentity> ();
				if(oi && oi.objType == ObjectType.Virus){
					// append it to the virus list
					ControlStatus cs = oi.GetComponent<ControlStatus> ();
					if (cs.controller == Controller.Boss) {
						count++;
					}
				}
			}
			return count;
		}
	}

	public int totalCount{
		get{
			int count = 0;
			foreach(Transform child in transform){
				// does if have a objectIdentity and the identity is virus?
				ObjectIdentity oi = child.GetComponentInChildren<ObjectIdentity> ();
				if(oi && oi.objType == ObjectType.Virus){
					// append it to the virus list
					count++;
				}
			}
			return count;
		}
	}

	public Transform[] targets;

	FieldOfView fov;
	HealthSystem hs;
	// Use this for initialization
	void Start () {
		fov = GetComponent<FieldOfView> ();
		hs = GetComponentInParent<HealthSystem> ();

		targetLastPos = transform.position + fov.facing * fov.radius;

		if(totalCount == 0){
			Respawn ();
		}

	}
	// Update is called once per frame
	void Update () {
//		// set the facing
//		float dist = Mathf.Infinity;
//		Transform target = null;
//		foreach(Transform trans in targets){
//			float newDist = Vector3.Distance (trans.position, transform.position);
//			if(newDist < dist){
//				//Debug.Log ("iterating target");
//				dist = newDist;
//				target = trans;
//			}
//		}
//		if(target != null && fov != null){
//			//Debug.Log ("set facing");
//			Vector3 dir = target.position - transform.position;
//			dir.Normalize ();
//			fov.facing = dir;
//		}

		if(respawnCoroutine == null){
			if (controlCount == 0 && totalCount <= 3) {
				respawnCoroutine = StartCoroutine (DelayRespawnIE (respawnDelay));
			}
		}
	}

	[ReadOnly]public Transform target;
	[ReadOnly]public Vector3 targetLastPos;
	[ReadOnly]public bool targetInSight = false;
	void FixedUpdate(){
		target = fov.ScanLeastRotationInSight (targets);

		if(target){
			targetLastPos = target.position;
			targetInSight = true;
		} else {
			targetInSight = false;
		}

		// rotate towards the direction of the target
		Vector3 targetDir = (targetLastPos - transform.position).normalized;

		if(targetDir.magnitude != 0f){
			float step = rotateFacingSpeed * Time.deltaTime;
			fov.facing = 
				Vector3.RotateTowards(fov.facing, targetDir, step, Mathf.Infinity);
		}
	}


	Coroutine respawnCoroutine;

	IEnumerator DelayRespawnIE(float delay){
		yield return new WaitForSeconds (delay);
		Respawn ();
		respawnCoroutine = null;
		yield break;
	}


	// Respawn the virus around a circle with the radius of respawnRadius
	void Respawn(){
		if(hs && hs.IsDead()){
			return;
		}

		for(int i = 0; i < respawnCount; ++i){
			GameObject newVirus = 
				Instantiate (virusPrefab, transform.position, transform.rotation);
			if(fov){
				newVirus.GetComponentInChildren<VirusPosReceiver>().transform.up = fov.facing;
			}
			newVirus.transform.parent = transform;

			ControlStatus cs = newVirus.GetComponentInChildren<ControlStatus> ();
			if(cs){
				cs.Hacker = this.hacker;
				cs.Boss = transform;
			}

			// stop the virus from changing its virusState until it is
			// reaching the spreadRadius
			VirusStateControl vsc = newVirus.GetComponentInChildren<VirusStateControl> ();
			vsc.enabled = false;
			StartCoroutine (EnableStateChange (newVirus.transform));

		}
//		if(virusPrefab == null || respawnCount == 0){
//			return;
//		}
//		float deltaAngle = 360f / respawnCount;
//		float curAngle = 0f;
//		for(int virusIndex = 0; virusIndex < respawnCount; virusIndex ++){
//			float dirx = Mathf.Cos (curAngle * Mathf.Deg2Rad);
//			float diry = Mathf.Sin (curAngle * Mathf.Deg2Rad);
//			Vector3 virusDir = new Vector3 (dirx, diry, 0f);
//			virusDir.Normalize ();
//
//			Vector3 virusPosOffset = virusDir * respawnRadius;
//			Vector3 virusNewPos = transform.position + virusPosOffset;
//
//			Quaternion newRot = new Quaternion();
//			newRot.eulerAngles = new Vector3 (0f, 0f, curAngle - 90f);
//			// instantiate the virus
//			GameObject newVirus = Instantiate (virusPrefab, virusNewPos, newRot);
//			newVirus.transform.SetParent (this.transform);
//		
//			curAngle += deltaAngle;
//		}
//		currentCount += respawnCount;
	}

	IEnumerator EnableStateChange(Transform virusTrans){

		yield return new WaitUntil (() => {return ReachingSpreadRadius(virusTrans);});
		if (virusTrans) {
			VirusStateControl vsc = virusTrans.GetComponentInChildren<VirusStateControl> ();
			if (vsc) {
				vsc.enabled = true;
			}
		}
	}

	bool ReachingSpreadRadius(Transform virusTrans){
		VirusPosManager vpm = virusTrans.GetComponentInChildren<VirusPosManager>();
		float radius = 0f;
		if(vpm){
			radius = vpm.spreadRadius;
		}
		if(virusTrans == null){
			return true;
		}
		float dist = Vector3.Distance (transform.position, virusTrans.position);

		// 0.75 is a magic number
		return (dist >= 0.75 * radius);
	}
//	public void LoseOneVirus(){
//		currentCount--;
//	}
}
