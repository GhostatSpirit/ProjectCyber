using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRecall: MonoBehaviour {
    bool exist = true;
    public float recallTime = 10;
    // Use this for initialization

	
    void Start () {
        
        Status(true);
	}

	// Update is called once per frame
    // Recall function added to OnObjectDead
	void Update () {
        gameObject.GetComponent<HealthSystem>().OnObjectDead += WallDestroy;
	}

    // define status of gameObject and all children 
    void Status(bool status)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = status;
        gameObject.GetComponent<PolygonCollider2D>().enabled = status;
        gameObject.GetComponent<HurtAndDamage>().enabled = status;
        int childNum = transform.childCount;
        for (int i = 0; i < childNum; i++)
        {
            Transform childTrans = transform.GetChild(i);
            childTrans.GetComponent<SpriteRenderer>().enabled = status;
            childTrans.GetComponent<PolygonCollider2D>().enabled = status;
            childTrans.GetComponent<WallTransparency>().enabled = status;
        }

    }

    // Wall exists
    void WallExist()
    {
        Status(true) ;
    }

    // Wall destroy and recall
    void WallDestroy(Transform transform)
    {
        Status(false);
        Invoke("WallExist", recallTime);
    }

}

