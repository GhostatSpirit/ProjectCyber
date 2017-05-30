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
// added tweening effect when hurt
using DG.Tweening;

public class HealthSystem : MonoBehaviour {
	public float maxHealth = 100f;

	public bool hasImmunuePeriod = true;

	[Range(0f, Mathf.Infinity)]
	public float hurtImmunePeriod = 1.5f;
	public Color hurtColor = new Color32 (255, 107, 107, 255);
	public int flashCount = 3;

	public bool destoryOnDead = false;
	public float destoryDelay = 1f;

	public bool changeColor = true;
	public Color deadColor = Color.red;

	public bool stopMovement = true;

	public Text displayText;

	// this object's current health
	[ReadOnly]public float objHealth;

	// whether this object is currently immune or not
	bool isImmune = false;
	bool isHarmless = false;

	bool _isDead = false;
	bool isDead{
		get{
			return _isDead;
		}
		set{
			bool oldIsDead = _isDead;
			if(oldIsDead != value){
				if(value == true){
					// object become dead
					DeathHandler ();
				} else{
					// object is revived
					ReviveHandler ();
				}
				_isDead = value;
			}
		}
	}

	// this action is executed as soon as this object is dead
	public event Action<Transform> OnObjectDead;
	public event Action<Transform> OnObjectRevive;

	public event Action<Transform> OnObjectHurt;


	SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();

		if(hurtColor == Color.white){
			hurtColor = new Color32 (255, 107, 107, 255);
		}

//		Debug.Log (hurtColor);

		objHealth = maxHealth;
		InitDeathHandler ();
	}
	
	// Update is called once per frame
	void Update () {
		if(displayText != null){
			displayText.text = Mathf.Round(objHealth).ToString();
		}
	}

	public Coroutine HurtCoroutine;

	IEnumerator	HurtImmuneIE(){
		// start the immune period
		if (hasImmunuePeriod) {
			StartImmune (false);
		}
		if(sr){
			// multiply flash count by 2 to ensure the color gets back to the original
			sr.DOColor(hurtColor, hurtImmunePeriod).SetEase(Ease.OutFlash, 2 * flashCount, 0);
		}
		yield return new WaitForSeconds (hurtImmunePeriod);

		if (hasImmunuePeriod) {
			EndImmune (false);
		}

		HurtCoroutine = null;
	}
		

	void StartHurtBehaviour(){
		if(HurtCoroutine == null && hurtImmunePeriod > 0f){
			HurtCoroutine = StartCoroutine (HurtImmuneIE ());
		}
	}



	/* public exposed methods for managing health */
	/**********************************************/
	public void Damage(float deltaHealth){
		if(isImmune)	return;
		if (isDead)		return;
	
		float tempHealth = objHealth;
		tempHealth -= deltaHealth;
		if(tempHealth <= 0f){
			objHealth = 0f;
			// the object is dead, call DeathHandler()
			isDead = true;
		} else{
			if(OnObjectHurt != null){
				OnObjectHurt (this.transform);
			}
			if (hurtImmunePeriod > 0f) {
				StartHurtBehaviour ();
			}
			objHealth = tempHealth;
		}
	}

	public void Heal(float deltaHealth){
		// we cannot heal a dead object, use revive instead
		if (isDead)		return;

		objHealth += deltaHealth;
		if(objHealth > maxHealth){
			objHealth = maxHealth;
		}
	}

	public void Revive(float healthPercentage = 0.5f){
		if (healthPercentage <= 0f || !isDead)
			return;
		if(healthPercentage > 1f){
			healthPercentage = 1f;
		}
		// revive the object now, first add the health
		objHealth += maxHealth * healthPercentage;
		isDead = false;
	}


	/* instantly kill this object, making its health to 0 */
	public void InstantDead(){
		if (!isImmune) {
			objHealth = 0f;
			isDead = true;
		}
	}
		
	public void StartHarmless(bool setColor = true){
		// during harmless, this obj cannot hurt others any more
		HurtAndDamage hd = GetComponent<HurtAndDamage> ();
		if(hd){
			hd.canHurtOther = false;
		}
		if (setColor) {
			if (GetComponent<SpriteRenderer> ().color == Color.white) {
				GetComponent<SpriteRenderer> ().color = Color.green;
			}
		}
		isHarmless = true;
	}

	/* end the harmless period, this object would be hurt again*/
	public void EndHarmless(bool setColor = true){
		HurtAndDamage hd = GetComponent<HurtAndDamage> ();
		if(hd){
			hd.canHurtOther = true;
		}
		if (setColor) {
			if (GetComponent<SpriteRenderer> ().color == Color.green) {
				GetComponent<SpriteRenderer> ().color = Color.white;
			}
		}
		isHarmless = false;
	}

	public void StartImmune(bool setColor = true){
		HurtAndDamage hd = GetComponent<HurtAndDamage> ();
		if(hd){
			hd.canHurtSelf = false;
		}
		if (setColor) {
			if (GetComponent<SpriteRenderer> ().color == Color.white) {
				GetComponent<SpriteRenderer> ().color = Color.green;
			}
		}
		isImmune = true;
	}

	public void EndImmune(bool setColor = true){
		HurtAndDamage hd = GetComponent<HurtAndDamage> ();
		if(hd){
			hd.canHurtSelf = true;
		}
		if (setColor) {
			if (GetComponent<SpriteRenderer> ().color == Color.green) {
				GetComponent<SpriteRenderer> ().color = Color.white;
			}
		}
		isImmune = false;
	}



	public Coroutine ImmuneCoroutine;

	public void Immune(float period){
		if(ImmuneCoroutine == null && period > 0f){
			ImmuneCoroutine = StartCoroutine (ImmuneIE(period));
		}
	}

	IEnumerator ImmuneIE(float period){
		// start the immune period
		StartImmune (false);
		yield return new WaitForSeconds (period);
		EndImmune (false);
		ImmuneCoroutine = null;
	}


	bool IsHarmless(){
		return this.isHarmless;
	}

	bool IsImmune(){
		return this.isImmune;
	}

	public float GetHealth(){
		return this.objHealth;
	}

	public bool IsDead(){
		return this.isDead;
	}


	/**********************************************/
	void InitDeathHandler(){
		if(changeColor){
			OnObjectDead += ChangeColor;
			OnObjectRevive += ResetColor;
		}
		if(stopMovement){
			OnObjectDead += StopMovement;
		}

		if(destoryOnDead){
			OnObjectDead += (Transform trans) => {
				StartCoroutine (DestroyObjectIE (trans, destoryDelay));
			};
		}
	}

	void DeathHandler(){
		// execute the action
		if(OnObjectDead != null){
			OnObjectDead (this.transform);
		}
	}

	void ReviveHandler(){
		if(OnObjectRevive != null){
			OnObjectRevive (this.transform);
		}
	}
		
	/* private methods for OnObjectDead()
	/******************************************************************/

	/* ChangeColor:
	 * change the color of the SpriteRenderer to a given color
	 */
	void ChangeColor(Transform trans){
		if(sr != null){
			sr.DOColor (deadColor, destoryDelay / 2f).SetEase (Ease.OutCubic);
		}
	}

	void ResetColor(Transform trans){
		if(sr != null){
			sr.color = Color.white;
		}
	}

	/* StopMovement:
	 * stop this object froming moving
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
