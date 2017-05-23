using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	bool _canControl = true;

	public bool canControl{
		get{
			return _canControl;
		}
		set{
			if(_canControl != value){
				if (value == true) {
					StartControls ();
				} else {
					StopControls ();
				}
				_canControl = value;
			}

		}
	}

//	PlayerMovement _pm;
//	PlayerMovement pm{
//		get{
//			if (!_pm) {
//				_pm = GetComponentInChildren<PlayerMovement> ();
//			}
//			return _pm;
//		}
//	}
//	DartSkill _ds;
//	DartSkill ds{
//		get{
//			if (!_ds) {
//				_ds = GetComponentInChildren<DartSkill> ();
//			}
//			return _ds;
//		}
//	}
//	ChargingDart _cd;
//	ChargingDart cd{
//		get{
//			if (!_cd) {
//				_cd = GetComponentInChildren<ChargingDart> ();
//			}
//			return _pm;
//		}
//	}
//	PlayerAim _pa;
//	PlayerMovement pm{
//		get{
//			if (!pm) {
//				_pm = GetComponentInChildren<PlayerMovement> ();
//			}
//			return _pm;
//		}
//	}
//	PlayerShoot _ps;
//	PlayerMovement pm{
//		get{
//			if (!pm) {
//				_pm = GetComponentInChildren<PlayerMovement> ();
//			}
//			return _pm;
//		}
//	}
//
	public List<MonoBehaviour> behaviours;

	// Use this for initialization
	void Start () {
		if(behaviours == null){
			behaviours = new List<MonoBehaviour> ();
		}
//		pm = GetComponentInChildren<PlayerMovement> ();
//		if (pm)  behaviours.Add (pm);
//		ds = GetComponentInChildren<DartSkill> ();
//		if (ds)  behaviours.Add (ds);
//		cd = GetComponentInChildren<ChargingDart> ();
//		if (cd)  behaviours.Add (cd);
//		pa = GetComponentInChildren<PlayerAim> ();
//		if (pa)  behaviours.Add (pa);
//		ps = GetComponentInChildren<PlayerShoot> ();
//		if (ps)  behaviours.Add (ps);


	}

	void StartControls(){
		// Debug.Log(
		foreach(MonoBehaviour behaviour in behaviours){
			if (behaviour != null)
				Debug.Log ("started: " + behaviour.ToString ());
				behaviour.enabled = true;
		}
	}

	void StopControls(){
		foreach(MonoBehaviour behaviour in behaviours){
			if (behaviour != null) {
				Debug.Log ("stopped: " + behaviour.ToString ());
				behaviour.enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
