using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : MonoBehaviour {

    DoorMover dm;
    AudioSource audioS;
    bool alreadyOpen = false;

	// Use this for initialization
	void Start () {
        dm = GetComponent<DoorMover>();
        audioS = GetComponent<AudioSource>();
	}
	
    IEnumerator DoorOpen(Transform transform)
    {
        // play door open sound
        yield return new WaitForSeconds(0f);
        audioS.PlayOneShot(audioS.clip);
        alreadyOpen = true;
    }

	// Update is called once per frame
	void Update () {

        if ( alreadyOpen == false && dm.doorStatus == DoorMover.DoorStatus.Opening)
        {
            StartCoroutine(DoorOpen(transform));
        }
	}
}
