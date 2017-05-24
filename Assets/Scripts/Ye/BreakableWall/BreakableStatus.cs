using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStatus : MonoBehaviour {

    HealthSystem HS;

	float delay = 1f;
	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
        HS = gameObject.GetComponent<HealthSystem>();
        Animator BWAnim = gameObject.GetComponent<Animator>();
        BWAnim.SetFloat("LeftHealth", HS.objHealth / HS.maxHealth * 100);

        if ( HS.objHealth == 0f )
        {
            foreach (Transform child in transform )
            {
                child.gameObject.SetActive(false);
            }
            gameObject.GetComponent<Collider2D>().enabled = false;
			Invoke ("TurnOffGameObject", delay);
        }
    }

	void TurnOffGameObject(){
		this.gameObject.SetActive (false);
	}
}
