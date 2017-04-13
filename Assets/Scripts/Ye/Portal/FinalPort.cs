using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.SceneManagement;

public class FinalPort : MonoBehaviour {

    public Camera cam;
    public GameObject AI;
    public GameObject Hacker;
    public float MaxtransTime = 5f;
    //public GameObject destination;

    public GameObject PortalBack;
    public GameObject PortalFore;

    public float transTime = 0f;
    bool end = false;

    // Transform[] DesList;

    ProCamera2DTransitionsFX transFX;
    Collider2D portalcol;
    Collider2D AIcol;
    Collider2D Hackercol;
    AudioSource TransAudio;

    // Use this for initialization
    void Start()
    {

        TransAudio = GetComponent<AudioSource>();

    }

    // Update is called once per frame

    IEnumerator DelayedTransition(ProCamera2DTransitionsFX transFX)
    {
        AudioSource audio = GetComponent<AudioSource>();
        // AI and Hacker can't move when transition start
        AI.GetComponent<PlayerControl>().canControl = false;
        Hacker.GetComponent<PlayerControl>().canControl = false;

        // start TransAnim
        PortalBack.GetComponent<Animator>().SetBool("PortalBackTrans", true);
        PortalFore.GetComponent<Animator>().SetBool("PortalForeTrans", true);


        // wait for start
        yield return new WaitForSeconds(1f);
        audio.Play();
        // camera exit
        // Debug.Log("in");
        transFX.TransitionExit();

        // wait for transition
        yield return new WaitForSeconds(0.5f);

        // reset TraansAnim
        PortalBack.GetComponent<Animator>().SetBool("PortalBackTrans", false);
        PortalFore.GetComponent<Animator>().SetBool("PortalForeTrans", false);

        SceneManager.LoadScene(2);
        // transition
        /*
        AI.transform.position = DesList[1].position;
        Hacker.transform.position = DesList[2].position;


        // wait for transition
        yield return new WaitForSeconds(0.5f);

        // camera enter
        transFX.TransitionEnter();

        // AI and Hacker can move when transition end
        AI.GetComponent<PlayerControl>().canControl = true;
        Hacker.GetComponent<PlayerControl>().canControl = true;

        end = false;
        */
    }


    void Update()
    {
        TransAudio = GetComponent<AudioSource>();
        transFX = cam.GetComponent<ProCamera2DTransitionsFX>();
        portalcol = gameObject.GetComponent<Collider2D>();
        AIcol = AI.GetComponent<Collider2D>();
        Hackercol = Hacker.GetComponent<Collider2D>();
        // DesList = destination.GetComponentsInChildren<Transform>();

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

        //  If transTime >= MaxtransTime ,stop transtime and transport
        if (transTime >= MaxtransTime && end == false)
        {

            transTime = 0f;
            end = true;
            StartCoroutine(DelayedTransition(transFX));
        }

    }
}
