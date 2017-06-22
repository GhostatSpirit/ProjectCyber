using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

// enum Direction { UP, DOWN, LEFT, RIGHT, UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT };

[RequireComponent(typeof(DeviceReceiver))]

public class HackerMovementAnim: MonoBehaviour {

    bool moveBool;

	FacingSpriteSwitcher switcher;

    Animator anim;
    InputDevice myInputDevice;
    Vector2 moveVector = new Vector2(0, 0);
    AudioClip clip;

	Rigidbody2D body;
	PlayerMovement playerMove;

	//    Vector2 moveVector = new Vector2(0, 0);

	Vector3 lastFrameFacing;

	[HideInInspector]public float playSpeedFactor = 1f;
	public float playSpeed = 1f;


    // Use this for initialization
    void Start ()
    {
		switcher = GetComponent<FacingSpriteSwitcher> ();

        clip = GetComponents<AudioSource>()[0].clip;
        anim = GetComponent<Animator>();

		playerMove = GetComponent<PlayerMovement> ();

		HealthSystem hs = GetComponent<HealthSystem> ();
		if(hs){
			hs.OnObjectDead += SetDeadAsTrue;
			hs.OnObjectRevive += SetDeadAsFalse;
		}

	}

    

    // Update is called once per frame
    void Update()
    {
        moveBool = anim.GetBool("Moving");
        myInputDevice = GetComponent<DeviceReceiver>().GetDevice();

        if (myInputDevice == null)
        {
            return;
        }
        float horizontal = myInputDevice.LeftStickX;
        float vertical = myInputDevice.LeftStickY;

		moveVector = new Vector2(horizontal, vertical);


        // If magnitude > 1, normalize
        if (moveVector.magnitude > 1f)
        {
            moveVector.Normalize();
        }

        // Magnitude != 0, set moving 
		if ( moveVector.magnitude != 0f && playerMove.finalMoveVector.magnitude != 0f)
        {
            // GetComponent<FacingSpriteSwitcher>().enabled = false;
            // Set moving
            //anim.SetBool("Moving", true);
			switcher.enabled = false;
			anim.SetFloat ("moveSpeed", moveVector.magnitude * playSpeed);


			lastFrameFacing = moveVector.normalized;

            Direction dir = Vector2Direction(moveVector);
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
		else
        {
			// target is not moving

            // GetComponent<FacingSpriteSwitcher>().enabled = true;
            // GetComponent<FacingSpriteSwitcher>().facing = new Vector3(anim.GetFloat("SpeedX"),anim.GetFloat("SpeedY"),0);
            anim.SetBool("Moving", false);
			anim.SetFloat ("moveSpeed", 0f);

//			switcher.enabled = true;
//			switcher.facing = lastFrameFacing;
//			switcher.UpdateSprite ();
        }
    }

	void SetDeadAsTrue(Transform trans){
		anim.SetBool ("dead", true);
	}

	void SetDeadAsFalse(Transform trans){
		anim.SetBool ("dead", false);
	}


	// translate a Direction enum to a normalized Vector3

	Direction Vector2Direction(Vector2 vec)
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
		if (angle > 22.5f && angle <= 67.5f)
		{
			return Direction.UPRIGHT;// up
		}
		// play "going up" animation if angle between 67.5° and 112.5°
		else if (angle > 67.5f && angle <= 112.5f)
		{
			return Direction.UP;// left
		}
		// play "going upleft" animation if angle between 225° and 315°
		else if (angle > 112.5f && angle <= 157.5f)
		{
			return Direction.UPLEFT;// down
		}
		else if (angle > 157.5f && angle <= 202.5f)
		{
			return Direction.LEFT;
		}
		else if (angle > 202.5f && angle <= 247.5f)
		{
			return Direction.DOWNLEFT;
		}
		else if (angle > 247.5f && angle <= 292.5f)
		{
			return Direction.DOWN;
		}
		else if (angle > 292.5f && angle <= 357.5f)
		{
			return Direction.DOWNRIGHT;
		}
		else
		{
			return Direction.RIGHT;
		}

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

}
