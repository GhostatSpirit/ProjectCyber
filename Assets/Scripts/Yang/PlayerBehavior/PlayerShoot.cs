﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerShoot : MonoBehaviour {
	public float initialVelocity = 30f;

	// replaced with InControl
	//public string shootTriggerName;
	public int playerIndex = 0;
	public Transform deviceAssigner;
	InputDevice myInputDevice;

	public GameObject bulletPrefab;
	public float coolDownDelay = 1f;

	//private bool m_isAxisInUse;
	private bool isCooledDown;

	PlayerEnergy energySys;
	public float energyConsume = 10f;
	//public bool isPS4Controller;
	public AudioClip shootSound;

	public int bulletCount = 3;
	public float deltaAngle = 10f;

	AudioSource myAudioSource;

	// Use this for initialization
	void Start () {
		isCooledDown = true;
		myAudioSource = transform.parent.GetComponent<AudioSource> ();
		energySys = transform.parent.GetComponent<PlayerEnergy> ();
		//myInputDevice = InputManager.Devices [deviceIndex];
	}
	
	// Update is called once per frame
	void Update () {
		myInputDevice = deviceAssigner.
			GetComponent<DeviceAssigner>().GetPlayerDevice(playerIndex);
		if(myInputDevice == null){
			return;
		}

		if(myInputDevice.Action1.WasPressed){
			if (bulletPrefab != null && isCooledDown) {
				if (energySys.UseEnergy(energyConsume)) {
					// if we sucessfully used the energy, shoot wave
					ShootWave ();
				}
			}
		}

//		float triggerAxis = Input.GetAxis (shootTriggerName);
//		//Debug.Log (triggerAxis);
//		if(isPS4Controller){
//			// -1 ~ 1 -> 0 ~ 1
//			triggerAxis = triggerAxis / 2.0f + 0.5f;
//		}
//
//		if(Input.GetAxis(shootTriggerName) > 0.5f && !m_isAxisInUse){
//
//			m_isAxisInUse = true;
//			ShootWave ();
//
//		}
//		if(Input.GetAxis(shootTriggerName) == 0){
//			m_isAxisInUse = false;
//		}
	}

	void ShootWave(){

        // ye added shootsound
		if (GetComponentInParent<ShootSound> ()) {
			GetComponentInParent<ShootSound> ().hackShoot ();
		}

		// instantiate the bullet prefabs

		float midAngleZ = transform.rotation.eulerAngles.z;
		float startAngleZ = midAngleZ - deltaAngle * (bulletCount - 1) / 2.0f;
		float endAngleZ = midAngleZ + deltaAngle * (bulletCount - 1) / 2.0f;

		for(float angleZ = startAngleZ; angleZ <= endAngleZ; angleZ += deltaAngle){
			Quaternion newBulletRot = transform.rotation;
			Vector3 shooterEuler = transform.rotation.eulerAngles;
			newBulletRot.eulerAngles = new Vector3 (shooterEuler.x, shooterEuler.y, angleZ);
			GameObject bulletObj = Instantiate (bulletPrefab, transform.position, newBulletRot);

			// set init velocity of the bullet
			bulletObj.GetComponent<Rigidbody2D> ().velocity = bulletObj.transform.up.normalized * initialVelocity;

			BulletHit bulletHit = bulletObj.GetComponent<BulletHit> ();
			if(bulletHit){
				Debug.Log ("Set init velo");
				bulletHit.initVelocity = transform.up.normalized;

				// Debug.Log (bulletObj.transform.up.normalized);
			}
			// tell the bulletObj the init velocity
			//bulletObj.GetComponent<BulletDeflect> ().initialVelocity = initialVelocity;
		}

		// play shoot sound
		myAudioSource.PlayOneShot(shootSound);
			
		isCooledDown = false;

		Invoke ("CoolDown", coolDownDelay);
		//}
	}

	void CoolDown(){
		isCooledDown = true;
	}
}
