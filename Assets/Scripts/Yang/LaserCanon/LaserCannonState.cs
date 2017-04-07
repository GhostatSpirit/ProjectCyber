using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.FastLineRenderer;

[System.Serializable]
public struct LineProperty{
	public float lineRadius;
	public float glowWidthMultiplier;
	public float glowIntensity;
	public float jitterMultiplier;

}

public class LaserCannonState : MonoBehaviour {

	Animator animator;
	[HideInInspector]public FieldOfView fov;
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


	// variables for the shooting laser
	public LineProperty shootProperty;
	[HideInInspector] public bool damaging = false;
	Coroutine shootLaserCoroutine = null;

	// functions for the shooting laser
	public void ShootLaser(Vector3 laserStart, Vector3 laserEnd, float fadeSeconds, float lifeTime){
		if (shootLaserCoroutine == null) {
			shootLaserCoroutine = 
				StartCoroutine (ShootLaserIE (laserStart, laserEnd, fadeSeconds, lifeTime));
		}
	}

	IEnumerator ShootLaserIE(Vector3 laserStart, Vector3 laserEnd, float fadeSeconds, float lifeTime){
		FastLineRenderer lr = shootLaserLine;
		yield return new WaitForEndOfFrame ();

		// reset the fast line renderer
		lr.Reset ();
		lr.JitterMultiplier = shootProperty.jitterMultiplier;

		FastLineRendererProperties props = new FastLineRendererProperties ();
		props.Start = laserStart;
		props.End = laserEnd;

		props.Radius = shootProperty.lineRadius;
		props.GlowIntensityMultiplier = shootProperty.glowIntensity;
		props.GlowWidthMultiplier = shootProperty.glowWidthMultiplier;

		props.SetLifeTime (lifeTime, fadeSeconds);

		lr.AddLine (props, true, true);
		lr.Apply ();

		yield return new WaitForSeconds (fadeSeconds);
		damaging = true;

		yield return new WaitForSeconds (lifeTime - 2 * fadeSeconds);
		damaging = false;

		yield return new WaitForSeconds (fadeSeconds);
		animator.SetTrigger ("finishShoot");
		shootLaserCoroutine = null;
	}


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		fov = GetComponentInChildren<FieldOfView> ();
		sp = GetComponent<LaserCannonSP> ();
	}

	//Coroutine resetCoroutine;

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
	}

	public LayerMask aimLaserMask;
	public LayerMask shootLaserMask;

	public Vector3 GetLaserContact(Vector3 start, Vector3 end, LayerMask mask){
		ContactFilter2D filter = new ContactFilter2D ();
		filter.useTriggers = false;
		filter.useLayerMask = true;
		filter.useDepth = false;
		filter.useNormalAngle = false;
		filter.SetLayerMask (mask);

		Vector3 dir = (end - start).normalized;
		float dist = (end - start).magnitude;
		RaycastHit2D[] hits = new RaycastHit2D[1];
		int count = Physics2D.Raycast (start, dir, filter, hits, dist);

		if(count == 0){
			return end;
		}
		else{
			return hits [0].point;
		}
	}

	public Vector3 GetLaserContact(Vector3 start, Vector3 dir, float maxDist, LayerMask mask){
		if(dir.magnitude > 1f){
			dir.Normalize ();
		}

		ContactFilter2D filter = new ContactFilter2D ();
		filter.useTriggers = false;
		filter.useLayerMask = true;
		filter.useDepth = false;
		filter.useNormalAngle = false;
		filter.SetLayerMask (mask);

		RaycastHit2D[] hits = new RaycastHit2D[1];
		int count = Physics2D.Raycast (start, dir, filter, hits, maxDist);

		if(count == 0){
			return start + dir * maxDist;
		}
		else{
			return hits [0].point;
		}
	}


	public void UpdateAimLaser(){
		Vector3 targetPos = GetLaserContact (shootLaser.position, playerLastPos, aimLaserMask);
		AimLaserUpdate alu = aimLaser.GetComponent<AimLaserUpdate> ();
		if(alu){
			alu.targetPos = targetPos;
		}
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
