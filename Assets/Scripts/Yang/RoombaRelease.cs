using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaRelease : StateMachineBehaviour {
	RoombaBehaviour roomba;
	HurtAndDamage hd;
	Vector3 targetPos;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		roomba = animator.GetComponent<RoombaBehaviour> ();
		hd = animator.GetComponent<HurtAndDamage> ();
		hd.canHurtOther = true;
		// decide where to shoot the roomba at
		targetPos = roomba.targetLastPos;
		// guess where the player might be at after fade seconds
//		if(roomba.target){
//			Rigidbody2D rb = roomba.target.GetComponent<Rigidbody2D> ();
//			if(rb){
//				targetPos = shootPos + (Vector3)(rb.velocity * fadeSeconds);
//			}
//		}
		Vector3 targetDir = animator.transform.up;
		if (roomba.cs.controller == Controller.Boss) {
			targetDir = (targetPos - animator.transform.position).normalized;
		} else if(roomba.cs.controller == Controller.Hacker) {
			Debug.Log (targetDir);
			targetDir = roomba.incomingVelocity.normalized;
		}

		roomba.body.velocity = targetDir * roomba.thrust;

		animator.SetFloat ("velocity", roomba.body.velocity.magnitude);

		// enable collision check for explosion
		roomba.checkCollision = true;
		// reset distToTarget
		animator.SetFloat ("distToTarget", 200f);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetFloat ("velocity", roomba.body.velocity.magnitude);

		float distToTarget = Vector3.Distance (animator.transform.position, targetPos);
		animator.SetFloat ("distToTarget", distToTarget);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		hd.canHurtOther = false;

		roomba.checkCollision = false;
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
