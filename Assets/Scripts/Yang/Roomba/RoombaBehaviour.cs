﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RoombaBehaviour : MonoBehaviour {
	public float thrust = 20f;

	public float aimDelay = 2f;

	public SpritePair aimPointSP;
	public ColorPair aimColorCP;

//	public GameObject explosion;
//	public Transform explosionParent;
	public float explosionDelay = 0.5f;

	[HideInInspector] public Rigidbody2D body;
	FieldOfView fov;
	[HideInInspector] public ControlStatus cs;
	LineUpdate lu;
	Animator animator;
	HurtAndDamage hd;


	public Controller controller{
		get{
			if (!cs)
				return Controller.None;
			else
				return cs.controller;
		}
	}


	[ReadOnly]public  Vector3 _incomingVelocity;
	[ReadOnly]public bool canSetIncomingVelocity = true;

	public Vector3 incomingVelocity{
		get{
			return _incomingVelocity;
		}
		set{
			if(canSetIncomingVelocity){
				_incomingVelocity = value;
				canSetIncomingVelocity = false;
			}
		}
	}


	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		fov = GetComponent<FieldOfView> ();
		cs = GetComponent<ControlStatus> ();
		lu = GetComponent<LineUpdate> ();
		animator = GetComponent<Animator> ();
		hd = GetComponent<HurtAndDamage> ();

		// set default targets 
		targets = playerTargets;

		// add actions
		cs.OnLinkedByEnemy += SetPlayerTargets;

		cs.OnLinkedByPlayer += SetEnemyTargets;
		cs.OnLinkedByPlayer += SetPlayerLink;

		cs.OnCutByPlayer += SetCut;
		cs.OnCutByEnemy += SetCut;


		hd.canHurtOther = false;

		if(cs.controller == Controller.Boss){
			animator.SetTrigger ("enemyLink");
		}

		if (_incomingVelocity == Vector3.zero) {
			_incomingVelocity = Vector3.up;
		}

		canSetIncomingVelocity = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetEnemyAim(){
		Transform aimTrans = aimPointSP.trans;
		// set aim laser color
		LineRenderer lr = aimTrans.GetComponent<LineRenderer> ();
		if(lr){
			lr.startColor = aimColorCP.enemyColor;
			lr.endColor = aimColorCP.enemyColor;
			lr.sortingLayerName = "ControlLine";
		}
		// set aim point sprite
		aimPointSP.SetEnemy ();
	}

	public void SetPlayerAim(){
		Transform aimTrans = aimPointSP.trans;
		// set aim laser color
		LineRenderer lr = aimTrans.GetComponent<LineRenderer> ();
		if(lr){
			lr.startColor = aimColorCP.playerColor;
			lr.endColor = aimColorCP.playerColor;
			lr.sortingLayerName = "ControlLine";
		}
		// set aim point sprite
		aimPointSP.SetPlayer ();
	}

	public void TurnOnAim(){
		switch(cs.controller){
		case Controller.Boss:
			SetEnemyAim ();
			aimPointSP.trans.GetComponent<AimLaserUpdate> ().targetPos = targetLastPos;
			aimPointSP.trans.GetComponent<AimLaserUpdate> ().SnapPosition ();
			aimPointSP.trans.position = targetLastPos;
			StartCoroutine (TurnOnAimIE ());
			break;
		case Controller.Hacker:
			{
//			SetPlayerAim ();
//			aimPointSP.trans.GetComponent<AimLaserUpdate> ().targetPos = targetLastPos;
//			aimPointSP.trans.GetComponent<AimLaserUpdate> ().SnapPosition ();
//			aimPointSP.trans.position = targetLastPos;
//			StartCoroutine (TurnOnAimIE ());
				SetPlayerAim ();
				// calculate the aim position through incoming velocity
				Vector3 newAimPos = transform.position + incomingVelocity.normalized * fov.radius;
				aimPointSP.trans.GetComponent<AimLaserUpdate> ().targetPos = newAimPos;
				aimPointSP.trans.GetComponent<AimLaserUpdate> ().SnapPosition ();
				aimPointSP.trans.position = newAimPos;
				StartCoroutine (TurnOnAimIE ());
				break;
			}
		}
	}

	


	IEnumerator TurnOnAimIE(){
		yield return new WaitForFixedUpdate ();
		//yield return new WaitForFixedUpdate ();
		aimPointSP.trans.gameObject.SetActive (true);
		yield return null;
	}

	public void TurnOffAim(){
		aimPointSP.trans.gameObject.SetActive (false);
	}

	public void UpdateAim(){
		aimPointSP.trans.GetComponent<AimLaserUpdate> ().targetPos = targetLastPos;
		aimPointSP.trans.position = targetLastPos;
	}


	Coroutine aimCoroutine;
	public void StartAim(){
		if(aimCoroutine != null){
			StopCoroutine (aimCoroutine);
			aimCoroutine = null;
		}
		aimCoroutine = StartCoroutine ("StartAimIE");
	}

	public void EndAim(){
		if(aimCoroutine != null){
			//Debug.Log ("stopped coroutine");
			StopCoroutine (aimCoroutine);
			aimCoroutine = null;
		}
	}

	IEnumerator StartAimIE(){
		yield return new WaitForSeconds (aimDelay);
		animator.SetTrigger ("finishAim");
		aimCoroutine = null;
	}


	public void ResetVelocity(){
		StartCoroutine (ResetVelocityIE());
	}

	IEnumerator ResetVelocityIE(){
		yield return new WaitUntil (() => body != null);
		body.velocity = Vector3.zero;
	}

	ObjectType[] playerTargets = { ObjectType.AI, ObjectType.Hacker, ObjectType.Roomba };
	ObjectType[] enemyTargets = { ObjectType.Virus, ObjectType.Robot, ObjectType.Boss, ObjectType.Roomba };

	ObjectType[] targets;

	[ReadOnly]public Transform target;
	[ReadOnly]public Vector3 targetLastPos;
	[ReadOnly]public bool targetInSight = false;
	void FixedUpdate(){
		target = fov.ScanTargetInSight (targets);

		if(target){
			targetLastPos = target.position;
			targetInSight = true;
		} else {
			targetInSight = false;
		}

		animator.SetBool ("targetInSight", targetInSight);
	}

	public void SetPlayerTargets(Transform targetTrans){
		targets = playerTargets;
	}

	public void SetEnemyTargets(Transform targetTrans){
		targets = enemyTargets;
	}

	public void SetCut(Transform objTrans){
		Debug.Log ("unlink");
		animator.SetTrigger ("unlink");
	}

	public void SetPlayerLink(Transform objTrans){
		animator.SetTrigger ("playerLink");
	}

	public void EnableLine(){
		lu.EnableLine ();
	}
	public void DisableLine(){
		lu.DisableLine ();
	}

	Coroutine explosionCoroutine;
//
	public void TryStartExplosion(){
		if(explosionCoroutine == null){
			Rigidbody2D body = GetComponent<Rigidbody2D> ();
			if(body){
				body.velocity = Vector3.zero;
			}

			explosionCoroutine = StartCoroutine (StartExplosionIE());
		}
	}

	public void StopExplosion(){
		if(explosionCoroutine != null){
			StopCoroutine (explosionCoroutine);
			explosionCoroutine = null;
		}
	}

	IEnumerator StartExplosionIE(){
		// stop the roomba from moving
		body.velocity = Vector3.zero;

		yield return new WaitForSeconds (explosionDelay);

		HealthSystem hs = GetComponent<HealthSystem> ();
		if(hs){
			DisableLine ();
			hs.InstantDead ();
		}

		explosionCoroutine = null;
	}

	// fields and functions for RoombaExplosion
	[ReadOnly] public bool checkCollision = false;
	ObjectType[] ignoredTypes = { ObjectType.HackerBullet, ObjectType.RobotBullet };
	void OnCollisionEnter2D(Collision2D coll){
		if(!checkCollision){
			return;
		}
		// check if the colliding object is in the ignored list
		ObjectIdentity oi = coll.collider.GetComponentInChildren<ObjectIdentity> ();
		if (oi && ignoredTypes.Contains (oi.objType)){
			return;
		}
		// if not, send a explode trigger to animator
		animator.SetTrigger ("explode");
//		Debug.Log ("explode set");
		checkCollision = false;
	}

	void OnCollisionStay2D(Collision2D coll){
		if(!checkCollision){
			return;
		}
		// check if the colliding object is in the ignored list
		ObjectIdentity oi = coll.collider.GetComponentInChildren<ObjectIdentity> ();
		if (oi && ignoredTypes.Contains (oi.objType)){
			return;
		}
		// if not, send a explode trigger to animator
		animator.SetTrigger ("explode");
		Debug.Log ("explode set");
		checkCollision = false;
	}


	public Vector3 GetLaserContact(Vector3 start, Vector3 end, LayerMask mask){
		ContactFilter2D filter = new ContactFilter2D ();
		filter.useTriggers = false;
		filter.useLayerMask = true;
		filter.useDepth = false;
		filter.useNormalAngle = false;
		filter.SetLayerMask (mask);

		Vector3 dir = (end - start).normalized;
		float dist = (end - start).magnitude;
		RaycastHit2D[] hits = new RaycastHit2D[1];
		int count = Physics2D.Raycast (start, dir, filter, hits, dist);

		if(count == 0){
			return end;
		}
		else{
			return hits [0].point;
		}
	}


	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + incomingVelocity);

	}
}
