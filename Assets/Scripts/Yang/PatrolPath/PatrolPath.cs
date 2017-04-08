/*
 * Author: Marcus Guimaraes @mzguimaraes
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PatrolPath : MonoBehaviour {

	public List<PatrolPathNode> nodePath;
	[ReadOnly]public List<Vector3> path;

	void OnDrawGizmos() {
		if (path == null) {
			return;
		}
		for (int i = 0; i < path.Count - 1; i++) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(nodePath[i].transform.position, nodePath[i+1].transform.position);

		}
	}

	void OnEnable(){
		// try to get path nodes in the child objects
		nodePath.Clear ();
		path.Clear ();
		foreach(Transform child in transform){
			PatrolPathNode pn = child.GetComponent<PatrolPathNode> ();
			if(pn){
				nodePath.Add (pn);
				path.Add (child.position);
			}
		}
	}

}
