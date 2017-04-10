using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class HealthToUI : MonoBehaviour {

    public GameObject Player;
    public float lerpSpeed = 10f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HealthSystem HS = Player.GetComponent<HealthSystem>();
        float health = HS.objHealth;
        float ratio = health / HS.maxHealth;

        float current = GetComponent<Image>().fillAmount;
        GetComponent<Image>().fillAmount = Mathf.Lerp(current, ratio, lerpSpeed * Time.deltaTime);
    }
}
