using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightTowerBehaviour : MonoBehaviour {
	ControlStatus cs;

	public SpriteRenderer fullLightSR;
	public SFLight sfLight;

	public float showSpriteDuration = 1f;
	public float lightDuration = 2f;

	// Use this for initialization
	void Start () {
		cs = GetComponent<ControlStatus> ();

		cs.OnCutByPlayer += LightTower;
		cs.OnLinkedByEnemy += UnlightTower;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LightTower(Transform objTrans){
		if(fullLightSR){
			// reset color alpha
			Color newColor = fullLightSR.color;
			newColor.a = 0f;
			fullLightSR.color = newColor;
			// turn on spriteRenderer
			fullLightSR.enabled = true;
			// dotween
			fullLightSR.DOFade (1f, showSpriteDuration);
		}
		if(sfLight){
			float finalIntensity = sfLight.intensity;
			sfLight.intensity = 0f;
			sfLight.enabled = true;
			// dotween
			DOTween.To (() => sfLight.intensity, 
						 x => sfLight.intensity = x, finalIntensity, lightDuration).
						 SetEase(Ease.InBounce);
		}
	}

	void UnlightTower(Transform objTrans){
		if(fullLightSR){
			// dotween
			fullLightSR.DOFade (0f, showSpriteDuration).OnComplete(()=>fullLightSR.enabled = false);
		}
		if(sfLight){
			// dotween
			DOTween.To (() => sfLight.intensity, 
				x => sfLight.intensity = x, 0f, lightDuration).
				OnComplete(()=>sfLight.enabled = false);
		}
	}
}
