using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour {
	private ControlStatus _cs;
	public ControlStatus cs{
		get
		{
			if (!_cs)
				_cs = GetComponent<ControlStatus>();
			return _cs;			
		}
	}

	private Animator _animator;
	public Animator animator{
		get{
			if (!_animator)
				_animator = GetComponent<Animator> ();
			return _animator;
		}
	}

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitUntil 
		(
			() => {
			return (cs != null && animator != null);
			}
		);

		cs.OnCutByPlayer += SetLineCut;

		cs.OnLinkedByPlayer += DisableLine;
		cs.OnLinkedByPlayer += SetBulletHit;

		cs.OnLinkedByEnemy += EnableLine;
	}

	void SetLineCut(Transform objTrans){
		animator.SetTrigger ("lineCut");
	}

	void SetBulletHit(Transform objTrans){
		animator.SetTrigger ("hackerBulletHit");
	}

	void DisableLine(Transform objTrans){
		LineUpdate lu = GetComponent<LineUpdate> ();
		if(lu){
			lu.enabled = false;
		}
		foreach(Transform child in objTrans){
			ObjectIdentity oi = child.GetComponent<ObjectIdentity> ();
			if(oi && oi.objType == ObjectType.Line){
				child.gameObject.SetActive (false);
			}
		}
	}

	void EnableLine(Transform objTrans){
		LineUpdate lu = GetComponent<LineUpdate> ();
		if(lu){
			lu.enabled = true;
		}
		foreach(Transform child in objTrans){
			ObjectIdentity oi = child.GetComponent<ObjectIdentity> ();
			if(oi && oi.objType == ObjectType.Line){
				child.gameObject.SetActive (true);
			}
		}
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
