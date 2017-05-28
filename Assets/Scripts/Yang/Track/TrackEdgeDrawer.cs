using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrackEdgeDrawer : MonoBehaviour {

	LineRenderer lr;
	EdgeCollider2D ec;

	public string sortingLayerName = "ControlLine";
	public bool looping = true;

	Vector2[] edgePoints;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();
		if (lr) {
			lr.enabled = true;
			lr.useWorldSpace = true;
			lr.sortingLayerName = sortingLayerName;
			lr.sortingOrder = 0;
			lr.loop = this.looping;
		}
		ec = GetComponent<EdgeCollider2D> ();

		if (lr && ec) {
			edgePoints = ec.points;
			int edgePointCount = ec.pointCount;

			lr.positionCount = edgePointCount;
			lr.SetPositions (ConvertArray(edgePoints));
		}


	}


	Vector3[] ConvertArray(Vector2[] v2){
		Vector3[] v3 = new Vector3[v2.Length];
		for(int i = 0; i < v2.Length; ++i){
			Vector2 tempV2 = v2 [i];
			Vector3 tempVec = transform.TransformPoint (tempV2.x, tempV2.y, 0f);

			v3 [i] = tempVec;
			//v3 [i] = new Vector3 (tempV2.x, tempV2.y, 0f);
		}
		return v3;
	}
		

	void OnDrawGizmos() {
		if (edgePoints == null) {
			return;
		}
		Vector2 tempV2;
		Vector3 startPos, endPos;

		for (int i = 0; i < edgePoints.Length - 1; i++) {
			tempV2 = edgePoints [i];
			startPos = transform.TransformPoint (tempV2.x, tempV2.y, 0f);

			tempV2 = edgePoints [i + 1];
			endPos =  transform.TransformPoint (tempV2.x, tempV2.y, 0f);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(startPos, endPos);

		}

		// draw from the last point to the first point to complete the loop
		tempV2 = edgePoints [edgePoints.Length - 1];
		startPos = transform.TransformPoint (tempV2.x, tempV2.y, 0f);
		tempV2 = edgePoints [0];
		endPos =  transform.TransformPoint (tempV2.x, tempV2.y, 0f);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(startPos, endPos);

	}

//	void OnEnable(){
//		// try to get path nodes in the child objects
//		ec = GetComponent<EdgeCollider2D> ();
//		if(ec){
//			edgePoints = ec.points;
//		}
//	}
}
