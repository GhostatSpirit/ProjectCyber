using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ChaseTarget))]
public class VirusTargetPicker : MonoBehaviour {
	/* A definition of all the target group this object could chase. */
	public enum Target {Player, Boss, None};
	public Target targetGroup = Target.Player;

	ObjectType[] playerTargets = { ObjectType.Virus, ObjectType.AI, ObjectType.Hacker };
	ObjectType[] bossTargets = { ObjectType.Boss, ObjectType.Virus };


//	public List<Transform> playerTargets;
//	public List<Transform> bossTargets;

	//VirusStateControl vs;
	FieldOfView fov;
	ChaseTarget ct;

	// Use this for initialization
	void Start () {
		//vs = GetComponent<VirusStateControl> ();
		fov = GetComponent<FieldOfView> ();
		ct = GetComponent<ChaseTarget> ();
	}

	public Transform PickTarget(){
		/* pick the specfic targets that:
		 * 0. if returning, choose parent obj as target
		 * 1. if chasing, choose targets belongs to targetGroup
		 * 2. target is within sight
		 * 3. if have multiple targets, choose the one which is the closest to this object
		 * 4. if targetGroup is none, return null
		 */

		Transform new_target = null;

		if (targetGroup == Target.Player || targetGroup == Target.Boss) {
			ObjectType[] targets;
			if(targetGroup == Target.Player){
				targets = playerTargets;
			} else{
				targets = bossTargets;
			}

			if(fov == null){
				Debug.LogError ("cannot find fov");
				new_target = null;
			}
			else{
				new_target = fov.ScanTargetInSight (targets);
			}

				
		} else{
			new_target = null;
		}

//		Debug.Log (new_target);

		return new_target;
	}

	public void SetNewTarget(Transform newTarget){
		if(ct){
			ct.target = newTarget;
		}
	}
		
	public void SetParentAsTarget(){
		if(ct){
			ct.target = transform.parent;
		}
	}

	// Update is called once per frame
	void Update () {
		// GetComponent<ChaseTarget> ().target = TargetPicker ();

	}
}
