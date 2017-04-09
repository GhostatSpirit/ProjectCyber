using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotParalyzedState : StateMachineBehaviour {

	[Range(0f, 1f)]
	public float maxSpeedFactor = 0f;

	float oldMaxSpeed;

	PolyNavAgent agent;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		agent = animator.gameObject.GetComponent<PolyNavAgent> ();
		if(agent){
			if (maxSpeedFactor == 0f) {
				agent.enabled = false;
			} else {
				oldMaxSpeed = agent.maxSpeed;
				agent.maxSpeed = oldMaxSpeed * maxSpeedFactor;
			}
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(agent){
			if (maxSpeedFactor == 0f) {
				agent.enabled = true;
			} else {
				agent.maxSpeed = oldMaxSpeed;
			}
		}
		agent.SetDestination (animator.transform.position);
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
