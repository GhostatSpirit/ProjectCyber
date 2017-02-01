using UnityEngine;
using System.Collections;

// Attach on an FOWSystem Object
// All FOWGrids will be instatiated as it children
public class FOWSystemControl : MonoBehaviour {
	public GameObject fowGrid;
	public float gridSize = 0.5f;

	// Use this for initialization
	void Start () {
		// get the properties from the SpriteRenderer bound
		Bounds bounds = GetComponent<SpriteRenderer> ().bounds;
		// Disable the SpriteRenderer
		GetComponent<SpriteRenderer> ().enabled = false;

		Vector3 bottomleft = bounds.center - bounds.extents;
		Vector3 topright = bounds.center + bounds.extents;
		Vector3 bottomright = new Vector3 (topright.x, bottomleft.y, bottomleft.z);
		Vector3 topleft = new Vector3 (bottomleft.x, topright.y, bottomleft.z);


		// create the child grid objects
		for(float xCursor = topleft.x; xCursor < bottomright.x; xCursor += gridSize){
			for(float yCursor = topleft.y; yCursor > bottomright.y; yCursor -= gridSize){
				// instatiate a single grid and set this transform as its parent
				Vector3 newGridPos = new Vector3 (xCursor, yCursor, topleft.z);
				Quaternion newGridRot = transform.rotation;
				GameObject newGrid = (GameObject)Instantiate (fowGrid, newGridPos, newGridRot);
				newGrid.transform.localScale = new Vector3 (gridSize, gridSize, gridSize);
				newGrid.transform.parent = transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
