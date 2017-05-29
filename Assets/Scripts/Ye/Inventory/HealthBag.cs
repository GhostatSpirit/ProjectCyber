using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBag : MonoBehaviour {

    GameObject player;
    // HealthSystem HBHS;
    HealthSystem PlayerHS;
	ObjectIdentity PlayerOI;
    public float HealRatio = 0.2f;


    // ye added

    AudioSource audioS;
    // Collision2D collision;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// heal
		PlayerHS = collision.gameObject.GetComponent<HealthSystem>();
		PlayerOI = collision.transform.GetComponent<ObjectIdentity>();


		if (!PlayerOI)
		{
			return;
		}

		if (PlayerOI.objType == ObjectType.AI || PlayerOI.objType == ObjectType.Hacker)
		{
			if (PlayerHS.GetHealth() == PlayerHS.maxHealth)
			{
				return ;
			}

			if (PlayerHS) {

				// ye added heal sound
				audioS = GetComponentInParent<AudioSource> ();
				if (audioS) {
					audioS.PlayOneShot (audioS.clip);
				}

				PlayerHS.Heal (HealRatio * PlayerHS.maxHealth);
			}

			Destroy(gameObject);
		}
		/*
        PlayerHS.Heal(HealRatio * PlayerHS.maxHealth);
        */

		// destory

	}

    /*
    // when collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // heal
        PlayerHS = collision.gameObject.GetComponent<HealthSystem>();
		PlayerOI = collision.transform.GetComponent<ObjectIdentity> ();


		if(!PlayerOI){
			return;
		}

		if (PlayerOI.objType == ObjectType.AI || PlayerOI.objType == ObjectType.Hacker) {
			if(PlayerHS.GetHealth() == PlayerHS.maxHealth){
				return;
			}

			if(PlayerHS)

                // ye added heal sound
                audioS.PlayOneShot(audioS.clip);

                PlayerHS.Heal (HealRatio * PlayerHS.maxHealth);
             

            //			PlayerInteract interact = collision.transform.GetComponent<PlayerInteract> ();
            //			Transform otherPlayer = interact.otherPlayer;
            //			HealthSystem otherhs = otherPlayer.GetComponent<HealthSystem> ();
            //			if(otherhs){
            //				otherhs.Heal (HealRatio * PlayerHS.maxHealth);
            //			}
            // gameObject.SetActive(false);

            Destroy(gameObject);
		}

        // destory

    }
    */
    // when collision
    
}
