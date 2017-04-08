using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class TestPortal : MonoBehaviour {

    public Camera cam;
    public GameObject AI;
    public GameObject Hacker;
    public float MaxtransTime;
    public GameObject destination;

    public float transTime = 0f;

	// Use this for initialization
	void Start () {
        ProCamera2DTransitionsFX transFX = cam.GetComponent<ProCamera2DTransitionsFX>();
        Collider2D portalcol = gameObject.GetComponent<CircleCollider2D>();
        Collider2D AIcol = AI.GetComponent<CircleCollider2D>();
        Collider2D Hackercol = Hacker.GetComponent<CircleCollider2D>();  
	}

    // Update is called once per frame

    IEnumerator DelayedTransition(ProCamera2DTransitionsFX transFX)
    {
        transFX.TransitionExit();
        yield return new WaitForSeconds(0.5f);
        AI.transform.position = destination.transform.position;
        Hacker.transform.position = destination.transform.position;
        yield return new WaitForSeconds(1f);
        transFX.TransitionEnter();
    }
    

	void Update () {
        ProCamera2DTransitionsFX transFX = cam.GetComponent<ProCamera2DTransitionsFX>();
        Collider2D portalcol = gameObject.GetComponent<CircleCollider2D>();
        Collider2D AIcol = AI.GetComponent<CircleCollider2D>();
        Collider2D Hackercol = Hacker.GetComponent<CircleCollider2D>();
        
        //  Examine whether ai and hacker are in the portal 
        if (portalcol.IsTouching(AIcol) && portalcol.IsTouching(Hackercol))
        {
            transTime += Time.deltaTime;
        }

        //  If not set transTime to 0
        else
        {
            transTime = 0f;
        }

        //  If transTime >= MaxtransTime, transport
        if (transTime >= MaxtransTime)
        {
            transTime = 0f;
            StartCoroutine(DelayedTransition(transFX));
        }

    }
}
