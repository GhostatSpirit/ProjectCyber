using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteSwitcher : MonoBehaviour {
	public Sprite BossControlSprite;
	public Sprite HackerControlSprite;
	public Sprite NoneControlSprite;

	public Controller defaultController = Controller.Boss;

	SpriteRenderer spRenderer;
	// Use this for initialization
	void Start () {
		spRenderer = GetComponent<SpriteRenderer> ();
		if (spRenderer) {
			switch (defaultController) {
			case Controller.Boss:
				if(BossControlSprite)
					spRenderer.sprite = BossControlSprite;
				break;
			case Controller.Hacker:
				if(HackerControlSprite)
					spRenderer.sprite = HackerControlSprite;
				break;
			case Controller.None:
				if(NoneControlSprite)
					spRenderer.sprite = NoneControlSprite;
				break;
			}
		}
		// add the switch sprite methods to actions
		ControlStatus cs = GetComponent<ControlStatus> ();
		if(cs){
			cs.OnLinkedByEnemy += SpriteToBoss;
			cs.OnLinkedByPlayer += SpriteToHacker;
			cs.OnCutByEnemy += SpriteToNone;
			cs.OnCutByPlayer += SpriteToNone;
		}
		else{
			Debug.LogError ("cannot find control status");
		}
	}

	public void SpriteToBoss(Transform virusTrans){
		spRenderer = virusTrans.GetComponent<SpriteRenderer> ();
//		Debug.Log ("Changing Sprite to Boss");
		if(spRenderer && BossControlSprite){
			spRenderer.sprite = BossControlSprite;
		}
	}

	public void SpriteToHacker(Transform virusTrans){
		spRenderer = virusTrans.GetComponent<SpriteRenderer> ();
//		Debug.Log ("Changing Sprite to hacker");
		if(spRenderer && HackerControlSprite){
			spRenderer.sprite = HackerControlSprite;
		}
	}

	public void SpriteToNone(Transform virusTrans){
		spRenderer = virusTrans.GetComponent<SpriteRenderer> ();
//		Debug.Log ("Changing Sprite to none");
		if(spRenderer && NoneControlSprite){
			spRenderer.sprite = NoneControlSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
