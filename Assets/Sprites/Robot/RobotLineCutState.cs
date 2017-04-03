using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// stop the robot from moving, but still let it rotate and shoot if player in sight
public class RobotLineCutState : StateMachineBehaviour {

	AgentPatrol ap;
	PolyNavAgent agent;
	RepeatShoot shoot;

	public float rotateSpeed = 100f;

	[Range(0f, 45f)]
	public float startShootAngle = 15f;

	bool shooting = false;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		ap = animator.gameObject.GetComponent<AgentPatrol> ();
		agent = animator.gameObject.GetComponent<PolyNavAgent> ();
		shoot = animator.gameObject.GetComponent<RepeatShoot> ();

		if(agent){
			agent.enabled = false;
		}

		if(shoot){
			shoot.StopShoot ();
		}
		shooting = false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (ap.playerTarget != null) {
			// found player, rotate towards it
			Vector3 dir2Target = ap.playerLastPos - animator.transform.position;
			dir2Target.Normalize ();

			float deltaAngle = DeltaAngle (dir2Target);

			if (shoot) {
				if (!shooting && deltaAngle <= startShootAngle) {
					shooting = true;
					shoot.StartShoot ();
				} 
			}
				
			float rotSpeedRad = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
			// rotate towards the target node
			ap.facing = Vector3.RotateTowards (ap.facing, dir2Target, rotSpeedRad, Mathf.Infinity);

		} else if(shooting){
			if (shoot) {
				shoot.StopShoot ();
				shooting = false;
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(shoot){
			shoot.StopShoot ();
		}
		shooting = false;

		if (agent) {
			agent.enabled = true;
		}
		agent.SetDestination (animator.transform.position);
		animator.ResetTrigger ("reachedDest");
	}

	float DeltaAngle(Vector3 dir2Target){
		if(!ap){
			return 180f;
		}

		Quaternion rot = Quaternion.FromToRotation (dir2Target, ap.facing);
		float angleZ = rot.eulerAngles.z;
		if(angleZ > 180f){
			angleZ -= 360f;
		}

		return Mathf.Abs (angleZ);
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
