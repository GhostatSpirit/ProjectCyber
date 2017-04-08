using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCPlayerAim : StateMachineBehaviour {
	LaserCannonState state;
	ControlStatus cs;
	CannonHackerControl control;
	FacingSpriteSwitcher hackerSwitcher;

	PlayerAim hackerAim;

	public float moveSpeed = 2f;

	public float minRadius = 1f;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		state = animator.GetComponent<LaserCannonState> ();
		cs = animator.GetComponentInChildren<ControlStatus> ();
		control = animator.GetComponent<CannonHackerControl> ();
		hackerSwitcher = cs.Hacker.GetComponent<FacingSpriteSwitcher> ();
		hackerAim = cs.Hacker.GetComponentInChildren<PlayerAim> ();

//		// initially point to hacker
//		state.playerAimPos = cs.Hacker.position;

		if(state){
			state.PlayerUpdateAimLaser ();
			state.aimLaser.GetComponent<AimLaserUpdate> ().SnapPosition ();
			state.aimLaser.gameObject.SetActive (true);
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// update state facing
		float step = moveSpeed * Time.deltaTime;

		Vector3 targetDir = control.direction;
//		if(targetDir.magnitude != 0f){
//			// update facing vector in state
//			state.facing = Vector3.RotateTowards(state.facing, targetDir, step, Mathf.Infinity);
//		}
		Vector3 originPos = state.playerAimPos + step * targetDir;
		Vector3 center2Target = originPos - state.shootLaser.position;
		center2Target = Vector3.ClampMagnitude (center2Target, state.fov.radius);

		if(center2Target.magnitude < minRadius){
			center2Target = center2Target.normalized * minRadius;
		}

		Vector3 finalPos = state.shootLaser.position + center2Target;
		state.playerAimPos = finalPos;

		state.PlayerUpdateAimLaser ();

		// let the hacker face to the aim point
		hackerSwitcher.facingPosition = state.aimLaser.position;
		if (hackerAim) {
			hackerAim.transform.up = 
				(state.aimLaser.position - cs.Hacker.position).normalized;
		}

		if(control.pressedShoot){
			// enter the shoot state
			animator.SetTrigger ("playerShoot");
		}

		if(control.pressedExit){
			// enter the idle state
			animator.SetTrigger ("playerUnlink");
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// turn off the laser
		if(state){
			state.aimLaser.gameObject.SetActive (false);
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
