using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartSound : MonoBehaviour {

    AudioSource audioS0;
    AudioSource audioS1;
    DartSkill ds;
    LineCut lc;

	// Use this for initialization
    // the cut sound must be second audioSource
	void Start () {

        audioS0 = GetComponents<AudioSource>()[0];
        audioS1 = GetComponents<AudioSource>()[1];

        lc = GetComponent<LineCut>();
        ds = GetComponent<DartSkill>();

        lc.OnLineCut += Cut ;

    }

    public void StartDart()
    {
        audioS0.PlayOneShot(audioS0.clip);
    }

    public void Cut(Transform trans)
    {
        audioS1.PlayOneShot(audioS1.clip);
    }


    // Update is called once per frame

	void Update () {
		
	}
}
