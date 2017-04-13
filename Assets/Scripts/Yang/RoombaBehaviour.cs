using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaBehaviour : MonoBehaviour {
	public float thrust = 20f;

	public float aimDelay = 2f;

	public SpritePair aimPointSP;
	public ColorPair aimColorCP;

	[HideInInspector] public Rigidbody2D body;
	FieldOfView fov;
	ControlStatus cs;
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

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		fov = GetComponent<FieldOfView> ();
		cs = GetComponent<ControlStatus> ();
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
			aimPointSP.trans.gameObject.SetActive (true);
			break;
		case Controller.Hacker:
			SetPlayerAim ();
			aimPointSP.trans.GetComponent<AimLaserUpdate> ().targetPos = targetLastPos;
			aimPointSP.trans.GetComponent<AimLaserUpdate> ().SnapPosition ();
			aimPointSP.trans.position = targetLastPos;
			aimPointSP.trans.gameObject.SetActive (true);
			break;
		}
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
}
