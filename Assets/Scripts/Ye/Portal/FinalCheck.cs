using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheck : MonoBehaviour {

    public GameObject boss;
    GameObject finalObeject;
    HealthSystem bossHS;
    public float showTime;

	// Use this for initialization
	void Start () {
        finalObeject = transform.GetChild(0).gameObject;
        finalObeject.SetActive(false);
        bossHS = boss.GetComponent<HealthSystem>();
		bossHS.OnObjectDead += portalable;
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    Coroutine routine;

    void portalable(Transform trans )
    {
       if(routine == null)
        {
            routine = StartCoroutine(finalObejectIE());
        }
    }

    IEnumerator finalObejectIE()
    {
        yield return new WaitForSeconds(showTime);
        finalObeject.SetActive(true);
    }
    
     
}
