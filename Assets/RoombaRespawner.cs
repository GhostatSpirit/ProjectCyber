using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaRespawner : MonoBehaviour {
	public float respawnInterval = 5f;
	public Transform hacker;

	public GameObject roomba;

	Collider2D areaTrigger;
	public LayerMask movableMask;

	int roombaCount{
		get{
			int tempCount = 0;
			foreach(Transform child in transform){
				ObjectIdentity oi = child.GetComponent<ObjectIdentity> ();
				if(oi && oi.objType == ObjectType.Roomba){
					tempCount++;
				}
			}
			return tempCount;
		}
	}

	// Use this for initialization
	void Start () {
		areaTrigger = GetComponent<Collider2D> ();
		if(roombaCount == 0 && roomba){
			RespawnRoomba ();
		}

	}

	Coroutine respawnCoroutine;
	// Update is called once per frame
	void Update () {
		Debug.Log (roombaCount);
		if(roombaCount == 0 && respawnCoroutine == null){
			// start the respawnCoroutine
			respawnCoroutine = StartCoroutine (RespawnRoombaIE (respawnInterval));
		}
	}

	bool ObjectNearby(){
		if (!areaTrigger){
			return false;
		}
		else{
			return areaTrigger.IsTouchingLayers (movableMask);
		}
	}

	IEnumerator RespawnRoombaIE(float _respawnInterval){
		yield return new WaitForSeconds (_respawnInterval);
		yield return new WaitUntil (
			() => {
				return !ObjectNearby();
			}
		);
		RespawnRoomba ();
		respawnCoroutine = null;
	}


	void RespawnRoomba(){
		GameObject roombaGO = 
			Instantiate (roomba, transform.position, transform.rotation);
		roombaGO.transform.parent = transform;
		roombaGO.transform.localScale = new Vector3 (1f, 1f, 1f);
		// set boss and hacker
		ControlStatus cs = roombaGO.GetComponent<ControlStatus> ();
		if(cs){
			cs.Boss = this.transform;
			cs.Hacker = hacker;
		}
	}
}
