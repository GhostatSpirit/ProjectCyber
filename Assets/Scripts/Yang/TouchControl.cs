using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {
	Ray ray;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// converting from screen point to ray
		if (Input.touchCount > 0) {
			Vector2 vec = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			Debug.DrawRay (ray.origin, ray.direction);
			RaycastHit2D hit = Physics2D.Raycast (vec, (Input.GetTouch (0).position));
			if(hit.collider && Input.GetTouch(0).phase == TouchPhase.Began){
				// do some raycasting
				Debug.Log ("Hit something");
				// Draw Debug Ray
				Debug.DrawRay (vec, (Input.GetTouch (0).position) - vec);
				// Destory the object being hit
				Destroy (hit.transform.gameObject);
			}
		}


	}
}
