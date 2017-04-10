using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBag : MonoBehaviour {

    public GameObject player;
    HealthSystem HBHS;
    HealthSystem PlayerHS;
    public float HealRatio = 0.2f; 

	// Use this for initialization
	void Start () {
        HBHS = GetComponent<HealthSystem>();
        PlayerHS = player.GetComponent<HealthSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (HBHS.objHealth == 0)
        {
            PlayerHS.Heal( HealRatio * PlayerHS.maxHealth );
        }
	}
}
