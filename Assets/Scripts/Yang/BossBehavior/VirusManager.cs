using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusManager : MonoBehaviour {

	public GameObject virusPrefab;
	// the amount of virus that would be respawned at one time
	public int respawnCount = 4;

	public float respawnRadius = 3f;
	// the amount of virus that is alive for now
	public int currentCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(currentCount == 0){
			Respawn ();
		}
	}


	// Respawn the virus around a circle with the radius of respawnRadius
	void Respawn(){
		if(virusPrefab == null || respawnCount == 0){
			return;
		}
		float deltaAngle = 360f / respawnCount;
		float curAngle = 0f;
		for(int virusIndex = 0; virusIndex < respawnCount; virusIndex ++){
			float dirx = Mathf.Cos (curAngle * Mathf.Deg2Rad);
			float diry = Mathf.Sin (curAngle * Mathf.Deg2Rad);
			Vector3 virusDir = new Vector3 (dirx, diry, 0f);
			virusDir.Normalize ();

			Vector3 virusPosOffset = virusDir * respawnRadius;
			Vector3 virusNewPos = transform.position + virusPosOffset;

			Quaternion newRot = new Quaternion();
			newRot.eulerAngles = new Vector3 (0f, 0f, curAngle - 90f);
			// instantiate the virus
			//GameObject newVirus = Instantiate (virusPrefab, virusNewPos, newRot);
			Instantiate (virusPrefab, virusNewPos, newRot);
		
			curAngle += deltaAngle;
		}
		currentCount += respawnCount;
	}

	public void LoseOneVirus(){
		currentCount--;
	}
}
