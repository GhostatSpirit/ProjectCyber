// when control by boss, shoot a raycast every update()
// from virus to boss(parent),
// and check whether a darting AI is between this virus and boss

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ControlStatus))]
public class LineChecker : MonoBehaviour {

	ControlStatus cs;
	public LayerMask playerMask;

	// Use this for initialization
	void Start () {
		cs = GetComponent<ControlStatus> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(cs.controller == Controller.Boss){
			// if controlled by boss
			float dist = Vector3.Distance (transform.position, transform.parent.position);
			Vector3 dir = transform.parent.position - transform.position;
			dir.Normalize ();

			RaycastHit2D[] hits = 
				Physics2D.RaycastAll (transform.position, dir, dist, playerMask);
			foreach (RaycastHit2D hit in hits) {
				Transform hitTrans = hit.transform;

				ObjectIdentity oi = hitTrans.GetComponent<ObjectIdentity> ();
				if (oi && oi.objType == ObjectType.AI) {
					LineCut lc = hitTrans.GetComponent<LineCut> ();
					if(lc && lc.couldCut){
						// a darting AI is in between, set the status to none
						cs.controller = Controller.None;
					}
				}
			}

		}
	}
}
