using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDead : MonoBehaviour {

	public GameObject explosion;
	public float explosionDelay = 0.125f;

	public Transform explosionParent;

	public bool stopMovement = true;

	HealthSystem hs;
	Rigidbody2D body;
	LineUpdate lu;

	// Use this for initialization
	void Start () {
		hs = GetComponent<HealthSystem> ();
		body = GetComponent<Rigidbody2D> ();
		lu = GetComponent<LineUpdate> ();


		if(hs){
			hs.OnObjectDead += StartExplosion;
			hs.destoryDelay = Mathf.Max (hs.destoryDelay, explosionDelay);
		}
	}



	Coroutine explosionCoroutine;

	public void StartExplosion(Transform objTrans){
		if(explosionCoroutine == null){
			explosionCoroutine = StartCoroutine (StartExplosionIE());
		}
	}

	IEnumerator StartExplosionIE(){
		// stop the roomba from moving
		if (stopMovement && body) {
			body.velocity = Vector3.zero;
		}

		yield return new WaitForSeconds (explosionDelay);

		if(explosion != null){
			if(lu){
				lu.DisableLine ();
			}

			GameObject explosionGO = Instantiate (explosion, transform.position, 
				Quaternion.Euler (0f, 0f, 0f), explosionParent);
			ParticleLayerSetter setter = explosionGO.GetComponentInChildren<ParticleLayerSetter> ();
			SpriteRenderer sr = GetComponent<SpriteRenderer> ();

			if(setter && sr){
				setter.SetSortingLayer (sr.sortingLayerID);
			}
		}

		explosionCoroutine = null;
	}

}
