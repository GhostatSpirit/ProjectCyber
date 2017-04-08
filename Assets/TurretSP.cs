using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSP : MonoBehaviour {
	public SpritePair baseSP;
	public SpritePair turretSP;

	public void SetPlayerSprite(){
		baseSP.SetPlayer ();
		turretSP.SetPlayer ();
	}

	public void SetEnemySprite(){
		baseSP.SetEnemy ();
		turretSP.SetEnemy ();
	}

	public void SetNoneSprite(){
		baseSP.SetNone ();
		turretSP.SetNone();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
