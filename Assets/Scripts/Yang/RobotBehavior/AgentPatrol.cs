using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyNavAgent))]
public class AgentPatrol : MonoBehaviour {
	public PatrolPath patrolPath;

	int nodeCount = 0;
	int targetNode = 0;

	bool forwarding = true;

	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}

	private FacingSpriteSwitcher _switcher;
	public FacingSpriteSwitcher switcher{
		get{
			if(!_switcher){
				_switcher = GetComponent<FacingSpriteSwitcher> ();
			}
			return _switcher;
		}
	}

	private FieldOfView _fov;
	public FieldOfView fov{
		get{
			if(!_fov){
				_fov = GetComponent<FieldOfView> ();
			}
			return _fov;
		}
	}

	private RepeatShoot _shoot;
	public RepeatShoot shoot{
		get{
			if(!_shoot){
				_shoot = GetComponent<RepeatShoot> ();
			}
			return _shoot;
		}
	}

	Vector3 _facing;
	public Vector3 facing{
		get{
			return _facing;
		}
		set{
			if(value.magnitude == 0f){
				return;
			}

			if (switcher)  switcher.facing = value;
			if (fov)       fov.facing = value;
			if (shoot)     shoot.facing = value;

			_facing = value;
		}
	}
		
	public Vector3 targetNodePos{
		get{
			if(patrolPath && targetNode >= 0){
				return patrolPath.path [targetNode];
			}
			else{
				return Vector3.zero;
			}
		}
	}

	// FSM related variables
	Animator animator;
	//bool playerInSight = false;

	// Use this for initialization
	void Start () {
		//yield return new WaitForSeconds (0.5f);
		if (patrolPath) {
			nodeCount = patrolPath.path.Count;
		}
		animator = GetComponent<Animator> ();

		facing = transform.up;
	}

	void OnEnable(){
		agent.OnDestinationReached += ReachedNodeTrigger;
		agent.OnDestinationInvalid += PrintDestInvalid;
	}

	void OnDisable(){
		agent.OnDestinationReached -= ReachedNodeTrigger;
		agent.OnDestinationInvalid -= PrintDestInvalid;
	}

	public void ReachedNodeTrigger(){
		if(animator){
			animator.SetTrigger ("reachedDest");
		}
	}

	public void PrintDestInvalid(){
		Debug.Log ("Destination Invalid");
	}

	public void MoveTowardNode(){
		if (nodeCount > 0 && targetNode >= 0) {
			agent.SetDestination (patrolPath.path [targetNode]);
		} 
	}

	public void GetFirstNode(){
		targetNode = -1;
		if (patrolPath) {
			if (nodeCount != 0) {
				targetNode = 0;
				animator.SetTrigger ("updatedNode");
			}
			forwarding = true;
		}
	}

	public void GetClosestNode(){
		if (!patrolPath || nodeCount == 0) {
			targetNode = -1;
			return;
		}
		float minDist = Mathf.Infinity;
		int tempNode = -1;
		for(int i = 0; i < nodeCount; ++i){
			float dist = Vector3.Distance (transform.position, patrolPath.path [i]);
			if(dist < minDist){
				minDist = dist;
				tempNode = i;
			}
		}
		targetNode = tempNode;
		animator.SetTrigger ("updatedNode");
		// forwarding = true;
	}

	public void GetNextNode(){
		if (!patrolPath) {
			targetNode = -1;
			return;
		}

		int newNode = 0;
		if(forwarding){
			// move forwards along the path
			if(targetNode == nodeCount - 1){
				// we are at the last node
				newNode = (targetNode - 1) >= 0 ? targetNode - 1 : 0;
				forwarding = false;
			} else {
				// we are not at the last node
				newNode = targetNode + 1;
			}

		} else {
			// move backwards along the path
			if(targetNode == 0){
				// we are at the first node
				newNode = (targetNode + 1) <= nodeCount - 1 ? targetNode + 1 : 0;
				forwarding = true;
			} else {
				newNode = targetNode - 1;
			}
		}
			
		targetNode = newNode;
		animator.SetTrigger ("updatedNode");
	}


	// Update is called once per frame
	void Update () {


	}

	public Transform playerTarget;
	ObjectType[] playerTargets = { ObjectType.AI, ObjectType.Hacker };
	public Vector3 playerLastPos;

	void FixedUpdate(){
		playerTarget = fov.ScanTargetInSight (playerTargets);
		if(playerTarget){
			playerLastPos = playerTarget.position;
			animator.SetBool ("playerInSight", true);
		} else{
			animator.SetBool ("playerInSight", false);
		}
	}
}
