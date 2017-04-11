using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaBehaviour : MonoBehaviour {
	public float thrust = 20f;

	public float aimDelay = 2f;

	[HideInInspector] public Rigidbody2D body;
	FieldOfView fov;
	ControlStatus cs;
	Animator animator;

	public Controller controller{
		get{
			if (!cs)
				return Controller.None;
			else
				return cs.controller;
		}
	}

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		fov = GetComponent<FieldOfView> ();
		cs = GetComponent<ControlStatus> ();
		animator = GetComponent<Animator> ();

		// set default targets 
		targets = playerTargets;

		// add actions
		cs.OnLinkedByEnemy += SetPlayerTargets;

		cs.OnLinkedByPlayer += SetEnemyTargets;
		cs.OnLinkedByPlayer += SetPlayerLink;

		cs.OnCutByPlayer += SetPlayerCut;

	}
	
	// Update is called once per frame
	void Update () {
		
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
		body.velocity = Vector3.zero;
	}

	ObjectType[] playerTargets = { ObjectType.AI, ObjectType.Hacker };
	ObjectType[] enemyTargets = { ObjectType.Virus, ObjectType.Robot, ObjectType.Boss };

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

	public void SetPlayerCut(Transform objTrans){
		Debug.Log ("unlink");
		animator.SetTrigger ("unlink");
	}

	public void SetPlayerLink(Transform objTrans){
		animator.SetTrigger ("playerLink");
	}
}
