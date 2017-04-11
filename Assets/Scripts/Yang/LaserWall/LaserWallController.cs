using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWallController : MonoBehaviour {
	public Transform laserWall;

	bool enableWhenDisconnect = false;
	WallLaser wallLaser;
//	WallLaser wallLaser{
//		get{
//			if(laserWall == null){
//				return null;
//			}
//			if(_wallLaser == null){
//				_wallLaser = laserWall.GetComponentInChildren<WallLaser> ();
//			}
//			return _wallLaser;
//		}
//	}
	ControlStatus cs;
	// Use this for initialization
	void Start () {
		cs = GetComponent<ControlStatus> ();
		wallLaser = laserWall.GetComponent<WallLaser> ();
		if (laserWall != null) {
			StartCoroutine (BindLaserWallActionIE ());
		}


	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (wallLaser);
	}


	IEnumerator BindLaserWallActionIE(){
		yield return new WaitUntil (() => {
			return (cs != null) && (wallLaser != null) ;
		});
		if (cs && wallLaser) {
			BindLaserWallActions ();
		}
	}

	void BindLaserWallActions(){
//		Debug.Log ("binding actions");
		if (enableWhenDisconnect) {
			cs.OnCutByEnemy += wallLaser.DrawJitterLine;
			cs.OnCutByEnemy += wallLaser.EnableCollider;
		}

		cs.OnCutByPlayer += wallLaser.DrawJitterLine;

		cs.OnLinkedByEnemy += wallLaser.DrawDefaultLine;

		cs.OnLinkedByPlayer += wallLaser.ClearLine;
		cs.OnLinkedByPlayer += wallLaser.DisableCollider;
	}

	void DrawJitterLine(Transform trans){
//		Debug.Log ("controller: DrawJitter");
		wallLaser.DrawJitterLine (trans);
	}

	void DrawDefaultLine(Transform trans){
//		Debug.Log ("controller: Default");
		wallLaser.DrawDefaultLine (trans);
	}

	void ClearLine(Transform trans){
//		Debug.Log ("controller: Clear");
		wallLaser.ClearLine (trans);
	}

	void EnableCollider(Transform trans){
//		Debug.Log ("controller: Enable");
		wallLaser.EnableCollider (trans);
	}

	void DisableCollider(Transform trans){
//		Debug.Log ("controller: Disable");
		wallLaser.DisableCollider (trans);
	}

}
