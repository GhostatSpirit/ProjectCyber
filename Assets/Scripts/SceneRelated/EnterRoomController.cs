using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

public class EnterRoomController : MonoBehaviour {
	public Collider2D hackerCollider;
	public Collider2D aiCollider;

	Collider2D roomTrigger;

	public Transform doorController;
	public Transform bossTrans;
	public Transform roombaBaseParent;

	public float playerStopTime = 2f;


	public BossHealth bossHealth;

	public Transform oldRevivePoint;
	public Transform newRevivePoint;


	public event Action OnPlayersEnterRoom;


	bool enteredRoom = false;
	// Use this for initialization
	void Start () {
		roomTrigger = GetComponent<Collider2D> ();
		InitEnterRoom ();
	}

	void InitEnterRoom(){
		OnPlayersEnterRoom += StartBossScene;
	}

	Coroutine routine;
	void StartBossScene(){
		if(routine == null){
			routine = StartCoroutine (StartSceneIE ());
		}
	}

	void CloseDoor(){
		ControlStatus cs = doorController.GetComponent<ControlStatus> ();
		cs.controller = Controller.Boss;
	}


	IEnumerator StartSceneIE(){
		CloseDoor ();

		PlayerControl hackerControl = hackerCollider.transform.GetComponent<PlayerControl> ();
		PlayerControl AIControl = aiCollider.transform.GetComponent<PlayerControl> ();

		// turn off controls
		if (hackerControl)
			hackerControl.canControl = false;
		if (AIControl)
			AIControl.canControl = false;


		// waiting for the door to close
		yield return new WaitForSeconds (playerStopTime);

		StartRespawnRoomba ();
		StartRespawnVirus ();
		if(bossHealth){
			bossHealth.bossAppear = true;
		}

		SwitchRevivePoint ();


		if (hackerControl)
			hackerControl.canControl = true;
		if (AIControl)
			AIControl.canControl = true;

	}

	void StartRespawnRoomba(){
		if(roombaBaseParent == null){
			return;
		}

		RoombaRespawner[] respawners = roombaBaseParent.GetComponentsInChildren<RoombaRespawner> ();

		foreach(RoombaRespawner respawner in respawners){
			if(respawner){
				respawner.enabled = true;
			}
		}
	}

	void StartRespawnVirus(){
		if(!bossTrans){
			return;
		}
		VirusManager manager = bossTrans.GetComponent<VirusManager> ();
		if(manager){
			manager.enabled = true;
		}
	}

	void SwitchRevivePoint(){
		oldRevivePoint.gameObject.SetActive (false);
		newRevivePoint.gameObject.SetActive (true);
	}


	void FixedUpdate(){
		// Debug.Log (Time.fixedDeltaTime);
		if(!hackerCollider || !aiCollider || !roomTrigger){
			return;
		}
		if(roomTrigger.IsTouching(hackerCollider) 
			&& roomTrigger.IsTouching(aiCollider) && !enteredRoom){
			// both the hacker and ai enters this room
			if(OnPlayersEnterRoom != null){
				OnPlayersEnterRoom ();
			}
			enteredRoom = true;
		}
	}

}
