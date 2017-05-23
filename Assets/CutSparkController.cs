using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSparkController : MonoBehaviour {
	public ParticleSystem hitSparkParticleSystem;
	private ParticleSystem.EmissionModule emission;

	public float emissionDuration = 0.5f;
	public float destoryDelay = 2f;

	// Use this for initialization
	IEnumerator Start () {
//		if(hitSparkParticleSystem){
//			emission = hitSparkParticleSystem.emission;
//			emission.enabled = false;
//		}
//
//		yield return new WaitForSeconds (destoryDelay);

		if(hitSparkParticleSystem){
			emission = hitSparkParticleSystem.emission;
			emission.enabled = true;
		}

		yield return new WaitForSeconds (emissionDuration);

		if(hitSparkParticleSystem)
			emission.enabled = false;

		yield return new WaitForSeconds (destoryDelay);

		Destroy (this.gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
