using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : MonoBehaviour {

	FieldOfView _fov;

	public FieldOfView fov{
		get{
			if (!_fov)
				_fov = GetComponentInChildren<FieldOfView> ();
			return _fov;
		}
	}

	Animator _animator;
	public Animator animator{
		get{
			if(!_animator){
				_animator = GetComponent<Animator> ();
			}
			return _animator;
		}
	}

	public void SetTargetsToPlayer(){
		targets = playerTargets;
	}

	public void SetTargetsToEnemy(){
		targets = enemyTargets;
	}

	// Use this for initialization
	void Start () {
		targets = playerTargets;
	}

	ObjectType[] playerTargets = { ObjectType.AI, ObjectType.Hacker };
	ObjectType[] enemyTargets = { ObjectType.Virus, ObjectType.Robot };

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
			
	}

	public void SetTargetDist(float dist){
		animator.SetFloat ("targetDist", dist);
	}
}
