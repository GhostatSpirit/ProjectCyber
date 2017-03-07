using UnityEngine;
using System.Collections;

public class FOWGridControl : MonoBehaviour {
	SpriteRenderer gridRenderer;
	public float fadeSpeed = 5f;

	bool startFading = false;

	void Start(){
		gridRenderer = GetComponent<SpriteRenderer> ();
	}

	void Update(){
		if(startFading){
			Color color = gridRenderer.material.color;
			color.a -= fadeSpeed / 10f * Time.deltaTime;
			if(color.a <= 0f){
				color.a = 0f;
				Destroy (transform.gameObject);
			}
			gridRenderer.material.color = color;
		}
	}


	// Replaced for performance concerns
//	void OnTriggerStay2D( Collider2D activator ) {
//		if ( activator.gameObject.tag == "PlayerSight" ) {
//			startFading = true;
//			Color color = gridRenderer.material.color;
//			color.a -= fadeSpeed / 10f * Time.deltaTime;
//			if(color.a <= 0f){
//				color.a = 0f;
//				Destroy (transform.gameObject);
//			}
//			gridRenderer.material.color = color;
//		}
//	}

	// a function that is automatically called when
	// something with a Rigidbody2D enters this trigger
	void OnTriggerEnter2D( Collider2D activator ) {
		// Is player activating the trigger?
		if ( activator.gameObject.tag == "PlayerSight" ) {
			startFading = true;
		}
	}
}
