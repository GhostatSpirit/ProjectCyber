using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShootSound : MonoBehaviour {

    AudioSource audioS0;

	// Use this for initialization
	void Start () {
        audioS0 = GetComponents<AudioSource>()[0];
	}
	
    public void RobotShoot()
    {
        audioS0.PlayOneShot(audioS0.clip);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
