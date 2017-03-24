using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharge : MonoBehaviour {
	public Transform otherPlayer;
	public float chargeDistance = 1.0f;
	public float chargeSpeed = 20f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(otherPlayer == null){
			return;
		}
		float dist = Vector2.Distance (otherPlayer.position, transform.position);
		if(dist < chargeDistance){
			PlayerEnergy energyScript = GetComponent<PlayerEnergy> ();
			if(energyScript != null){
				energyScript.AddEnergy (chargeSpeed * Time.deltaTime);
			}
		}
	}
}
