using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : StateMachineBehaviour {
	AgentPatrol ap;
	PolyNavAgent agent;
	RepeatShoot shoot;
	[Range(0f, 45f)]
	public float startShootAngle = 5f;

	[Range(0f, 1f)]
	public float maxSpeedFactor = 0.25f;

	public float stoppingDistance = 0.1f;

	public float slowingDistance = 0.3f;
	public float accelerationRate = 15f;
	public float decelerationRate = 15f;

	float oldMaxSpeed;
	float oldStoppingDist;
	float oldSlowingDist;
	float oldAccelRate;
	float oldDecelRate;

	bool shooting = false;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		ap = animator.gameObject.GetComponent<AgentPatrol> ();
		agent = animator.gameObject.GetComponent<PolyNavAgent> ();
		shoot = animator.gameObject.GetComponent<RepeatShoot> ();

		oldMaxSpeed = agent.maxSpeed;
		oldStoppingDist = agent.stoppingDistance;
		oldSlowingDist = agent.slowingDistance;
		oldAccelRate = agent.accelerationRate;
		oldDecelRate = agent.decelerationRate;

		// SetNewAgent ();

		agent.SetDestination (ap.playerLastPos);


		if(shoot){
			UpdateTargetPos ();
			shoot.StopShoot ();
		}
		shooting = false;
//		if(shoot){
//			shoot.StartShoot ();
//			shooting = true;
//		}

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		agent.SetDestination (ap.playerLastPos);
		UpdateTargetPos ();

		if (shoot) {
			//Debug.Log ("Shooting: " + shooting.ToString ());
			if (shooting && ap.playerTarget == null) {
				// lost player target, stop shooting
				shoot.StopShoot ();
				SetOldAgent ();
				shooting = false;

			} else if (!shooting && ap.playerTarget != null) {
				// found player target, start shooting
//				Debug.Log (deltaAngle (ap.playerTarget, animator.transform));
				if (deltaAngle (ap.playerTarget, animator.transform) < startShootAngle) {
					shoot.StartShoot ();
					SetNewAgent ();
					shooting = true;
				}
			}
		}

		if (ap.agent.movingDirection.magnitude != 0f) {
			ap.facing = ap.agent.movingDirection;
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		SetOldAgent ();

		if(shoot){
			shoot.StopShoot ();
		}
		shooting = false;
	}

	float deltaAngle(Transform target, Transform animator){
		Vector3 dir2Target = target.position - animator.transform.position;
		dir2Target.Normalize ();


		if(!ap){
			return 180f;
		}

//		Debug.Log (dir2Target);
//		Debug.Log (ap.facing);

		Quaternion rot = Quaternion.FromToRotation (dir2Target, ap.facing);
		float angleZ = rot.eulerAngles.z;
		if(angleZ > 180f){
			angleZ -= 360f;
		}

		return Mathf.Abs (angleZ);
	}

	void SetNewAgent(){
		agent.maxSpeed = oldMaxSpeed * maxSpeedFactor;
		agent.stoppingDistance = stoppingDistance;
		agent.slowingDistance = slowingDistance;
		agent.accelerationRate = accelerationRate;
		agent.decelerationRate = decelerationRate;
	}

	void SetOldAgent(){
		agent.maxSpeed = oldMaxSpeed;
		agent.stoppingDistance = oldStoppingDist;
		agent.slowingDistance = oldSlowingDist;
		agent.accelerationRate = oldAccelRate;
		agent.decelerationRate = oldDecelRate;
	}

	void UpdateTargetPos(){
		shoot.targetPos = ap.playerLastPos;
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
