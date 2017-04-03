using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatShoot : MonoBehaviour {
	
	public GameObject bulletPrefab;
	public Transform bulletParent;


	public float interval = 1f;

	bool shootTowardsPlayer = true;

	[ReadOnly]public Vector3 facing;

	private Coroutine shootCoroutine = null;

	AgentPatrol ap;
	// Use this for initialization
	void Start () {
		ap = GetComponent<AgentPatrol> ();
		if(facing == Vector3.zero){
			facing = transform.up;
		}
	}

	public void StartShoot(){
		if(shootCoroutine == null){
			shootCoroutine = StartCoroutine ("ShootInterval");
		}
	}

	public void StopShoot(){
		if(shootCoroutine != null){
			StopCoroutine (shootCoroutine);
			shootCoroutine = null;
		}
	}

	void OnDestroy(){
		StopShoot ();
	}


	IEnumerator ShootInterval(){
		while(true){
			fire ();
			yield return new WaitForSeconds (interval);
		}
	}

	void fire(){
		if(!bulletPrefab){
			return;
		}
		GameObject bullet = Instantiate (bulletPrefab, transform.position, transform.rotation);


		if (shootTowardsPlayer && ap && ap.playerTarget) {
			Vector3 dir2Target = ap.playerTarget.position - transform.position;
			dir2Target.Normalize ();
			bullet.transform.up = dir2Target;
		} else {
			bullet.transform.up = facing;
		}


		bullet.transform.parent = bulletParent;
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
