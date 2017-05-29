using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSound : MonoBehaviour {

    AudioSource audioS1; 

	// Use this for initialization
	void Start ()
    {
        audioS1 = GetComponents<AudioSource>()[1];
	}
	
    public void hackShoot()
    {
        audioS1.PlayOneShot(audioS1.clip);
    }
    // Update is called once per frame

    void Update () {
		
	}
}
