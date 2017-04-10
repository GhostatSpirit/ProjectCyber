using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItalianGunStatus : MonoBehaviour {

    // Four ItalianGun status: Enemy, Controlled
    public enum GunStatus { Enemy, Controlled };
    public GunStatus Status = GunStatus.Enemy;

    // Use this for initialization
    void Start()
    {

        // Wall is initially full
        Status = GunStatus.Enemy;

    }

    // Update is called once per frame
    void Update()
    {

        // When Broken disable collider
        if (Status == GunStatus.Enemy)
        {
            // GetComponent<PolygonCollider2D>().enabled = false;
        }

        // When Full anable colider
        if (Status == GunStatus.Controlled)
        {
            // GetComponent<PolygonCollider2D>().enabled = true;
        }
    }


    /*
    //  When catch collision, set Status to Break
    void OnCollisionEnter2D(Collision2D collision)
    {
        Status = GunStatus.Break;
    }
    */
}
