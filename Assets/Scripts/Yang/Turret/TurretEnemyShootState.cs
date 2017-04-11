using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyShootState : StateMachineBehaviour {
	Animator animator;
	TurretSP sp;
	TurretState state;
	LineUpdate _lineUpdate;
	LineUpdate lineUpdate{
		get{
			if (!animator)
				return null;
			if(!_lineUpdate)
				_lineUpdate = animator.GetComponentInChildren<LineUpdate> ();
			return _lineUpdate;
		}
	}
	RepeatShoot _shoot;
	RepeatShoot shoot{
		get{
			if (!animator)
				return null;
			if(!_shoot)
				_shoot = animator.GetComponentInChildren<RepeatShoot> ();
			return _shoot;
		}
	}

	bool shooting = false;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// set enemy sprites
		this.animator = animator;
		sp = animator.GetComponent<TurretSP> ();
		state = animator.GetComponentInChildren<TurretState> ();

		if(lineUpdate){
			lineUpdate.EnableLine ();
		}

		if(shoot){
			shoot.StopShoot ();
			UpdateTargetPos ();
		}

		shooting = false;

		sp.SetEnemySprite ();
		state.SetTargetsToPlayer ();


	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			
		if (shoot) {
			float dist = Vector3.Distance (shoot.transform.position, state.targetLastPos);
			animator.SetFloat ("targetDist", dist);
			UpdateTargetPos ();
			//Debug.Log ("Shooting: " + shooting.ToString ());
			if (shooting && state.target == null) {
				// lost player target, stop shooting
				shoot.StopShoot ();
				shooting = false;
//				Debug.Log("stopshoot");

			} else if (!shooting && state.target != null) {
				// found player target, start shooting
				//				Debug.Log (deltaAngle (ap.playerTarget, animator.transform));
				shoot.StartShoot ();
				shooting = true;
			}

		}
	}
		

	void UpdateTargetPos(){
		shoot.targetPos = state.targetLastPos;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(shoot){
			shoot.StopShoot ();
		}
		shooting = false;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
