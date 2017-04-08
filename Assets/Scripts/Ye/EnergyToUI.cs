using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class EnergyToUI : MonoBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerEnergy PE = Player.GetComponent<PlayerEnergy>();
        float energy = PE.GetEnergy();
        float ratio = energy / PE.maxEnergy;
        // gameObject.GetComponent<Image>;       	
	}
}
