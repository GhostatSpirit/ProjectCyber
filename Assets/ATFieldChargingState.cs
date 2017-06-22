using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATFieldChargingState : StateMachineBehaviour {

	HackerFieldControl control;

	bool canceled = false;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		control = animator.transform.GetComponent<HackerFieldControl> ();
		control.SetChargeMoveSpeed ();
		canceled = false;
		animator.SetFloat ("chargeSpeed", 1.0f);

		control.PlayForward ();
		control.PlayChargeSound ();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(control.isButtonReleased && !canceled){
			// player released the button, cancel charging
			animator.SetFloat ("chargeSpeed", -0.5f);
			// animator.SetTrigger ("exitCharge");
			control.chargeCanceled = true;
			canceled = true;

			control.PlayBackward ();
			// control.StopChargeSound ();
		}

		// if charge has not been canceled, try consume energy
		if (!canceled) {
			if (!control.ConsumeEnergy ()) {
				// not enough energy, force canceling
				// player released the button, cancel charging
				animator.SetFloat ("chargeSpeed", -0.5f);
				// animator.SetTrigger ("exitCharge");
				control.chargeCanceled = true;
				canceled = true;

				control.PlayBackward ();
				// control.StopChargeSound ();
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// control.chargeCanceled = false;
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
