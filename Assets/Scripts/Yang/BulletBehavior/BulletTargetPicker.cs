using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ChaseTarget))]
public class BulletTargetPicker : MonoBehaviour {

	// add the objects you would like bullet to chase here
	// that are not controllables
	public List<ObjectType> targets = new List<ObjectType>{};

	public bool addControllables = true;

	[Range(0f, 1f)]
	public float factorInterval = 0.25f;

	ChaseTarget ct;
	FieldOfView fov;

	// Use this for initialization
	void Start () {
		if (addControllables) {
			targets.AddRange (ObjectIdentity.controllables);
		}

		ct = GetComponent<ChaseTarget> ();
		fov = GetComponent<FieldOfView> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!ct || !fov){
			return;
		} else {
			Transform target = null;
			if(factorInterval == 0f || factorInterval == 1f){
				target = fov.ScanTargetInSight (targets);
			} else {
				for(float factor = factorInterval; factor <= 1f; factor += factorInterval){
					target = fov.ScanTargetInSight (targets, factorInterval);
					if(target != null){
						break;
					}
				}
			}
			ct.target = target;
		}
	}
}
