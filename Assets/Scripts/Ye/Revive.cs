using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class Revive : MonoBehaviour {

    public GameObject hacker;
    public GameObject AI;
    public Camera Cam;
    public GameObject revivePoint;
    Transform[] DesList;

    ProCamera2DTransitionsFX reviveFx;

    HealthSystem hackerHS;
    HealthSystem AIHS;
    PlayerControl hackerPC;
    PlayerControl AIPC;
    
//    bool able = true;

	// Use this for initialization
	void Start () {
        // get initial
        reviveFx = Cam.GetComponent<ProCamera2DTransitionsFX>();
        hackerHS = hacker.GetComponent<HealthSystem>();
        AIHS = AI.GetComponent<HealthSystem>();
        hackerPC = hacker.GetComponent<PlayerControl>();
        AIPC = AI.GetComponent<PlayerControl>();
        DesList = revivePoint.GetComponentsInChildren<Transform>();
    }

    IEnumerator DelayedTransition(ProCamera2DTransitionsFX reviveFX)
    {

        // AI and Hacker can't move when transition start
        AIPC.canControl = false;
        hackerPC.canControl = false;


        // wait for start
        yield return new WaitForSeconds(1f);

        // camera exit
        reviveFX.TransitionExit();

        // wait for transition
        yield return new WaitForSeconds(0.5f);


        // transition

        AI.transform.position = DesList[1].position;
        hacker.transform.position = DesList[2].position;

        // wait for transition
        yield return new WaitForSeconds(0.5f);

        // camera enter
        reviveFX.TransitionEnter();

        // AI and Hacker can move when transition end
        AIPC.canControl = true;
        hackerPC.canControl = true;
        AIHS.Revive(1);
        hackerHS.Revive(1);
//        able = false;

		reviveCoroutine = null;
    }

    
	Coroutine reviveCoroutine;
    // Update is called once per frame
    
    void Update () {
        
        if ( hackerHS.IsDead() && AIHS.IsDead() )
        {
			if (reviveCoroutine == null) {
				reviveCoroutine = StartCoroutine (DelayedTransition (reviveFx));
			}
        }	
	}
    
}
