using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

[RequireComponent (typeof(VirusTargetPicker))]
public class VirusStateControl : MonoBehaviour {

	public enum VirusState {Idle, Chase, Return, Paralyze};

	[HideInInspector] public VirusState virusState{
		get{
			return m_virusState;
		}
		set{
			VirusState oldState = m_virusState;
			VirusState newState = value;

			if (oldState != newState) {
				switch (oldState) {
				case VirusState.Idle:
					if (OnIdleEnd != null) OnIdleEnd (this.transform);
					break;

				case VirusState.Chase:
					if (OnChaseEnd != null) OnChaseEnd (this.transform);
					break;

				case VirusState.Return:
					if (OnReturnEnd != null) OnReturnEnd (this.transform);
					break;

				case VirusState.Paralyze:
					if (OnParalyzeEnd != null) OnParalyzeEnd (this.transform);
					break;

				default:
					Debug.LogError ("VirusState invalid");
					break;
				}

				switch (newState) {
				case VirusState.Idle:
					if (OnIdleStart != null) OnIdleStart (this.transform);
					break;

				case VirusState.Chase:
					if (OnChaseStart != null) OnChaseStart (this.transform);
					break;

				case VirusState.Return:
					if (OnReturnStart != null) OnReturnStart (this.transform);
					break;

				case VirusState.Paralyze:
					if (OnParalyzeStart != null) OnParalyzeStart (this.transform);
					break;

				default:
					Debug.LogError ("VirusState invalid");
					break;
				}
			}
				
			m_virusState = value;
		}
	}



	public VirusState m_virusState;


	public event Action<Transform> OnIdleStart;
	public event Action<Transform> OnChaseStart;
	public event Action<Transform> OnReturnStart;
	public event Action<Transform> OnParalyzeStart;

	public event Action<Transform> OnIdleEnd;
	public event Action<Transform> OnChaseEnd;
	public event Action<Transform> OnReturnEnd;
	public event Action<Transform> OnParalyzeEnd;

	ChaseTarget ct;
	float defaultRotSpeed = 0f;
	float defaultMoveSpeed = 0f;


	public float maxControlDistance = 10f;

	[Range(0f, 90f)]
	public float stopRotAngle = 20f;


	public float rotMoveSpeedFactor = 0.2f;
	public float rotRotSpeedFactor = 2f;

	Vector3 releasePos;

	void ResetActions(){
		OnIdleStart = null;
		OnIdleStart += StopChase;
		OnIdleStart += StartPosReceiver;


		OnIdleEnd = null;
		OnIdleEnd += StopPosReceiver;

		OnChaseStart = null;
		OnChaseStart += StartChase;
		OnChaseStart += SetTargetToNull;
		OnChaseStart += SetReleasePos;
		OnChaseStart += SetIsBullet;
		OnChaseEnd = null;
		OnChaseEnd += UnsetIsBullet;

		OnReturnStart = null;
		OnReturnStart += StartReturn;
		OnReturnStart += ReturnIsBulletLogic;
		OnReturnEnd = null;
		OnReturnEnd += BreakEndRot;
		OnReturnEnd += UnsetIsBullet;

		OnParalyzeStart = null;
		OnParalyzeStart += StartParalyze;
		OnParalyzeEnd = null;
		OnParalyzeEnd += BreakEndParalyze;
	}

	HurtAndDamage hd;
	ControlStatus cs;

	// Use this for initialization
	void Start () {
		ct = GetComponent<ChaseTarget> ();
		cs = GetComponent<ControlStatus> ();
		hd = GetComponent<HurtAndDamage> ();

		if(ct){
			defaultRotSpeed = ct.rotationSpeed;
			defaultMoveSpeed = ct.moveSpeed;
		}
			
		virusState = VirusState.Idle;
		previousState = VirusState.Idle;

		ResetActions ();
		if (OnIdleStart != null) OnIdleStart (this.transform);

		paralyzeTime = 5f;

		tp = GetComponent<VirusTargetPicker> ();
		vpm = transform.parent.GetComponent<VirusPosManager> ();

		if(cs){
			cs.OnCutByEnemy += StopStateControl;
			cs.OnCutByPlayer += StopStateControl;

			cs.OnLinkedByEnemy += StartStateControl;
			cs.OnLinkedByPlayer += StartStateControl;
		}
	}


