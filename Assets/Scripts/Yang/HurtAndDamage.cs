using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HurtAndDamage : MonoBehaviour {

	public TypeDamagePair[] DamageOtherList;
	public TypeDamagePair[] HurtSelfList;

	Dictionary<ObjectType, float> DamageOtherDict = new Dictionary<ObjectType, float>();
	Dictionary<ObjectType, float> HurtSelfDict = new Dictionary<ObjectType, float>();

	// if true, this object would kill anything collides into itself
	public bool instantKillOther = false;
	// if true, this object would kill itself when collides into anything
	public bool instantKillSelf = false;

	// if set true, this object would not be able to hurt others anymore,
	// even with instantKillOther enabled.
	[ReadOnly]public bool canHurtOther = true;
	// if set false, this object would not be able to hurt itself anymore,
	// even with instantKillSelf enabled.
	[ReadOnly]public bool canHurtSelf = true;

	// bullet will kill itself on collision no matter what happens
	public bool isBullet = false;

	// if collide with a type in the ignoredTypeList, nothing would happen
	public ObjectType[] ignoredTypeList;

	HealthSystem selfHealthSystem;
	// Use this for initialization
	void Start () {
		// initialize fields
		selfHealthSystem = GetComponent<HealthSystem> ();

		// translate the two arrays into dictionary to boost speed
		foreach (TypeDamagePair pair in DamageOtherList){
			DamageOtherDict.Add (pair.type, pair.damage);
		}
		foreach (TypeDamagePair pair in HurtSelfList){
			HurtSelfDict.Add (pair.type, pair.damage);
		}
	}

	/* this object could only hurt itself when:
	 * 1. this.canHurtSelf && other.canHurtOther
	 * 2. this.canHurtSelf && other does not have HurtAndDamage
	 */
	bool VerifyHurtSelf(Transform other){
		if(!this.canHurtSelf){
			return false;
		}
		HurtAndDamage hd = other.GetComponent<HurtAndDamage> ();
		if(hd == null || hd.canHurtOther == true){
			return true;
		} else{
			return false;
		}
	}

	/* this object could only hurt other when:
	 * 1. this.canHurtOther && other.canHurtSelf
	 * 2. this.canHurtOther && other does not have HurtAndDamage
	 */
	bool VerifyHurtOther(Transform other){
		if(!this.canHurtOther){
			return false;
		}
		HurtAndDamage hd = other.GetComponent<HurtAndDamage> ();
		if(hd == null || hd.canHurtSelf == true){
			return true;
		} else{
			return false;
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		HurtDamageLogic (coll);
	}

	void OnCollisionStay2D(Collision2D coll){
		HurtDamageLogic (coll);
	}



	void HurtDamageLogic(Collision2D coll){

		if(isBullet && selfHealthSystem ){
			selfHealthSystem.isImmune = false;
			selfHealthSystem.InstantDead ();
		}


		ObjectIdentity oi = coll.transform.GetComponent<ObjectIdentity> ();
		if(oi != null && ignoredTypeList.Contains(oi.objType)){
			// the target is ignored
			return;
		}


		if(instantKillOther && VerifyHurtOther(coll.transform)){
			HealthSystem hs = coll.transform.GetComponent<HealthSystem> ();
			if(hs){
				hs.InstantDead ();
			}
		}

		// if instant kill self,  will override other's can hurt other
		// if(instantKillSelf && selfHealthSystem && !this.canHurtSelf){
		if(instantKillSelf && selfHealthSystem && VerifyHurtSelf(coll.transform)){
			selfHealthSystem.InstantDead ();
		}

		// get the type of the colliding object
		if(oi == null){
			return;
		}

//		Debug.Log (oi);

		// if the colliding object has an identity and type...
		ObjectType otherType = oi.objType;
		float damage;
		if(!instantKillOther && 
			DamageOtherDict.TryGetValue(otherType, out damage) &&
			VerifyHurtOther(coll.transform) )
		{
//			Debug.Log (damage);
			// do damage to the other type
			HealthSystem hs = coll.transform.GetComponent<HealthSystem> ();
			if(hs){
				hs.Damage (damage);
			}
		}
			

		if(!instantKillSelf &&
			HurtSelfDict.TryGetValue(otherType, out damage) &&
			VerifyHurtSelf(coll.transform) ) 
		{
			// do damage to this object itself
//			Debug.Log (otherType);
//			Debug.Log ("Damage");
			if(selfHealthSystem){
				selfHealthSystem.Damage (damage);
			}
		}



	}



}


// serializable class for visualing the table
[System.Serializable]
public class TypeDamagePair {
	public ObjectType type;
	public float damage;
}