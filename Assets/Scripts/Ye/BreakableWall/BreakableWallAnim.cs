using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallAnim: MonoBehaviour
{

    public float RecoverTime;

    Animator anim;
    
	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Break",false);
        anim.SetBool("Recover",false);
        
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (GetComponent<BreakableWallStatus>().Status == BreakableWallStatus.WallStatus.Break)
        {
            Break();

            // Let the wall recover after RecoverTime
            Debug.Log(RecoverTime);
            Invoke("Recover", RecoverTime);
        }
    }

    // Break Status
    void Break()
    {
        
        GetComponent<PolygonCollider2D>().enabled = false;
        anim.SetBool("Recover", false);
        anim.SetBool("Break", true);
        GetComponent<BreakableWallStatus>().Status = BreakableWallStatus.WallStatus.Broken;
        Debug.Log("broken!");
        GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
    }

    // Recover Status
    void Recover()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<BreakableWallStatus>().Status = BreakableWallStatus.WallStatus.Recover;
        anim.SetBool("Break", false);
        anim.SetBool("Recover", true);
        GetComponent<BreakableWallStatus>().Status = BreakableWallStatus.WallStatus.Full;
        GetComponent<HealthSystem>().Heal(GetComponent<HealthSystem>().maxHealth);
        GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;

    }
}
