using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public Vector2 speed = new Vector2(36, 36);
    private Vector2 movement;
    public GameObject target;
    private Rigidbody2D rb;
    private RaycastHit2D rch;
    private LayerMask lm;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = (target.transform.position - transform.position).normalized;
        Vector2 start_pos = transform.position + new Vector3(speed.normalized.x * 5, speed.normalized.y * 5, 0);
        Vector2 end_pos = transform.position + new Vector3(speed.normalized.x * 14, speed.normalized.y * 14, 0);
        Debug.DrawLine(start_pos, end_pos, Color.red);
        movement = new Vector2(speed.x * 28f, speed.y * 28f);

        rch = Physics2D.Linecast(start_pos, end_pos, 1);
        if (rch.collider == null)
            rb.velocity = movement;
        else
        {
            Debug.Log("Hit");
        }


    }
}
