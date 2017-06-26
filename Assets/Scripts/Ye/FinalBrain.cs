using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.SceneManagement;
using DG.Tweening;

[HideInInspector]public enum BrainStatus { active, negative };

public class FinalBrain : MonoBehaviour {

    public Camera cam;
    public GameObject AI;
    public GameObject Hacker;
    public int targetSceneNum = 2;

	bool started = false;

	public AudioSource bgmSource;

    // Use this for initialization
    
    [HideInInspector]public BrainStatus BS = BrainStatus.negative;


    ProCamera2DTransitionsFX transFX;

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        transFX = cam.GetComponent<ProCamera2DTransitionsFX>();
		if ( started == false && BS == BrainStatus.active )
        {
//            SceneManager.LoadScene("End");
			StartCoroutine (BrainAction (transFX));
            started = true;
            this.enabled = false;
        }
	}

    IEnumerator BrainAction(ProCamera2DTransitionsFX transFX)
    {
        // AI and Hacker can't move when transition start
        AI.GetComponent<PlayerControl>().canControl = false;
        Hacker.GetComponent<PlayerControl>().canControl = false;

        // wait for start
        yield return new WaitForSeconds(0f);

        // camera exit
        transFX.TransitionExit();

//		if(bgmSource){
//			bgmSource.DOFade (0f, 1f);
//		}

        // wait for transition
        yield return new WaitForSeconds(1f);
        // load BrainScene
        SceneManager.LoadScene(targetSceneNum);
    }

}
