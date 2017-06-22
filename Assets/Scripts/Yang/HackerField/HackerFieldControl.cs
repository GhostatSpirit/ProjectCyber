using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class HackerFieldControl : MonoBehaviour {

	[ReadOnly]public bool chargeCanceled = false;

	[Range(0f, 1f)]
	public float chargeMoveSpeedFactor = 0f;

	[Range(0f, 1f)]
	public float hackMoveSpeedFactor = 0.5f;
	float oldSpeedFactor = 1f;

	public float energyConsume = 20f;

	InputDevice myInputDevice;
	Animator animator;

	PlayerMovement playerMove;
	PlayerEnergy playerEnergy;
	//HackerMovementAnim moveAnim;

	// Use this for initialization
	void Start () {
		myInputDevice = GetComponentInParent<DeviceReceiver>().GetDevice();
		animator = GetComponent<Animator> ();
		playerMove = GetComponentInParent<PlayerMovement> ();
		oldSpeedFactor = playerMove.moveSpeedFactor;
		playerEnergy = GetComponentInParent<PlayerEnergy> ();
		// moveAnim = GetComponentInParent<HackerMovementAnim> ();

	}
	
	// Update is called once per frame
	void Update () {
		// update input device every frame
		myInputDevice = GetComponentInParent<DeviceReceiver>().GetDevice();

		// Debug.Log (this.chargeCanceled);
	}

	public bool wasButtonPressed{
		get{
			if(this.enabled == false){
				return false;
			}

			if (myInputDevice != null && myInputDevice.Action1.WasPressed) {
				return true;
			} else {
				return false;
			}
		}
	}

	public bool isButtonReleased{
		get{
			if(this.enabled == false){
				return true;
			}

			if (myInputDevice == null || !myInputDevice.Action1.IsPressed) {
				return true;
			} else {
				return false;
			}
		}
	}

	public void ReachedFirstFrame(){
		if(chargeCanceled){
			animator.SetTrigger ("exitCharge");
		}
		chargeCanceled = false;
	}

	public void ResetMoveSpeed(){
		playerMove.moveSpeedFactor = oldSpeedFactor;
	}

	public void SetChargeMoveSpeed(){
		playerMove.moveSpeedFactor = chargeMoveSpeedFactor;
			
	}

	public void SetHackMoveSpeed(){
		playerMove.moveSpeedFactor = hackMoveSpeedFactor;

	}

	public bool ConsumeEnergy(){
		return playerEnergy.UseEnergy (energyConsume * Time.deltaTime);
	}
}
