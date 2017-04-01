/* Written by Yang Liu.
 * This script:
 * 1. contains the definition of a HealthSystem,
 * 2. defines public methods such as Damage(), Heal()
 * 3. defines an Action OnObjectDead() that would be called
 *    when this object is dead
 * 4. defines StartImmune() & EndImmune(), 
 *    during immune, this object could not be hurt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.ObjectModel;

public class HealthSystem : MonoBehaviour {
	public float maxHealth = 100f;

	public bool destoryOnDead = false;
	public float destoryDelay = 1f;

	public bool changeColor = true;
	public Color newColor = Color.red;

	public bool stopMovement = true;

	public Text displayText;

	// this object's current health
	public float objHealth;
	// whether this object is currently immune or not
	bool isImmune = false;

	// this action is executed as soon as this object is dead
	public event Action<Transform> OnObjectDead;

	// Use this for initialization
	void Start () {
		objHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if(displayText != null){
			displayText.text = Mathf.Round(objHealth).ToString();
		}
	}

	/* public exposed methods for managing health */
	/**********************************************/
	public void Damage(float deltaHealth){
		if(isImmune) {
			return;
		}

		float tempHealth = objHealth;
		tempHealth -= deltaHealth;
		if(tempHealth <= 0f){
			objHealth = 0f;
			// the object is dead, call DeathHandler()
			DeathHandler ();
		} else{
			objHealth = tempHealth;
		}
	}

	public void Heal(float deltaHealth){
		objHealth += deltaHealth;
		if(objHealth > maxHealth){
			objHealth = maxHealth;
		}
	}

	/* instantly kill this object, making its health to 0 */
	public void InstantDead(){
		objHealth = 0f;
		DeathHandler ();
	}

	/* start the immune period, this object would not be hurt*/
	public void StartImmune(){
		// during immune, this obj cannot hurt others any more
		HurtAndDamage hd = GetComponent<HurtAndDamage> ();
		if(hd){
			hd.canHurtOther = false;
		}
		if (GetComponent<SpriteRenderer> ().color == Color.white) {
			GetComponent<SpriteRenderer> ().color = Color.green;
		}
		isImmune = true;
	}

	/* end the immune period, this object would be hurt again*/
	public void EndImmune(){
		HurtAndDamage hd = GetComponent<HurtAndDamage> ();
		if(hd){
			hd.canHurtOther = true;
		}
		if (GetComponent<SpriteRenderer> ().color == Color.green) {
			GetComponent<SpriteRenderer> ().color = Color.white;
		}
		isImmune = false;
	}

	public float GetHealth(){
		return this.objHealth;
	}


	/**********************************************/


	void DeathHandler(){
		if(changeColor){
			OnObjectDead += ChangeColor;
		}
		if(stopMovement){
			OnObjectDead += StopMovement;
		}

		if(destoryOnDead){
			OnObjectDead += (Transform trans) => {
				StartCoroutine (DestroyObjectIE (trans, destoryDelay));
			};
		}
		// execute the action
		if(OnObjectDead != null){
			OnObjectDead (this.transform);
		}

	}
		
	/* private methods for OnObjectDead()
	/******************************************************************/

	/* ChangeColor:
	 * change the color of the SpriteRenderer to a given color
	 */
	void ChangeColor(Transform trans){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		if(sr != null){
			sr.color = newColor;
		}
	}

	/* StopMovement:
	 * stop this object froming moving by changing body type to "Static"
	 */
	void StopMovement (Transform trans){
		Rigidbody2D myRigidbody = GetComponent<Rigidbody2D> ();
		if(myRigidbody){
			myRigidbody.simulated = false;
		}
	}

	// IEnumerator for invoking a function with parameters
	// this IEnumerator is for destory this GameObject
	IEnumerator DestroyObjectIE(Transform trans, float delay){
		yield return new WaitForSeconds (delay);
		Destroy (trans.gameObject);
	}

}
