using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCEnemyAim : StateMachineBehaviour {
	LaserCannonState state;
	LaserCannonStatus status;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		state = animator.GetComponent<LaserCannonState> ();
		status = animator.GetComponentInChildren<LaserCannonStatus> ();
		// turn on laser line
		if(state){
			state.EnemyUpdateAimLaser ();
			state.aimLaser.GetComponent<AimLaserUpdate> ().SnapPosition ();
			state.aimLaser.gameObject.SetActive (true);
		}
		if(status){
			 // status.EnableServerLine (state.playerLastPos);

		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		state.EnemyUpdateAimLaser ();
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// turn off the laser
		if(state){
			state.aimLaser.gameObject.SetActive (false);
		}
		if(status){
			status.DisableServerLine ();
		}
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
