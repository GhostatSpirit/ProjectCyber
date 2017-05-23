using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannonStatus : MonoBehaviour {
	ControlStatus cs;
	LineUpdate lineUpdate;
	ServerPicker picker;
	Animator animator;

	// Use this for initialization
	void Start () {
		cs = GetComponent<ControlStatus> ();
		lineUpdate = GetComponent<LineUpdate> ();
		picker = GetComponent<ServerPicker> ();
		animator = GetComponentInParent<Animator> ();

		cs.OnLinkedByEnemy += DisableLine;


		cs.OnCutByPlayer += SetPlayerCut;
		cs.OnLinkedByPlayer += SetPlayerLink;

		HealthSystem hs = cs.Hacker.GetComponent<HealthSystem> ();
		if(hs){
			hs.OnObjectDead += SetPlayerUnLink;
			// hs.OnObjectDead += ForceDisconnect;

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EnableLine(){
		if(lineUpdate){
			lineUpdate.EnableLine ();
		}
	}

	public void DisableLine(){
		if(lineUpdate){
			lineUpdate.DisableLine ();
		}
	}

	public void DisableLine(Transform objTrans){
		if(lineUpdate){
			lineUpdate.DisableLine ();
		}
	}

	public void EnableServerLine(Vector3 lastPlayerPos){
	//	Debug.Log ("enable");
		if(picker && cs){
			cs.Boss = picker.GetFarthestFrom (lastPlayerPos);
		}
		StartCoroutine (EnableLineIE ());
	}

	public void DisableServerLine(){
		StartCoroutine (DisableLineIE ());
	}

	IEnumerator EnableLineIE(){
		yield return new WaitForEndOfFrame ();
		EnableLine ();
	}

	IEnumerator DisableLineIE(){
		yield return new WaitForEndOfFrame ();
		DisableLine ();
	}

	public void SetPlayerCut(Transform objTrans){
		if(animator){
			animator.SetTrigger ("playerCut");
		}
	}

	public void SetPlayerLink(Transform objTrans){
		if(animator){
			animator.SetTrigger ("playerLink");
		}
	}

	public void SetPlayerUnLink(Transform objTrans){
		if(animator){
			animator.SetTrigger ("playerUnlink");
		}
	}

	public void ForceDisconnect(Transform deadObject){
		cs.controller = Controller.None;
	}

	public void TurnOffHackerControl(Transform trans){
		CannonHackerControl control = transform.parent.GetComponent<CannonHackerControl> ();
		if(control){
			control.enabled = false;
		}
	}

	public void TurnOnHackerControl(Transform trans){
		CannonHackerControl control = transform.parent.GetComponent<CannonHackerControl> ();
		if(control){
			control.enabled = true;
		}
	}
}
