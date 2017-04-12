using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class BossHealth : MonoBehaviour {

    public GameObject Boss;
    public float lerpSpeed = 10f;
    public bool bossAppear = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HealthSystem HS = Boss.GetComponent<HealthSystem>();
        float health = HS.objHealth;
        float ratio = health / HS.maxHealth;

        if (bossAppear == true)
        {
            GetComponentsInParent<Image>()[0].enabled = true;
            GetComponentsInParent<Image>()[1].enabled = true;
            float current = GetComponent<Image>().fillAmount;
            GetComponent<Image>().fillAmount = Mathf.Lerp(current, ratio, lerpSpeed * Time.deltaTime);
        }
        else
        {
            GetComponentsInParent<Image>()[0].enabled = false;
            GetComponentsInParent<Image>()[1].enabled = false;
        }
    }
}
