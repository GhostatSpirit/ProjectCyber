/* Written by Yang Liu,
 * Notify the boss when this virus is dead
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ControlStatus))]
public class NotifyBoss : MonoBehaviour {
	public float distroyDist = 100f;

	Transform controllerTrans;
	Transform bossTrans;
	HealthSystem hs;
	// Use this for initialization
	void Start () {
		controllerTrans = GetComponent<ControlStatus> ().controllerTransfrom;
		bossTrans = GetComponent<ControlStatus> ().Boss.transform;

		hs = GetComponent<HealthSystem> ();
		if(hs){
			hs.OnObjectDead += Notify;
		}
	}

	void Update(){
		// calcuate the distance between the virus and the controller
		if(controllerTrans == null){
			return;
		}
		float dist = Vector3.Distance (transform.position, controllerTrans.position);
		if(dist > distroyDist){
			hs.InstantDead ();
		}
	}

	// Update is called once per frame
	void Notify(Transform objTrans){
		VirusManager vm = (bossTrans != null) ? bossTrans.GetComponent<VirusManager> () : null;
		if(vm != null){
			vm.LoseOneVirus ();
		}
	}
}
