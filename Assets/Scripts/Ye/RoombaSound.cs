using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaSound : MonoBehaviour {

    AudioSource audioS;

    // bool exploded = false;

	// Use this for initialization
	void Start () {
        audioS = GetComponent<AudioSource>();
        StartCoroutine(explosion(transform));	
	}
	
    // explosion sound
    IEnumerator explosion(Transform transform)
    {
        audioS.PlayOneShot(audioS.clip);
        yield return new WaitForSeconds(0f);
        //exploded = true;
    }

	// Update is called once per frame
	void Update ()
    {
        /*
        if ( exploded == false && )
        {
            StartCoroutine(explosion(transform));
        }
        */	
	}
}
