using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallStatus : MonoBehaviour {

    // Four BreakableWall status: Full, Break, Broken, Recover
    public enum WallStatus { Full, Break, Broken, Recover };
    public WallStatus Status = WallStatus.Full;
    
    public float deadHealth = 0f;

    // Use this for initialization
    void Start () {

        // Wall is initially full
         Status = WallStatus.Full;
            
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(Status);

        if (GetComponent<HealthSystem>().GetHealth() == GetComponent<HealthSystem>().maxHealth)
        {
            Status = WallStatus.Full;
        }

		if (GetComponent<HealthSystem>().IsDead())
        {
            Status = WallStatus.Break;
        }
        /*
        // When Broken disable collider
        if (Status == WallStatus.Broken)
        {
            Debug.Log("Hit");
            GetComponent<PolygonCollider2D>().enabled = false; 
        }

        // When Full anable colider
        if (Status == WallStatus.Full)
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
        */
    }
    /*
    //  When catch collision, set Status to Break
    void OnCollisionEnter2D(Collision2D collision)
    {
        Status = WallStatus.Break;
    }
    */
}
