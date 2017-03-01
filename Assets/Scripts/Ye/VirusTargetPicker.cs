using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ChaseTarget))]
public class VirusTargetPicker : MonoBehaviour {
	/* A definition of all the target group this object could chase. */
	public enum Target {Player, Boss, None};
	public Target targetGroup = Target.Player;

	public List<Transform> playerTargets;
	public List<Transform> bossTargets;

	Transform PickTarget(){
		/* pick the specfic target that:
		 * 1. belongs to targetGroup
		 * 2. is closest to this object
		 * 3. if targetGroup is none, return null
		 */
		Transform new_target = null;

		if (targetGroup == Target.Player || targetGroup == Target.Boss) {
			List<Transform> targets;
			if(targetGroup == Target.Player){
				targets = playerTargets;
			} else{
				targets = bossTargets;
			}

			if (targets.Count != 0) {
				float dist = Mathf.Infinity;
				foreach (Transform trans in targets) {
					float newDist = Vector3.Distance (this.transform.position, trans.position);
					if (newDist < dist) {
						new_target = trans;
					}
				}
			} else {
				new_target = null;
			}
		} else{
			new_target = null;
		}
		return new_target;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<ChaseTarget> ().target = PickTarget ();
	}
}
