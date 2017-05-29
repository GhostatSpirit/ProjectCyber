using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallSound : MonoBehaviour {

    HealthSystem hs;
    AudioSource audioS;

	// Use this for initialization
	void Start () {
        audioS = GetComponent<AudioSource>();
        hs = GetComponent<HealthSystem>();
        hs.OnObjectDead += PlayBreakSound; 
	}
	
    void PlayBreakSound(Transform transform)
    {
        audioS.PlayOneShot(audioS.clip);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
