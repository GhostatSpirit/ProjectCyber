using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCut : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Object caught");
        if (other.gameObject.tag == "EnemyLine")
		    Destroy (other.gameObject);
	}
}
