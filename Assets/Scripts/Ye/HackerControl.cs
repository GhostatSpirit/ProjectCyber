using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerControl : MonoBehaviour
{

    public int moveSpeed = 3;
    public Vector3 newVector;
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

    }
}