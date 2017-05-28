using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLightController : MonoBehaviour {
	public bool blinkBrightOnce = false;

	Animator animator;
	ControlStatus _cs;
	ControlStatus cs{
		get{
			if (!_cs)
				_cs = GetComponentInParent<ControlStatus> ();
			return _cs;
		}
	}
	// Use this for initialization
	IEnumerator Start () {
		animator = GetComponent<Animator> ();

		yield return new WaitUntil (() => cs != null);

		// initialize the animator state
		switch(cs.controller){
		case Controller.Boss:
			SetOff ();
			break;
		case Controller.None:
			SetBlink ();
			break;
		case Controller.Hacker:
			SetBright ();
			break;
		}

		// add actions
		cs.OnCutByEnemy += SetBlink;
		cs.OnCutByPlayer += SetBlink;
		cs.OnLinkedByPlayer += SetBright;
		cs.OnLinkedByEnemy += SetOff;

		yield return null;
	}

	void SetOff(Transform objTrans = null){
		animator.SetTrigger ("setOff");
	}

	bool brighted = false;
	void SetBright(Transform objTrans = null){
		if (!brighted) {
			animator.SetTrigger ("setBright");
			if(blinkBrightOnce){
				brighted = true;
			}
		}

	}


	bool blinked = false;
	void SetBlink(Transform objTrans = null){
		if (!blinked) {
			animator.SetTrigger ("setBlink");
			if (blinkBrightOnce) {
				blinked = true;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
