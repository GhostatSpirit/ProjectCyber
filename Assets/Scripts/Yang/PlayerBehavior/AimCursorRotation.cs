using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursorRotation : MonoBehaviour {

	public Transform shootDummy;

	Quaternion viewAngleRot;

	// Use this for initialization
	void Start () {
		viewAngleRot = Quaternion.AngleAxis (30f, Vector3.right);
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion shootRot = shootDummy.rotation;
		Quaternion finalRot =  viewAngleRot * shootRot;
		transform.rotation = finalRot;
	}
}
