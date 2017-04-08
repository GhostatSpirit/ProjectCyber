using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.FastLineRenderer;

public class LCEnemyShoot : StateMachineBehaviour {
	Vector3 shootPos;
	LaserCannonState state;
	Vector3 shootDir;

	Vector3 laserStart;
	Vector3 laserEnd;

	public float maxDistance = 50f;

	[Range(0f, Mathf.Infinity)]
	public float extraDistance = 0f;

	public float fadeSeconds = 0.5f;
	public float lifeTime = 3f;

	bool damaging{
		get{
			if (state)
				return state.damaging;
			else
				return false;
		}
	}

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		state = animator.GetComponent<LaserCannonState> ();
		// get the start pos of the laser
		laserStart = state.shootLaser.position;
		// decide where to shoot the laser at
		shootPos = state.aimLaser.position;
		// guess where the player might be at after fade seconds
		if(state.playerTarget){
			Rigidbody2D rb = state.playerTarget.GetComponent<Rigidbody2D> ();
			if(rb){
				shootPos = shootPos + (Vector3)(rb.velocity * fadeSeconds);
			}
		}
		shootDir = (shootPos - laserStart).normalized;
		// the cannon at least can shoot $radius units 
		maxDistance = Mathf.Max (maxDistance, state.fov.radius);

		state.ShootLaser (shootDir, maxDistance, fadeSeconds, lifeTime);

	}



	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(state.playerTarget){
			state.shootLaserLine.targetGo = state.playerTarget.gameObject;
		}
		else{
			state.shootLaserLine.targetGo = null;
		}

		if(damaging){
			// shoot a raycast between startPos and endPos
			// do damage to every target object
		}
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
