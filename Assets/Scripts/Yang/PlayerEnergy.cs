using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour {
	public float initialEnergy = 0f;
	public float maxEnergy = 100f;
	public Text displayText;

	float playerEnergy;
	// Use this for initialization
	void Start () {
		playerEnergy = initialEnergy;
	}
	
	// Update is called once per frame
	void Update () {
		if(displayText != null){
			displayText.text = Mathf.Round(playerEnergy).ToString();
		}
	}

	// subtract the used energy and return the remaining energy
	public float UseEnergy(float deltaEnergy){
		playerEnergy -= deltaEnergy;
		if(deltaEnergy > 0f){
			deltaEnergy = 0f;
		}
		return playerEnergy;
	}

	// add the additional energy and return the remaining energy
	public float AddEnergy(float deltaEnergy){
		playerEnergy += deltaEnergy;
		if(playerEnergy > maxEnergy){
			playerEnergy = maxEnergy;
		}
		return playerEnergy;
	}

	public float GetEnergy(float deltaEnergy){
		return playerEnergy;
	}

	public bool IsDepleted(){
		return (playerEnergy == 0f);
	}
}
