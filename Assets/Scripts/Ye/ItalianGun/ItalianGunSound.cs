using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItalianGunSound : MonoBehaviour {


    AudioSource audioS;
    LaserCannonState lcS;

    float lifetime;

	// Use this for initialization
	void Start () {
        audioS = GetComponent<AudioSource>();
   
	}
	
    IEnumerator ItalianGunShoot( float lifetime)
    {
        audioS.PlayOneShot(audioS.clip);
        
        yield return new WaitForSeconds( lifetime );

        audioS.Stop();
  
    }

    public void ShootSound( float  lifetime)
    {
        StartCoroutine(ItalianGunShoot(lifetime));
    }


	// Update is called once per frame
	void Update ()
    {
	}
}
