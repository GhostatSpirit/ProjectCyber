using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheck : MonoBehaviour {

    public GameObject boss;
    GameObject portal;
    HealthSystem bossHS;
    public float showTime;

	// Use this for initialization
	void Start () {
        portal = transform.GetChild(0).gameObject;
        portal.SetActive(false);
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
            routine = StartCoroutine(portalIE());
        }
    }

    IEnumerator portalIE()
    {
        yield return new WaitForSeconds(showTime);
        portal.SetActive(true);
    }
    
     
}
