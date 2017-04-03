using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent (typeof(DeviceReceiver))]
[RequireComponent (typeof(LineCut))]
public class DartSkill : MonoBehaviour {
	[HideInInspector]public bool darting = false;
	public float dartSpeed = 20f;
	public float dartDuration = 0.1f;
    public float MinDuration = 0.1f;
    public float MaxDuration = 0.2f;

    

	public float startMovementDelay = 0.6f;
	public float coolDownDelay = 0.6f;
	public float penaltyCoolDownDelay = 3f;

	public float colliderAmpFactor = 2f;

	InputDevice myInputDevice;
	// comment out Unity Input line
	// public string dartButtonName = "Bbutton";

	Rigidbody2D myRigidbody;

	Vector2 dartDirection;
	float timer = 0f;

	CapsuleCollider2D myCapsuleColl;
	float defaultColliderWidth;
	float newColliderWidth;

	// how much energy will one dart consume?
	float energyConsume = 10f;
	PlayerEnergy energySys;

	bool coolDown = true;

	// variable for counting how many enemies the player killed in one dart
	int killCount = 0;

	// LineCut class for enabling AI to cut lines
	LineCut linecut;
	// HeathSys class, enabling immune when darting
	HealthSystem healthSys;
	public float extraImmuneTime = 0.5f;

    // add for charging dart
    


	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myCapsuleColl = GetComponent<CapsuleCollider2D> ();
		defaultColliderWidth = myCapsuleColl.size.x;
		newColliderWidth = defaultColliderWidth * colliderAmpFactor;

		energySys = GetComponent<PlayerEnergy> ();

		linecut = GetComponent<LineCut> ();
		healthSys = GetComponent<HealthSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
		if(myInputDevice == null){
			return;
		}



        /*/
		if(myInputDevice.Action1.IsPressed && !darting && 
			coolDown && energySys.UseEnergy(energyConsume)){
        /*/

        // Ye change 2017/3/25 try to add charging 
        
        ChargingDart x = gameObject.GetComponent<ChargingDart>();
        if ( (x.Status ==  ChargingDart.chargingStatus.StartCharge || x.Status == ChargingDart.chargingStatus.Charging) && !darting &&
            coolDown && energySys.UseEnergy(energyConsume))
        {

            // CAUTION: starting the darting skill
            darting = true;
			// AI can cut lines now
			linecut.couldCut = true;

			if(GetComponent<PlayerMovement>() != null){
				// stop the player from moving around
				GetComponent<PlayerMovement> ().moveEnabled = false;
			} else{
				Debug.Log ("DartSkill: Failed to Find PlayerMovement Script");
			}

			// get the dart Direction
			dartDirection = myRigidbody.velocity.normalized;
			// reset the timer
			timer = 0f;
			// amplify the size of the collider
			myCapsuleColl.size = new Vector2(newColliderWidth, myCapsuleColl.size.y);
			// reset the kill count
			killCount = 0;

			// start the immune buff
			if(healthSys){
				healthSys.StartImmune();
				healthSys.StartHarmless ();
			}
		}

		timer += Time.deltaTime;

        /*/
		if(timer > dartDuration && darting){
        /*/

        // stop dart
        if( darting && (((x.Status != ChargingDart.chargingStatus.Charging && x.Status != ChargingDart.chargingStatus.StartCharge) || timer > MaxDuration) || ( x.Status == ChargingDart.chargingStatus.Release && x.ChargingSum < MinDuration && timer > MinDuration)))
        { 
        

			// CAUTION: stop darting skill
			darting = false;

			// AI can no longer cut lines now
			linecut.couldCut = false;

			// Invoke StartMovement after delay
			Invoke ("StartMovement", startMovementDelay);
			// stop the player
			myRigidbody.velocity = Vector3.zero;
			// start cooling down
			coolDown = false;
			if (killCount != 0) {
				Invoke ("CoolDown", coolDownDelay);
			} else{
				Invoke ("CoolDown", penaltyCoolDownDelay);
			}
			// reset the size of the collider
			myCapsuleColl.size = new Vector2(defaultColliderWidth, myCapsuleColl.size.y);

			// end the immune buff 
			Invoke ("EndImmune", extraImmuneTime);
		}

	}

	void EndImmune(){
		if(healthSys){
			healthSys.EndImmune();
			healthSys.EndHarmless ();
		}
	}

	void FixedUpdate() {
		if(darting){
			myRigidbody.velocity = dartDirection * dartSpeed * Time.fixedDeltaTime * 10f;
		}
			
	}

	void StartMovement(){
		// enable the player to move around again
		if(GetComponent<PlayerMovement>() != null){
			GetComponent<PlayerMovement> ().moveEnabled = true;
		} else{
			Debug.Log ("DartSkill: Failed to Find PlayerMovement Script");
		}
	}

	void CoolDown(){
		coolDown = true;

        // when cooldown reset dartDuration   
        dartDuration = MinDuration;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "AIEnemy" && darting){
			// let the enemy die
			coll.gameObject.GetComponent<DeathHandler> ().LetDead ();
			// add the kill count by one
			killCount++;
		}

	}

}
