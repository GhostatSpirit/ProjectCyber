using UnityEngine;
using System.Collections;

public class LazerControl : MonoBehaviour {
	public float rotateSpeed = 30;
	public float maxAngle = 50;

	//float lastAngle = 0;
	bool turningLeft = true;

	public GameObject laserBeamObject;
	public float laserLength = 10;

	public bool isRotating = true;

	float initialRotZ;
	// Use this for initialization
	void Start () {
		if(laserBeamObject == null){
			laserBeamObject = transform.GetChild (0).gameObject;
		}
		initialRotZ = transform.rotation.eulerAngles.z;
	}

	// Update is called once per frame
	void Update () {

		// turn the laser generator
		if (turningLeft && isRotating) {
			transform.Rotate (0f, 0f, Time.deltaTime * rotateSpeed);
			float currentAngle = transform.rotation.eulerAngles.z;
			if(InRange(LocalAngle(currentAngle, initialRotZ), 0f, 180f) && LocalAngle(currentAngle, initialRotZ) > maxAngle){
				turningLeft = false;
			}
		}else if(isRotating){
			transform.Rotate (0f, 0f, - Time.deltaTime * rotateSpeed);
			float currentAngle = transform.rotation.eulerAngles.z;
			if(InRange(LocalAngle(currentAngle, initialRotZ), 180f, 360f) && LocalAngle(currentAngle, initialRotZ) < 360f - maxAngle){
				turningLeft = true;
			}
		}

		// actually shoot raycast now, only detecting things on the player layer
		SpriteRenderer beamRenderer = laserBeamObject.GetComponent<SpriteRenderer> ();
		float beamRawHeight = beamRenderer.sprite.rect.height / beamRenderer.sprite.pixelsPerUnit;
		float beamScaledHeight = beamRawHeight * laserBeamObject.transform.localScale.y;
		float raycastLength = laserLength * beamScaledHeight;

		RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.up,
			raycastLength);

		// visualize the laser ray
		Debug.DrawRay (transform.position, transform.up.normalized * raycastLength);
		//Debug.Log (raycastLength);

		// let's check the results of the raycast
		if (raycastHit.collider != null) {
			// destroy the object taged "Player" it hits
			if (raycastHit.collider.gameObject.tag == "Player") {
				raycastHit.collider.gameObject.SetActive (false); 
			}
		}


	}

	bool InRange(float candidate_angle, float min_angle, float max_angle){
		return (candidate_angle < max_angle) && (candidate_angle >= min_angle);
	}

	// takes a world angle representation (euler angle z) and init angle
	// returs a local euler angle z (init angle as zero), 0 - 360
	float LocalAngle(float worldAngle, float initAngle){
		float delta = worldAngle - initAngle;
		if(delta < 0f){
			delta += 360f;
		}
		return delta;
	}
}


