using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit Collider");
        Destroy(other.gameObject);
    }

}
