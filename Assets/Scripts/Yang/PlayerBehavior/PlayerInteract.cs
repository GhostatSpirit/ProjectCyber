using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInteract : MonoBehaviour {
	public Transform otherPlayer;
	public float chargeDistance = 1.0f;
	public float chargeSpeed = 20f;

	public GameObject hintImage;

	InputDevice device;

	PlayerHintUI _hintUI;
	PlayerHintUI hintUI{
		get{
			if (!hintImage)
				return null;
			if(!_hintUI){
				_hintUI = hintImage.GetComponent<PlayerHintUI> ();
			}
			return _hintUI;
		}
	}

	HealthSystem _selfHealth;
	HealthSystem selfHealth{
		get{
			if(!_selfHealth){
				_selfHealth = GetComponent<HealthSystem> ();
			}
			return _selfHealth;
		}
	}

	HealthSystem _otherHealth;
	HealthSystem otherHealth{
		get{
			if(!otherPlayer){
				return null;
			}
			if(!_otherHealth){
				_otherHealth= otherPlayer.GetComponent<HealthSystem> ();
			}
			return _otherHealth;
		}
	}


	float reviveDistance;
	// Use this for initialization
	IEnumerator Start () {
		reviveDistance = chargeDistance;

		yield return new WaitUntil (() => {
			return (selfHealth != null && otherHealth != null);
		});

//		Debug.Log ("init complete");
	}
	
	// Update is called once per frame
	void Update () {
		device = GetComponent<DeviceReceiver>().GetDevice();

		if(otherPlayer == null){
			return;
		}
		float dist = Vector2.Distance (otherPlayer.position, transform.position);
		if(dist < chargeDistance && !selfHealth.IsDead() && !otherHealth.IsDead()){
			PlayerEnergy energyScript = GetComponent<PlayerEnergy> ();
			if(energyScript != null){
				energyScript.AddEnergy (chargeSpeed * Time.deltaTime);
			}
		}
//
//		Debug.Log ("self dead: " + selfHealth.IsDead ().ToString ());
//		Debug.Log ("other dead: " + otherHealth.IsDead ().ToString ());
//		Debug.Log (dist);

		if(!selfHealth.IsDead() && otherHealth.IsDead() && dist < reviveDistance){

			if(hintUI){
				hintUI.hint = PlayerHintUI.HintStatus.PressY;
			}
			if((device != null) && device.Action4.WasPressed){
				otherHealth.Revive ();
			}

		} else{
			if (hintUI) {
				hintUI.hint = PlayerHintUI.HintStatus.None;
			}
		}

	}
}
