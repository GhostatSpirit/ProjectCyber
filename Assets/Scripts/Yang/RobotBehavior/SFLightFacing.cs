using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFLightFacing : MonoBehaviour {
	RectTransform lightRect;
	FieldOfView fov;

	float initHeight;
	float initRadius;
	public bool angledLight = false;

	public bool useTransformRotation = false;

	[Range(0f, 1f)]
	public float radiusFactor = 0.95f;
	// Use this for initialization
	void Start () {
		lightRect = GetComponent<RectTransform> ();
		fov = GetComponentInParent<FieldOfView> ();

		initHeight = lightRect.sizeDelta.y;
		initRadius = fov.radius;
	}

	// Update is called once per frame
	void Update () {
		Vector3 facing = fov.facing;

		if (useTransformRotation == false) {
			lightRect.up = facing;
		}

		if (angledLight) {
			// change the height of the rect depending on the angle
			float angleDeg = 0f;
			if (useTransformRotation == false) {
				angleDeg = lightRect.rotation.eulerAngles.z;
			} else{
				angleDeg = transform.rotation.eulerAngles.z;
			}
			float angleRad = Mathf.Deg2Rad * angleDeg;

			float factor = Mathf.Sqrt (4.0f / (1 + 3 * Mathf.Cos (angleRad) * Mathf.Cos (angleRad)));
			float newHeight = initHeight * factor;
			lightRect.sizeDelta = new Vector2 (lightRect.sizeDelta.x, newHeight);

			float newRadius = initRadius * factor * radiusFactor;
			fov.radius = newRadius;
		}
	}
}
