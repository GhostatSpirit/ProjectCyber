using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaExplosion : StateMachineBehaviour {
	// RoombaBehaviour roomba;
	HealthSystem hs;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//roomba = animator.GetComponent<RoombaBehaviour> ();
		//roomba.StartExplosion();
		hs = animator.GetComponent<HealthSystem> ();
		if(hs){
			hs.InstantDead ();
		}
//		
//		roomba.body.velocity = Vector3.zero;
//
//		if(roomba.explosion != null){
//			GameObject explosionGO = Instantiate (roomba.explosion, roomba.transform.position, 
//				Quaternion.Euler (0f, 0f, 0f), roomba.explosionParent);
//			ParticleLayerSetter setter = explosionGO.GetComponentInChildren<ParticleLayerSetter> ();
//			SpriteRenderer sr = animator.GetComponent<SpriteRenderer> ();
//
//			if(setter && sr){
//				setter.SetSortingLayer (sr.sortingLayerID);
//			}
//			Destroy (animator.gameObject);
//		}

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// roomba.StopExplosion ();
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
