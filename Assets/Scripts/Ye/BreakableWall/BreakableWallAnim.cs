using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallAnim: MonoBehaviour
{

    public float RecoverTime;
	public bool turnOffSFPolygon = false;

	HealthSystem hs;
    Animator anim;
	SFPolygon sfp;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Break",false);
        anim.SetBool("Recover",false);
        
		hs = GetComponent<HealthSystem> ();
		sfp = GetComponent<SFPolygon> ();

		hs.OnObjectDead += Break;
		hs.OnObjectDead += StartRecover;
		if(turnOffSFPolygon){
			hs.OnObjectDead += TurnOffSFPolygon;
			hs.OnObjectRevive += TurnOnSFPolygon;
		}
	}

    // Break Status
	void Break (Transform wallTrans)
    {
        
        GetComponent<PolygonCollider2D>().enabled = false;
        anim.SetBool("Recover", false);
        anim.SetBool("Break", true);
        GetComponent<BreakableWallStatus>().Status = BreakableWallStatus.WallStatus.Broken;
//        Debug.Log("broken!");
        GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
    }

	void TurnOffSFPolygon (Transform wallTrans){
		sfp.enabled = false;
	}

	void TurnOnSFPolygon (Transform wallTrans){
		sfp.enabled = true;
	}

	Coroutine recoverCoroutine;
	void StartRecover (Transform wallTrans){
		if(recoverCoroutine == null){
			recoverCoroutine = StartCoroutine (RecoverIE (RecoverTime));
		}
	}

    // Recover Status
	IEnumerator RecoverIE(float recoverTime)
    {
		yield return new WaitForSeconds (recoverTime);

        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<BreakableWallStatus>().Status = BreakableWallStatus.WallStatus.Recover;
        anim.SetBool("Break", false);
        anim.SetBool("Recover", true);
        GetComponent<BreakableWallStatus>().Status = BreakableWallStatus.WallStatus.Full;
		GetComponent<HealthSystem> ().Revive (1f);
        GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;

		recoverCoroutine = null;
		yield break;
    }

	void Update(){
//		if(recoverCoroutine != null){
//			Debug.Log (recoverCoroutine);
//		}
	}
}
