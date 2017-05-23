using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
	public Transform door;

	ControlStatus cs;

	// Use this for initialization
	void Start () {
		cs = GetComponent<ControlStatus> ();
		StartCoroutine (BindDoorActionIE ());
	}

	// Update is called once per frame
	void Update () {
		cs = GetComponent<ControlStatus> ();
	}


	IEnumerator BindDoorActionIE(){
		yield return new WaitUntil (() => {
			return (cs != null);
		});
		if (door) {
			BindDoorActions ();
		}
	}

	void BindDoorActions(){
		cs.OnCutByEnemy += StopDoorMovement;
		cs.OnCutByPlayer += StopDoorMovement;

		cs.OnLinkedByEnemy += StartDoorMovement;
		cs.OnLinkedByEnemy += CloseDoors;

		cs.OnLinkedByPlayer += StartDoorMovement;
		cs.OnLinkedByPlayer += OpenDoors;
	}

	void StopDoorMovement(Transform terminal){
		DoorMover[] doorMovers = door.GetComponentsInChildren<DoorMover>();
		foreach(DoorMover dm in doorMovers){
			dm.enabled = false;
		}
	}

	void StartDoorMovement(Transform terminal){
		DoorMover[] doorMovers = door.GetComponentsInChildren<DoorMover>();
		foreach(DoorMover dm in doorMovers){
			dm.enabled = true;
		}
	}

	void OpenDoors(Transform terminal){
		DoorMover[] doorMovers = door.GetComponentsInChildren<DoorMover>();
		foreach(DoorMover dm in doorMovers){
			dm.doorStatus = DoorMover.DoorStatus.Opening;
		}
	}

	void CloseDoors(Transform terminal){
		DoorMover[] doorMovers = door.GetComponentsInChildren<DoorMover>();
		foreach(DoorMover dm in doorMovers){
			dm.doorStatus = DoorMover.DoorStatus.Closing;
		}
	}


}