	VirusTargetPicker tp;
	VirusPosManager vpm;
	// logic for switching states
	void Update () {
		//ObjectIdentity oi = transform.parent.GetComponent<ObjectIdentity> ();


		//if (oi == null)
			//return;
		switch(cs.controller){
		case Controller.Boss:
			// state switch logic for Boss
			vpm = transform.parent.GetComponent<VirusPosManager> ();
			switch (virusState) {
			case VirusState.Idle:
				{
					Transform newTarget = tp.PickTarget ();
					if (newTarget != null) {
						// we found a new target here, set the target to the new target
						// and change the state
	//					Debug.Log ("leaving idle, entering chase");
						tp.SetNewTarget (newTarget);
						// entering the chase state
						virusState = VirusState.Chase;
					}
					break;
				}
			case VirusState.Chase:
				{
					Transform newTarget = tp.PickTarget ();
					float dist = 
						Vector3.Distance (transform.position, transform.parent.position);

					//if (dist > maxControlDistance) {
					if (newTarget == null || dist > maxControlDistance) {
						// lost the target OR reaching the distance boundary
						if(dist <= vpm.spreadRadius * 1.2f){
							virusState = VirusState.Idle;
						} else {
							virusState = VirusState.Return;
						}

					} else {
						// update target
						tp.SetNewTarget (newTarget);
					}
					break;
				}
			case VirusState.Return:
				{
					Transform newTarget = tp.PickTarget ();
					if (newTarget != null) {
//						Debug.Log ("leaving return, entering chase");
						// found new target
						tp.SetNewTarget (newTarget);
						// entering the chase state
						virusState = VirusState.Chase;
					} else {
						// set chase target to parent
						tp.SetParentAsTarget ();
						float dist = 
							Vector3.Distance (transform.position, transform.parent.position);
						if (dist <= vpm.spreadRadius) {
							// reached parent object, switch to idle
							virusState = VirusState.Idle;
						}
					}
					break;
				}
			default:
				break;
			
			}
			break;



		case Controller.Hacker:
			// state switch logic for hacker
			switch (virusState) {
			case VirusState.Return:
				{
					StopPosReceiver(this.transform);
					tp.SetParentAsTarget ();
					float dist = 
						Vector3.Distance (transform.position, transform.parent.position);
					if(dist <= vpm.spreadRadius){
						// reached parent object, switch to idle
						virusState = VirusState.Idle;
					}
					break;
				}
			case VirusState.Idle:
				{
					// if the player presses the buttom, release this virus
					// state switch from Idle to Chase is managed in Hacker's script

					break;
				}
			case VirusState.Chase:
				{
					Transform newTarget = tp.PickTarget ();
					float dist = 
						Vector3.Distance (transform.position, releasePos);
					tp.SetNewTarget (newTarget);
					if(dist > maxControlDistance){
						HealthSystem hs = GetComponent<HealthSystem> ();
						if(hs){
							hs.InstantDead ();
						}
					}

					break;
				}
			}
			break;

		case Controller.None:{
				StopChase (this.transform);
				break;
			}
		
		default:
			Debug.LogError ("parent obj type must be hacker of boss");
			break;
		}
	}



	void StopChase(Transform virusTrans){
		if(ct != null){
			ct.enabled = false;
		}
	}

	void StartChase(Transform virusTrans){
		if(ct != null){
			ct.enabled = true;
			ct.rotationSpeed = defaultRotSpeed;
			ct.moveSpeed = defaultMoveSpeed;
		}
	}

	void StartRot(Transform virusTrans){
		if(ct != null){
			ct.enabled = true;
			ct.rotationSpeed = defaultRotSpeed * rotRotSpeedFactor;
			ct.moveSpeed = defaultMoveSpeed * rotMoveSpeedFactor;
		}
	}

	/* Paralyze
	 * stop the enemy from chasing the target 
	 * in a given amount of time (seconds)
	 */
	[HideInInspector]public float paralyzeTime = 1f;
	VirusState previousState;
	public void StartParalyze(Transform virusTrans){
		// first stop this object
		StopChase (this.transform);
		StopPosReceiver (this.transform);
		// memorize the previous state
		previousState = virusState;
		// then start this object after a given amont of time
		StartCoroutine (EndParaylzeIE (this.transform, paralyzeTime));
	}
	// IEnumerator for invoking a function with parameters
	IEnumerator EndParaylzeIE(Transform trans, float delay){
		yield return new WaitForSeconds (delay);
		// previous state has been memorized above
		virusState = previousState;
	}
	public void BreakEndParalyze(Transform virusTrans){
		StopCoroutine ("EndParaylzeIE");
	}


	public void StartReturn(Transform virusTrans){
		// first the virus need to turn to the return target
		StopPosReceiver(virusTrans);
		StartRot(virusTrans);
		StartCoroutine(EndRotIE(virusTrans, stopRotAngle));
	}
	IEnumerator EndRotIE(Transform virusTrans, float stopRotAng){
		// wait for the virus to turn towards the enemy...
		yield return new WaitUntil (() => {return withinAngle(transform.parent, stopRotAng);});
		StartChase (virusTrans);
	}
	public void BreakEndRot(Transform virusTrans){
		StopCoroutine ("EndRotIE");
	}


	public void StartPosReceiver(Transform virusTrans){
		VirusPosReceiver pr = GetComponent<VirusPosReceiver> ();
		if(pr){
			pr.enabled = true;
		}
	}
	public void StopPosReceiver(Transform virusTrans){
		VirusPosReceiver pr = GetComponent<VirusPosReceiver> ();
		if(pr){
			pr.enabled = false;
		}
	}
		
	public void SetTargetToNull(Transform virusTrans){
		if(tp){
			tp.SetNewTarget (null);
		}
	}

	public void StopStateControl(Transform virusTrans){
		this.enabled = false;
	}

	public void StartStateControl(Transform virusTrans){
		this.enabled = true;
		// default behaviour is to set status as return
		virusState = VirusState.Return;
	}

	public void SetReleasePos(Transform virusTrans){
		releasePos = transform.position;
	}


	bool withinAngle(Transform target, float angle){
		if(target == null){
			return false;
		}
		Vector3 dirToTarget = target.position - this.transform.position;
		Quaternion rot = Quaternion.FromToRotation (transform.up, dirToTarget);


		float angleZ = rot.eulerAngles.z;
		if(angleZ > 180f){
			angleZ -= 360f;
		}

		if(Mathf.Abs(angleZ) > angle / 2.0f){
			return false;
		}
		else{
			return true;
		}
	}

	void SetIsBullet(Transform target){
		hd.isBullet = true;
	}

	void UnsetIsBullet(Transform target){
		hd.isBullet = false;
	}

	void ReturnIsBulletLogic(Transform target){
		if(cs.controller == Controller.Boss){
			hd.isBullet = true;
		} else {
			hd.isBullet = false;
		}
	}

}
