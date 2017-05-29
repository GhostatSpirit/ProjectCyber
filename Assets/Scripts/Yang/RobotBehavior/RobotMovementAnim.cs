using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementAnim : MonoBehaviour {

    bool moveBool;
    Animator anim;
//    Vector2 moveVector = new Vector2(0, 0);

	Vector3 lastFramePos;

	public float speedFactor = 1f;
	public float minMoveSpeed = 0.1f;

	[ReadOnly] public float moveSpeed = 0f;

//	FacingSpriteSwitcher switcher;

    // Use this for initialization
    void Start () {
        anim = GetComponentInParent<Animator>();
		lastFramePos = transform.position;

		//switcher = GetComponent<FacingSpriteSwitcher> ();
    }

    // updated version
    Direction Vector2NewDirection(Vector2 vec)
    {
        if (vec.magnitude == 0f)
        {
            Debug.Log("Warning: vec.magnitude == 0f");
            return Direction.RIGHT;
        }

        Vector2 rightVector = new Vector2(1f, 0f);

        float angle = Vector2.Angle(rightVector, vec);

        if (vec.y < 0f)
        {
            angle = 360f - angle;
        }
        // play "going upright" animation if angle between 22.5° and 67.5°
        if (angle > 15f && angle <= 60f)
        {
            return Direction.UPRIGHT;
        }
        // play "going up" animation if angle between 67.5° and 112.5°
        else if (angle > 60f && angle <= 120f)
        {
            return Direction.UP;
        }
        // play "going upleft" animation if angle between 225° and 315°
        else if (angle > 120f && angle <= 165f)
        {
            return Direction.UPLEFT;// down
        }
        else if (angle > 165f && angle <= 195f)
        {
            return Direction.LEFT;
        }
        else if (angle > 195f && angle <= 240f)
        {
            return Direction.DOWNLEFT;
        }
        else if (angle > 240f && angle <= 300f)
        {
            return Direction.DOWN;
        }
        else if (angle > 300f && angle <= 345f)
        {
            return Direction.DOWNRIGHT;
        }
        else
        {
            return Direction.RIGHT;
        }

    }

    // Update is called once per frame
    void Update()
    {

		moveSpeed = Vector3.Distance (lastFramePos, transform.position) / Time.deltaTime * speedFactor;

		// Debug.Log (speed);

        // float horizontal = myInputDevice.LeftStickX;
        // float vertical = myInputDevice.LeftStickY;

        // moveVector = new Vector2(horizontal, vertical);


        // If magnitude > 1, normalize
        /*
        if (moveVector.magnitude > 1f)
        {
            moveVector.Normalize();
        }
        */
        // Magnitude != 0, set moving 
		if (moveSpeed >= 0f)
        {
            // GetComponent<FacingSpriteSwitcher>().enabled = false;
            // Set moving
            anim.SetBool("Moving", true);
			anim.SetFloat ("moveSpeed", moveSpeed);

            Direction dir = Vector2NewDirection ( GetComponent<FacingSpriteSwitcher>().facing );
            switch (dir)
            {
                case Direction.DOWN:
                    anim.SetFloat("SpeedX", 0);
                    anim.SetFloat("SpeedY", -1);
                    break;
                case Direction.UP:
                    anim.SetFloat("SpeedX", 0);
                    anim.SetFloat("SpeedY", 1);
                    break;
                case Direction.LEFT:
                    anim.SetFloat("SpeedX", -1);
                    anim.SetFloat("SpeedY", 0);
                    break;
                case Direction.RIGHT:
                    anim.SetFloat("SpeedX", 1);
                    anim.SetFloat("SpeedY", 0);
                    break;
                case Direction.DOWNRIGHT:
                    anim.SetFloat("SpeedX", 1);
                    anim.SetFloat("SpeedY", -1);
                    break;
                case Direction.DOWNLEFT:
                    anim.SetFloat("SpeedX", -1);
                    anim.SetFloat("SpeedY", -1);
                    break;
                case Direction.UPLEFT:
                    anim.SetFloat("SpeedX", -1);
                    anim.SetFloat("SpeedY", 1);
                    break;
                case Direction.UPRIGHT:
                    anim.SetFloat("SpeedX", 1);
                    anim.SetFloat("SpeedY", 1);
                    break;
            }

		} 

		lastFramePos = transform.position;

        /*
        if (moveVector.magnitude == 0)
        {
            // GetComponent<FacingSpriteSwitcher>().enabled = true;
            // GetComponent<FacingSpriteSwitcher>().facing = new Vector3(anim.GetFloat("SpeedX"),anim.GetFloat("SpeedY"),0);
            anim.SetBool("Moving", false);
        }
        */
    }
}
