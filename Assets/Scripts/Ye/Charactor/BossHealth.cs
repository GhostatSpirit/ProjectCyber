using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

public class BossHealth : MonoBehaviour {

    public GameObject Boss;
    public float lerpSpeed = 10f;
    public bool bossAppear = false;

	public float appearTime = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		HealthSystem HS = null;
		if (Boss) {
			HS = Boss.GetComponent<HealthSystem> ();
		}

		float health = 0f, ratio = 0f;
		if (HS) {
			health = HS.objHealth;
			ratio = health / HS.maxHealth;
		}

        if (bossAppear == true)
        {
			// set initial color to transparent
			GetComponentsInParent<Image> () [0].color = new Color32 (255, 255, 255, 0);
			GetComponentsInParent<Image> () [1].color = new Color32 (255, 255, 255, 0);

            GetComponentsInParent<Image>()[0].enabled = true;
            GetComponentsInParent<Image>()[1].enabled = true;

			float current = GetComponent<Image>().fillAmount;
			GetComponent<Image>().fillAmount = Mathf.Lerp(current, ratio, lerpSpeed * Time.deltaTime);

			GetComponentsInParent<Image> () [0].DOFade (1f, appearTime);
			GetComponentsInParent<Image> () [1].DOFade (1f, appearTime);

            
        }
        else
        {
            GetComponentsInParent<Image>()[0].enabled = false;
            GetComponentsInParent<Image>()[1].enabled = false;
        }
    }
}
