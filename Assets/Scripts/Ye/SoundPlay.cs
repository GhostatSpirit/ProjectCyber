using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour {

    AudioSource audioSource;
    
    // Outer parameter bool a
    public bool soundPlay = false;


	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	

    // Play audio
    IEnumerator Sound(AudioSource audio)
    {
        audio.Play();
        yield return new WaitForSeconds(0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Play audioSource 
        if (soundPlay == true)
        { 
            StartCoroutine(Sound(audioSource));
        }
    }
}
