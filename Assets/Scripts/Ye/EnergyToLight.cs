using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyToLight : MonoBehaviour {


    SFLight sflight;
    PlayerEnergy pe;
    public float minIntensity;
	public float maxIntensity;

	// Use this for initialization
	void Start () {
        sflight = GetComponent<SFLight>();
        pe = GetComponentInParent<PlayerEnergy>();

    }
	
	// Update is called once per frame
	void Update () {
		sflight.intensity = 
		(float)(minIntensity + ( maxIntensity - minIntensity ) * ( pe.GetEnergy() / pe.maxEnergy ));
	}
}
