using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log (Input.touchCount);

		if(Input.touchCount > 0){
			// test touch controls
//			Touch myFirstTouch = Input.GetTouch (0);
//			Vector2 touchPos = myFirstTouch.position;
//			Debug.Log (touchPos);

//			foreach(Touch touch in Input.touches){
//				Debug.Log (touch.position);
//			}

			if(Input.GetTouch(0).phase == TouchPhase.Began){
				Debug.Log ("Touch Began");
			}
			if(Input.GetTouch(0).phase == TouchPhase.Moved){
				Debug.Log ("Touch Moved");
			}
			if(Input.GetTouch(0).phase == TouchPhase.Ended){
				Debug.Log ("Touch Ended");
			}
		}
	}
}
