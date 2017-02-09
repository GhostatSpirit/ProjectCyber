using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCut : MonoBehaviour {


    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 CollisionPoint;
        CollisionPoint = collision.contacts[0].point;
        print(CollisionPoint);
        Debug.Log(CollisionPoint);

    }

    void OnTriggerEnter2D(Collider2D other){
        Invoke("OnCollisionEnter2D", 0.00001f);
        if (other.gameObject.tag == "EnemyLine")
            Debug.Log("Object caught");
		    Destroy (other.gameObject);
	}

}
