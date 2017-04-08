using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPicker : MonoBehaviour {
	public Transform serverParent;
	public List<Transform> servers;

	ControlStatus cs;

	// Use this for initialization
	void Start () {
		// add the servers from serverParent to the servers list
		if(serverParent != null){
			foreach(Transform child in serverParent){
				servers.Add (child);
			}
		}

		cs = GetComponentInChildren<ControlStatus> ();
	}

	public Transform GetNearestFromSelf(){
		float minDist = Mathf.Infinity;
		Transform nearest = null;
		foreach(Transform server in servers){
			float dist = Vector3.Distance (server.position, transform.position);
			if(dist < minDist){
				minDist = dist;
				nearest = server;
			}
		}
		return nearest;
	}

	public Transform GetFarthestFrom(Transform target){
		float maxDist = 0f;
		Transform farthest = null;
		foreach(Transform server in servers){
			float dist = Vector3.Distance (server.position, target.position);
			if(dist > maxDist){
				maxDist = dist;
				farthest = server;
			}
		}
		return farthest;
	}

	public Transform GetFarthestFrom(Vector3 targetPos){
		float maxDist = 0f;
		Transform farthest = null;
		foreach(Transform server in servers){
			float dist = Vector3.Distance (server.position, targetPos);
			if(dist > maxDist){
				maxDist = dist;
				farthest = server;
			}
		}
		return farthest;
	}

	public Transform SetNearestFromSelf(){
		Transform server = GetNearestFromSelf ();
		if(server && cs){
			cs.Boss = server;
		}
		return server;
	}

	public Transform SetFarthestFrom(Transform target){
		Transform server = GetFarthestFrom (target);
		if(server && cs){
			cs.Boss = server;
		}
		return server;
	}

	public Transform SetFarthestFrom(Vector3 targetPos){
		Transform server = GetFarthestFrom (targetPos);
		if(server && cs){
			cs.Boss = server;
		}
		return server;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
