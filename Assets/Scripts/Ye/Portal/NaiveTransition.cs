using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NaiveTransition : MonoBehaviour {

    public Camera cam;
    // public GameObject target;
    public bool trans; 

    // save back place 
    // public GameObject initial;

    ProCamera2DTransitionsFX s;

    // Use this for initialization
    void Start () {
        s = cam.GetComponent<ProCamera2DTransitionsFX>();
    }

    IEnumerator DelayedOperations()
    {
        s = cam.GetComponent<ProCamera2DTransitionsFX>();

        if (trans == true)
        {
            
            // start transition

            s.TransitionExit();
            
            yield return new WaitForSeconds(0.5f);

            SceneManager.LoadScene(2);
            /*
            gameObject.transform.position = target.transform.position;
            yield return new WaitForSeconds(1f);
            s.TransitionEnter();
            */
        }
        
        // go back
        /*
        if (Input.GetKey(KeyCode.Z))
        {
            gameObject.transform.position = initial.transform.position;
        }
        */
        
    }
    void Update()
    {
        StartCoroutine(DelayedOperations());
    }

}