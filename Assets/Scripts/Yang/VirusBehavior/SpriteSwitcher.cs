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
				spRenderer.sprite = BossControlSprite;
				break;
			case Controller.Hacker:
				spRenderer.sprite = HackerControlSprite;
				break;
			case Controller.None:
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
	}

	void SpriteToBoss(Transform virusTrans){
		spRenderer = virusTrans.GetComponent<SpriteRenderer> ();
		if(renderer && BossControlSprite){
			spRenderer.sprite = BossControlSprite;
		}
	}

	void SpriteToHacker(Transform virusTrans){
		spRenderer = virusTrans.GetComponent<SpriteRenderer> ();
		if(renderer && HackerControlSprite){
			spRenderer.sprite = HackerControlSprite;
		}
	}

	void SpriteToNone(Transform virusTrans){
		spRenderer = virusTrans.GetComponent<SpriteRenderer> ();
		if(spRenderer && NoneControlSprite){
			spRenderer.sprite = NoneControlSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
