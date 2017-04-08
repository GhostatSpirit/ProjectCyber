using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class NaiveTransition : MonoBehaviour {

    public Camera cam;
    public GameObject target;
    public GameObject initial;

    // Use this for initialization
    void Start () {
        ProCamera2DTransitionsFX s = cam.GetComponent<ProCamera2DTransitionsFX>();
        //StartCoroutine(DelayedOperations());
        //print("start coroutine");
    }

    IEnumerator DelayedOperations()
    {
        ProCamera2DTransitionsFX s = cam.GetComponent<ProCamera2DTransitionsFX>();
        if (Input.GetKey(KeyCode.Space))
        {
            //s.TransitionExit();
            s.TransitionExit();
            yield return new WaitForSeconds(1.5f);
            gameObject.transform.position = target.transform.position;
            yield return new WaitForSeconds(1f);
            s.TransitionEnter();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            gameObject.transform.position = initial.transform.position;
        }
        
    }
    void Update()
    {
        StartCoroutine(DelayedOperations());
    }
    // Update is called once per frame
    /*
	void Update () {
         ProCamera2DTransitionsFX s = cam.GetComponent<ProCamera2DTransitionsFX>();
        if (Input.GetKey(KeyCode.Space))
        {
            //s.TransitionExit();
            s.TransitionExit();
            
            gameObject.transform.position = target.transform.position;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            gameObject.transform.position = initial.transform.position;
        }

    }
    */


}