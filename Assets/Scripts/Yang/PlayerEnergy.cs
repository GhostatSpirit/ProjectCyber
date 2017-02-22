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

	// subtract the used energy if we have remaining energy
	// if failed, return false and keep the energy unchanged
	public bool UseEnergy(float deltaEnergy){
		if(playerEnergy >= deltaEnergy){
			playerEnergy -= deltaEnergy;
			return true;
		}
		else{
			return false;
		}
	}

	// subtract the additional energy and return the remaining energy
	public float SubstractEnergy(float deltaEnergy){
		playerEnergy -= deltaEnergy;
		if(playerEnergy < 0f){
			playerEnergy = 0f;
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

	public float GetEnergy(){
		return playerEnergy;
	}

	public bool IsDepleted(){
		return (playerEnergy == 0f);
	}
}
