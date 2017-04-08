using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardState : StateMachineBehaviour {
	public float rotateSpeed = 300f;

	AgentPatrol ap;

	Vector3 dir2Target;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		ap = animator.gameObject.GetComponent<AgentPatrol> ();

		dir2Target = ap.targetNodePos - animator.transform.position;
		dir2Target.Normalize ();

		animator.SetFloat ("deltaAngle", deltaAngle (dir2Target));

		// rotate towards the target node

//		float rotSpeedRad = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
//
//		ap.facing = 
//			Vector3.RotateTowards (ap.facing, dir2Target, rotSpeedRad, Mathf.Infinity);

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// update dir2Target
		dir2Target = ap.targetNodePos - animator.transform.position;
		dir2Target.Normalize ();

		animator.SetFloat ("deltaAngle", deltaAngle (dir2Target));


		float rotSpeedRad = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
		// rotate towards the target node
		ap.facing = Vector3.RotateTowards (ap.facing, dir2Target, rotSpeedRad, Mathf.Infinity);
		//Debug.Log(ap.facing);
	}

	float deltaAngle(Vector3 dir2Target){
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
