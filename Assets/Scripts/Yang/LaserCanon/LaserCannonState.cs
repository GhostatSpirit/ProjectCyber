using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.FastLineRenderer;
using TwoDLaserPack;


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
//	ServerPicker picker;

	Vector3 _facing = Vector3.up;
	[HideInInspector] public Vector3 facing{
		get{
			return _facing;
		}
		set{
			_facing = value.normalized;
		}
	}

	public Vector3 playerAimPos;

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

	public LineBasedLaser shootLaserLine{
		get{
			if (!shootLaser)
				return null;
			else
				return shootLaser.GetComponent<LineBasedLaser> ();
		}
	} 


	// variables for the shooting laser
	[HideInInspector] public bool damaging = false;
	Coroutine shootLaserCoroutine = null;
	public float damage = 10f;

	public PlayerHintUI playerHint;

	public void TurnOnHint(){
		if(playerHint){
			playerHint.hint = PlayerHintUI.HintStatus.PressB;
		}
	}

	public void TurnOffHint(){
		if(playerHint){
			playerHint.hint = PlayerHintUI.HintStatus.None;
		}
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		fov = GetComponentInChildren<FieldOfView> ();
		sp = GetComponent<LaserCannonSP> ();
		//picker = GetComponentInChildren<ServerPicker> ();

		playerAimPos = shootLaser.position;
	}

	//Coroutine resetCoroutine;

	public void SetEnemyColor(){
		shootLaserLine.laserLineRenderer.material = sp.shootMaterials.enemyMaterial;
		shootLaserLine.hitSparkParticleSystem = sp.shootParticles.enemyParticle;

		shootLaserLine.SetLaserState (false);

		aimLaserLine.startColor = sp.aimCP.enemyColor;
		aimLaserLine.endColor = sp.aimCP.enemyColor;
		aimLaserLine.sortingLayerName = "ControlLine";
	}

	public void SetPlayerColor(){
		shootLaserLine.laserLineRenderer.material = sp.shootMaterials.playerMaterial;
		shootLaserLine.hitSparkParticleSystem = sp.shootParticles.playerParticle;
		shootLaserLine.SetLaserState (false);

		aimLaserLine.startColor = sp.aimCP.playerColor;
		aimLaserLine.endColor = sp.aimCP.playerColor;
		aimLaserLine.sortingLayerName = "ControlLine";
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


	public void EnemyUpdateAimLaser(){
		Vector3 targetPos = GetLaserContact (shootLaser.position, playerLastPos, aimLaserMask);
		AimLaserUpdate alu = aimLaser.GetComponent<AimLaserUpdate> ();
		if(alu){
			alu.targetPos = targetPos;
		}
	}

	public void PlayerUpdateAimLaser(){
		Vector3 targetPos = GetLaserContact (shootLaser.position, playerAimPos, aimLaserMask);
		AimLaserUpdate alu = aimLaser.GetComponent<AimLaserUpdate> ();
		if(alu){
			alu.targetPos = targetPos;
		}
	}

	public void DisableShootLaser(){
		shootLaserLine.SetLaserState (false);
	}

	// functions for the shooting laser
	public void ShootLaser
	(Vector3 direction, float maxDistance, float fadeSeconds, float lifeTime, bool rotating = false){
		if (shootLaserCoroutine == null) {
			shootLaserCoroutine = 
				StartCoroutine (ShootLaserIE (direction, maxDistance, fadeSeconds, lifeTime, rotating));
		}
	}

	IEnumerator ShootLaserIE
	(Vector3 direction, float maxDistance, float fadeSeconds, float lifeTime, bool rotating){
		shootLaser.right = direction;
		shootLaserLine.maxLaserRaycastDistance = maxDistance;
		shootLaserLine.laserRotationEnabled = rotating;
		shootLaserLine.OnLaserHitTriggered += HurtTarget;

		yield return new WaitForFixedUpdate ();
		yield return new WaitForFixedUpdate ();
		shootLaserLine.SetLaserState (true);
        // ye added

		if (GetComponent<ItalianGunSound> ()) {
			GetComponent<ItalianGunSound> ().ShootSound (lifeTime);
		}

        damaging = true;
		//		yield return new WaitForSeconds (fadeSeconds);
		//		damaging = true;
		//
		//		yield return new WaitForSeconds (lifeTime - 2 * fadeSeconds);
		//		damaging = false;

		yield return new WaitForSeconds (lifeTime);

		damaging = false;
		shootLaserLine.laserRotationEnabled = false;
		shootLaserLine.SetLaserState (false);




		shootLaserLine.OnLaserHitTriggered -= HurtTarget;

		animator.SetTrigger ("finishShoot");
		shootLaserCoroutine = null;
	}

	void HurtTarget(RaycastHit2D hit){
		HealthSystem hs = hit.transform.GetComponentInParent<HealthSystem> ();
		if(hs){
			hs.Damage (damage);
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

	public float rotateSpeedFactor = 1f;
}
