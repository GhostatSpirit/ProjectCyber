using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTogether : MonoBehaviour {

	HealthSystem[] healths;

	bool killedAll = false;
	// Use this for initialization
	void Start () {
		healths = GetComponentsInChildren<HealthSystem> ();
		foreach(HealthSystem hs in healths){
			hs.OnObjectDead += KillAll;
		}
	}

	void KillAll(Transform objTrans){
		if(killedAll){
			return;
		}
		foreach(HealthSystem hs in healths){
			if(hs.transform != objTrans){
				hs.InstantDead ();
				hs.OnObjectDead -= KillAll;
			} else{
				hs.OnObjectDead -= KillAll;
			}
		}
		killedAll = true;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
