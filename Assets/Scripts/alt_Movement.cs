using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alt_Movement : MonoBehaviour {
    public Vector2 speed = new Vector2(1, 1);
    private Vector2 movement;
    // Use this for initialization
    void Start () {

}
	
	// Update is called once per frame
	void Update () {
        float inputX = Input.GetAxis("2nd_Horizontal");
        float inputY = Input.GetAxis("2nd_Vertical");
        movement = new Vector2(speed.x * inputX * 36f, speed.y * inputY * 36f);
        GetComponent<Rigidbody2D>().velocity = movement;
    }
}
