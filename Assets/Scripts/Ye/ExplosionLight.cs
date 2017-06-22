using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionLight : MonoBehaviour {

    SFLight sfLight;

    public float maxTime = 0.1f;
    public float endTimeFactor = 20f;  
    public float maxIntensity = 3f;

	// Use this for initialization
	void Start () {
        sfLight = GetComponent<SFLight>();        
	}

    private void OnEnable()
    {
        StartCoroutine(ExplodeLightIE());
    }

    // Update is called once per frame
    void Update() {

	}

    IEnumerator ExplodeLightIE()
    {
        DOTween.To(() => sfLight.intensity, x => sfLight.intensity = x, maxIntensity, maxTime).SetEase(Ease.InCirc);
        yield return new WaitForSeconds(maxTime);
        DOTween.To(() => sfLight.intensity, x => sfLight.intensity = x, 0f, maxTime * endTimeFactor).SetEase(Ease.OutQuart);
        yield return null;
    }
}
