using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.FastLineRenderer;

public class LaserCannonState : MonoBehaviour {

	Animator animator;
	FieldOfView fov;
	LaserCannonSP sp;

	public Transform aimLaser;
	public Transform shootLaser;

	public LineRenderer aimLaserLine{
		get{
			if (!aimLaser)
				return null;
			else
				return aimLaser.GetComponent<LineRenderer> ();
		}
	}

	public FastLineRenderer shootLaserLine{
		get{
			if (!shootLaser)
				return null;
			else
				return shootLaser.GetComponent<FastLineRenderer> ();
		}
	} 

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		fov = GetComponentInChildren<FieldOfView> ();
		sp = GetComponent<LaserCannonSP> ();
	}

	Coroutine resetCoroutine;

	public void SetEnemyColor(){
		shootLaserLine.GlowColor = sp.shootTintCP.enemyColor;
		shootLaserLine.TintColor = sp.shootTintCP.enemyColor;
//		if(resetCoroutine == null){
//			resetCoroutine = StartCoroutine ("ResetLineIE");
//		}

		aimLaserLine.startColor = sp.aimCP.enemyColor;
		aimLaserLine.endColor = sp.aimCP.enemyColor;
	}

	public void SetPlayerColor(){
		shootLaserLine.GlowColor = sp.shootTintCP.playerColor;
		shootLaserLine.TintColor = sp.shootTintCP.playerColor;
//		if(resetCoroutine == null){
//			resetCoroutine = StartCoroutine ("ResetLineIE");
//		}

		aimLaserLine.startColor = sp.aimCP.playerColor;
		aimLaserLine.endColor = sp.aimCP.playerColor;
	}

	IEnumerator ResetLineIE(){
		yield return new WaitForEndOfFrame ();
		shootLaserLine.Reset ();
		resetCoroutine = null;
	}

	public LayerMask laserMask;

	public Vector3 GetLaserContactPoint(Vector3 start, Vector3 end){
		return Vector3.zero;
	}

	[ReadOnly]public Transform playerTarget;
	ObjectType[] playerTargets = { ObjectType.AI, ObjectType.Hacker };
	[ReadOnly]public Vector3 playerLastPos;

	void FixedUpdate(){
		playerTarget = fov.ScanTargetInSight (playerTargets);
		if(playerTarget){
			playerLastPos = playerTarget.position;
			animator.SetBool ("playerInSight", true);
		} else{
			animator.SetBool ("playerInSight", false);
		}
	}
}
