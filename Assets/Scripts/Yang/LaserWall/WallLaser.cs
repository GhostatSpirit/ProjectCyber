using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.FastLineRenderer;

[System.Serializable]
public struct LaserLine{
	public Transform leftEnd;
	public Transform rightEnd;
}

//[System.Serializable]
//public struct LaserLineProperty{
//	public static LaserLineProperty Default(){
//		LaserLineProperty p = new LaserLineProperty ();
//		p.lineRadius = 2f;
//		p.glowWidthMultiplier = 1f;
//		p.glowIntensity = 0.18f;
//
//		return p;
//	}
//
//	public float lineRadius;
//	public float glowWidthMultiplier;
//	public float glowIntensity;
//}

public class WallLaser : MonoBehaviour {
//	public Transform interfaceTransform;

	public List<LaserLine> laserLines;


	FastLineRenderer lr;
//	FastLineRenderer lr{
//		get{
//			if (!_lr)
//				_lr = GetComponent<FastLineRenderer> ();
//			return _lr;
//		}
//	}

	EdgeCollider2D ec;
//	ControlStatus cs;

	public float lineRadius = 2f;
	public float glowWidthMultiplier = 1f;
	public float glowIntensity = 0.18f;

	public float jitterMultiplier = 5f;

	public float fadeDelay = 1f;

	static readonly float radiusFactor = 0.01f;
	//static readonly float jitterRadiusFactor = 0.25f;

	// Use this for initialization
	void Start () {
		if(laserLines == null){
			laserLines = new List<LaserLine> ();
		}
		lr = GetComponent<FastLineRenderer> ();

		ec = transform.gameObject.AddComponent<EdgeCollider2D> ();
		ec.enabled = false;

		//yield return new WaitForSeconds (0.1f);

		DrawDefaultLine (transform);
		InitEdgeCollider ();

//		if(interfaceTransform){
//			cs = interfaceTransform.GetComponent<ControlStatus> ();
//		}

	}
		

	public void DrawDefaultLine(Transform objTrans){
//		Debug.Log ("WallLaser: Default");
		StartCoroutine (DefaultLineIE());
	}

	IEnumerator DefaultLineIE(){
//		Debug.Log ("before Fixed");
		yield return new WaitForEndOfFrame ();
//		Debug.Log ("after Fixed");
		lr.Reset ();
		lr.JitterMultiplier = 0f;

		foreach (LaserLine line in laserLines) {

			if (line.leftEnd && line.rightEnd) {
				// create a new line in renderer
				//				Debug.Log ("drawing line");
				FastLineRendererProperties props = new FastLineRendererProperties ();
				props.Start = line.leftEnd.position;
				props.End = line.rightEnd.position;

				props.Radius = lineRadius * radiusFactor;
				props.GlowIntensityMultiplier = glowIntensity;
				props.GlowWidthMultiplier = glowWidthMultiplier;

				lr.AddLine (props, true, true);
			}
		}
		lr.Apply ();
	}

	public void DrawJitterLine(Transform objTrans){
//		Debug.Log ("WallLaser: Jitter");
		StartCoroutine (JitterLineIE());
	}

	IEnumerator JitterLineIE(){
		yield return new WaitForEndOfFrame ();

		lr.Reset ();

		//Debug.Log (jitterMultiplier);
		lr.JitterMultiplier = jitterMultiplier;
		foreach (LaserLine line in laserLines) {
			if (line.leftEnd && line.rightEnd) {
				// create a new line in renderer
				FastLineRendererProperties props = new FastLineRendererProperties ();
				props.Start = line.leftEnd.position;
				props.End = line.rightEnd.position;

				props.Radius = lineRadius * radiusFactor / jitterMultiplier;
				props.GlowIntensityMultiplier = glowIntensity;
				props.GlowWidthMultiplier = glowWidthMultiplier;

				lr.AddLine (props, true, true);
			}
		}

		lr.Apply ();
	}

	public void ClearLine(Transform objTrans){
//		Debug.Log ("WallLaser: Clear");
		StartCoroutine (ClearLineIE());
	}

	IEnumerator ClearLineIE(){
		yield return new WaitForEndOfFrame ();
		lr.Reset ();
	}

	void InitEdgeCollider(){
		if (laserLines.Count == 0)
			return;

		ec.isTrigger = false;
		// use the last laser line as collider
		Vector3 leftPos = laserLines [laserLines.Count - 1].leftEnd.position;
		Vector3 rightPos = laserLines [laserLines.Count - 1].rightEnd.position;
		Vector2[] temparray = new Vector2[2];
		temparray [0] = transform.InverseTransformPoint (leftPos);
		temparray [1] = transform.InverseTransformPoint (rightPos);
		ec.points = temparray;

		ec.enabled = true;
	}

	public void EnableCollider(Transform objTrans){
//		Debug.Log ("WallLaser: Enable");
		ec.enabled = true;
	}

	public void DisableCollider(Transform objTrans){
//		Debug.Log ("WallLaser: Disable");
		ec.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			DrawJitterLine (transform);
		}
	}


}
