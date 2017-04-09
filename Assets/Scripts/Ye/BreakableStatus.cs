using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStatus : MonoBehaviour {


	// Use this for initialization
	void Start () {
        HealthSystem HS = gameObject.GetComponent<HealthSystem>();
        
	}
	
	// Update is called once per frame
	void Update () {
        HealthSystem HS = gameObject.GetComponent<HealthSystem>();
        Animator BWAnim = gameObject.GetComponent<Animator>();
        BWAnim.SetFloat("LeftHealth", HS.objHealth / HS.maxHealth * 100);

        if ( HS.objHealth == 0f )
        {
            foreach (Transform child in transform )
            {
                child.gameObject.SetActive(false);
            }
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
