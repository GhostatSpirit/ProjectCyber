﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaConnected : StateMachineBehaviour {
	RoombaBehaviour roomba;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		roomba = animator.GetComponent<RoombaBehaviour> ();
		roomba.ResetVelocity ();

		switch(roomba.cs.controller){
		case Controller.Boss:
			roomba.SetPlayerTargets (animator.transform);
			break;
		case Controller.Hacker:
			roomba.SetEnemyTargets (animator.transform);
			break;
		}

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		if (roomba.cs.controller == Controller.Hacker) {
//			animator.SetBool ("targetInSight", true);
//			Debug.Log ("set targetInSight");
//		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
