using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLaserUpdate : MonoBehaviour {
	LineRenderer lr;
	[HideInInspector] public Vector3 targetPos;
	public float easing = 30f;

	public string sortingLayerName = "ControlLine";
	// Use this for initialization
	void Start () {
		lr = GetComponentInChildren<LineRenderer> ();
		lr.sortingLayerName = this.sortingLayerName;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, targetPos, easing * Time.deltaTime);
		if(GetComponent<Renderer>()){
			// draw a aim laser between transform.pos and parent.transform.pos
			lr.useWorldSpace = true;
			Vector3[] positions = new Vector3[2];
			positions [0] = transform.position;
			positions [1] = transform.parent.position;
			lr.SetPositions (positions);

//			Renderer r = lr.transform.GetComponent<Renderer> ();
//			if(r){
//				r.sortingLayerName = this.sortingLayerName;
//				r.sortingOrder = 0;
//			}
		}
	}

	public void SnapPosition(){
		transform.position = targetPos;
	}
}
