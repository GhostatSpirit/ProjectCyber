using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLayerSetter : MonoBehaviour {
	public int orderOffset = 100;

	public bool SetSortingLayer(string layerName){
		if(SortingLayer.NameToID(layerName) == 0){
			// layerName invalid
			return false;
		}
		ParticleSystemRenderer[] particles;
		particles = GetComponentsInChildren<ParticleSystemRenderer> ();
		foreach(ParticleSystemRenderer particle in particles){
			particle.sortingLayerName = layerName;
			particle.sortingOrder += orderOffset;
		}

		return true;
	}

	public bool SetSortingLayer(int layerId){
		if(SortingLayer.IsValid(layerId) == false){
			// layerName invalid
			return false;
		}
		ParticleSystemRenderer[] particles;
		particles = GetComponentsInChildren<ParticleSystemRenderer> ();
		foreach(ParticleSystemRenderer particle in particles){
			particle.sortingLayerID = layerId;
			particle.sortingOrder += orderOffset;
		}

		return true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
