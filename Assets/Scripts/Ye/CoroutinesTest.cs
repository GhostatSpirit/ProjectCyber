using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinesTest: MonoBehaviour {

    public float waitTime = 2.0f;

	// Use this for initialization
	IEnumerator Start () {
        Debug.Log("start");
        yield return StartCoroutine(WaitAndPrint(waitTime));
        Debug.Log("done");
	}

    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("wait");
    }
	/*
	// Update is called once per frame
	void Update () {
		
	}
    */
}
