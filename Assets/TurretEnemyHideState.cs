using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyHideState : StateMachineBehaviour {
	Animator animator;
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

	TurretState state;
	TurretSP sp;
	LineUpdate update;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.animator = animator;
		state = animator.GetComponentInChildren<TurretState> ();
		sp = animator.GetComponent<TurretSP> ();
		update = animator.GetComponentInChildren<LineUpdate> ();

		sp.turretSP.SetNone ();
		update.DisableLine ();

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (shoot && state.targetInSight) {
			float dist = Vector3.Distance (shoot.transform.position, state.targetLastPos);
			animator.SetFloat ("targetDist", dist);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		update.DisableLine ();
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
