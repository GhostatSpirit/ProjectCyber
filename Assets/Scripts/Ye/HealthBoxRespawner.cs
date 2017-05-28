using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxRespawner : MonoBehaviour {

    public float respawnInterval = 5f;
    Collider2D areaTrigger;
    public GameObject healthBox;
    public LayerMask movableMask;

    // bool hasOldHealthBox = false;
    // float oldFovRadius = 1f;
    // bool oldIgnoreVisionBlock = false;

    // count healthbox
    int healthBoxCount
    {
        get
        {
            int tempCount = 0;
            foreach (Transform child in transform)
            {
                ObjectIdentity oi = child.GetComponent<ObjectIdentity>();
                if (oi && oi.objType == ObjectType.HealthBox )
                {
                    tempCount++;
                    /*
                    FieldOfView fov = child.GetComponent<FieldOfView>();
                    if (fov)
                    {
                        hasOldHealthBox = true;

                    }
                    */
                }
            }
            return tempCount;
        }
    }

    // Use this for initialization
    void Start () {
        // innitialize areaTrigger
        areaTrigger = GetComponent<Collider2D>();
	}

    void OnEnable()
    {
        if (healthBoxCount == 0 && healthBox && !ObjectNearby())
        {
            RespawnHealthBox();
        }
    }

    Coroutine respawnCoroutine;
    
    // Update is called once per frame
    void Update () {
        if (healthBoxCount == 0 && respawnCoroutine == null)
        {
            // start the respawnCoroutine
            respawnCoroutine = StartCoroutine(RespawnHealthBoxIE(respawnInterval));
        }
    }

    // nearby check
    bool ObjectNearby()
    {
        if (!areaTrigger)
        {
            areaTrigger = GetComponent<Collider2D>();
        }
        if (!areaTrigger)
        {
            return false;
        }
        else
        {
            return areaTrigger.IsTouchingLayers(movableMask);
        }
    }

    IEnumerator RespawnHealthBoxIE(float _respawnInterval)
    {
        yield return new WaitForSeconds(_respawnInterval);
        yield return new WaitUntil(
            () => {
                return !ObjectNearby();
            }
        );
        RespawnHealthBox();
        respawnCoroutine = null;
    }

    void RespawnHealthBox()
    {
        GameObject HealthBoxGO =
            Instantiate(healthBox, transform.position, transform.rotation);
        HealthBoxGO.transform.parent = transform;
        HealthBoxGO.transform.localScale = new Vector3(1f, 1f, 1f);
        /*
        if (hasOldHealthBox)
        {
            FieldOfView fov = HealthBoxGO.GetComponent<FieldOfView>();
            if (fov)
            {
                fov.radius = oldFovRadius;
                fov.ignoreVisionBlock = oldIgnoreVisionBlock;
            }
        }
        */
    }
}
