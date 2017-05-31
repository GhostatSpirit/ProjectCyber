using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCutSpark : MonoBehaviour {
	public GameObject lineCutSpark;
	public float scale = 1f;

	LineCut lineCut;
	// Use this for initialization
	void Start () {
		lineCut = GetComponent<LineCut> ();
		lineCut.OnLineCut += CreateSpark;
	}

	void CreateSpark(Transform transform){
		if(lineCutSpark){
			GameObject sparkGO = Instantiate (lineCutSpark, transform.position, transform.rotation);
			sparkGO.transform.localScale = new Vector3 (scale, scale, scale);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
