using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitParalyze : MonoBehaviour {
	public float paralyzeTime = 2f;

	ObjectType[] paralyzeTargets = 
	{ ObjectType.Virus };

	void OnCollisionEnter2D(Collision2D coll){
		Transform target = coll.collider.transform;
		ObjectIdentity oi = target.GetComponent<ObjectIdentity> ();
		if(oi && paralyzeTargets.Contains(oi.objType)){
			// paralyze the target
			switch(oi.objType){
			case ObjectType.Virus:
				{
					ControlStatus cs = target.GetComponent<ControlStatus> ();
					if(cs && cs.controller != Controller.None){
						VirusActions va = coll.transform.GetComponent<VirusActions> ();
						if(va){
							va.Paralyze (paralyzeTime);
						}
					}
					break;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
