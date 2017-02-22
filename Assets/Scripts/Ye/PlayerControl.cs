using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public int moveSpeed = 3;
  //  Transform firePos;
    Transform Player;
  //  Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
 //       rb = GetComponent<Rigidbody2D>();
  //      firePos = transform.FindChild("firePos");
 //       newVector = firePos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.I))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

    }
}