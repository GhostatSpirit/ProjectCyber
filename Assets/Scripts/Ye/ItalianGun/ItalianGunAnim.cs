using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItalianGunAnim : MonoBehaviour {

    // public float ControlTime;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Enemy", true);
        anim.SetBool("Controlled", false);

    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<ItalianGunStatus>().Status == ItalianGunStatus.GunStatus.Enemy && Input.GetKey(KeyCode.Q))
        {
            Control();
        }
        if (GetComponent<ItalianGunStatus>().Status == ItalianGunStatus.GunStatus.Controlled && Input.GetKey(KeyCode.W))
        {
            Enemy();
        }
    }

    // Control Status
    void Control()
    {
        anim.SetBool("Enemy", false);
        anim.SetBool("Controlled", true);
        GetComponent<ItalianGunStatus>().Status = ItalianGunStatus.GunStatus.Controlled;

    }

    // Recover Status
    void Enemy()
    {
        anim.SetBool("Controlled", false);
        anim.SetBool("Enemy", true);
        GetComponent<ItalianGunStatus>().Status = ItalianGunStatus.GunStatus.Enemy;
    }
}
