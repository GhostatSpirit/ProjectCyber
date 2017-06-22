using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFLightStatusControl : MonoBehaviour {

	public Color bossColor;
	public Color hackerColor;

	ControlStatus cs;
	SFLight light;
	// Use this for initialization
	void Start () {
		cs = GetComponentInParent<ControlStatus> ();
		light = GetComponent<SFLight> ();

		if(cs){
			cs.OnCutByEnemy += TurnOffLight;
			cs.OnCutByPlayer += TurnOffLight;

			cs.OnLinkedByEnemy += SetBossColor;
			cs.OnLinkedByEnemy += TurnOnLight;

			cs.OnLinkedByPlayer += SetHackerColor;
			cs.OnLinkedByPlayer += TurnOnLight;
		}
	}

	public void SetBossColor(Transform trans){
		light.color = bossColor;
	}

	public void SetHackerColor(Transform trans){
		light.color = hackerColor;
	}

	public void TurnOnLight(Transform trans){
		light.enabled = true;
	}

	public void TurnOffLight(Transform trans){
		light.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
