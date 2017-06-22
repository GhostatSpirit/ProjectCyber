using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


public class InScene : MonoBehaviour {

    public GameObject anim;
    AudioSource ad1;

	// Use this for initialization
	void Start () {
        ad1 = GetComponent<AudioSource>();
        anim.GetComponent<Animator>().speed = 0.25f;
        GetComponent<ProCamera2DTransitionsFX>().TransitionEnter();
        ad1.pitch = 0.7f;
        ad1.Play();
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    IEnumerator BrainAnim(GameObject anim)
    {
        
        yield return new WaitForSeconds(1f);
        anim.SetActive(true);
        anim.GetComponent<Animator>().speed = 0.1f;
        yield return null;
    }

}
