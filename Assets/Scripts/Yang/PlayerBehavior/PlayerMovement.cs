// Script by Yang Liu
using UnityEngine;
using System.Collections;
using InControl;

public enum Direction {UP, DOWN, LEFT, RIGHT, UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT};

[RequireComponent (typeof(DeviceReceiver))]
public class PlayerMovement: MonoBehaviour {

    public float moveSpeed = 50f;

	[Range(0f, 1f)]
	[ReadOnly]public float moveSpeedFactor = 1f;

	public Direction initialFacing = Direction.DOWN;

	//public float frameDuration = 0.16f;

	public bool moveEnabled;
	public bool turnEnabled;

	public AudioClip moveSound;
	// default: delay 1 second and play again
	public float playSoundGap = 1f;
	public float volumeScale = 0.8f;
	bool playingSound = false;

	[HideInInspector] public Vector2 faceDirection;
	[HideInInspector] public float lastFrameSpeed = 0f;

    Vector2 moveVector;

	Vector2 _finalMoveVector;
	public Vector2 finalMoveVector{
		get{
			return _finalMoveVector;
		}
	}
//
    Rigidbody2D myRigidbody;
	InputDevice myInputDevice;
	AudioSource myAudioSource;

	FacingSpriteSwitcher _switcher;
	FacingSpriteSwitcher switcher{
		get{
			if (_switcher == null)
				_switcher = GetComponent<FacingSpriteSwitcher> ();
			return _switcher;
		}
	}

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
		myAudioSource = GetComponent<AudioSource> ();

		moveEnabled = true;
		turnEnabled = true;

		faceDirection = Direction2Vector (initialFacing);

		_finalMoveVector = Vector3.zero;
	}

	// Update is called once per frame
	void Update () {
		// get the axis values, construct a vector and normalize it
		// commented out Unity Input scripts
//		float horizontal = Input.GetAxis (horizontalAxisName);
//		float vertical = Input.GetAxis(verticalAxisName);
		myInputDevice = GetComponent<DeviceReceiver>().GetDevice();

		if(myInputDevice == null){
			return;
		}
		float horizontal = myInputDevice.LeftStickX;
		float vertical = myInputDevice.LeftStickY;

        moveVector = new Vector2(horizontal, vertical);

		if(moveVector.magnitude > 1f){
			moveVector.Normalize ();
		}


		if (moveVector.magnitude != 0f) {
			// update facedirection
			faceDirection = moveVector.normalized;

			// update facing
			if(switcher){
				switcher.facing = faceDirection;
			}

			// deal with sounds here
			if(!playingSound && moveVector.magnitude > 0.5f){
				playingSound = true;
				myAudioSource.PlayOneShot (moveSound, volumeScale);
				Invoke ("ResetSound", playSoundGap);
			}

		}


	}

    void FixedUpdate() {
		if(myInputDevice == null){
			return;
		}
		if (moveEnabled) {
			if (myRigidbody.bodyType != RigidbodyType2D.Static) {
				// myRigidbody.velocity = moveVector * moveSpeed * Time.deltaTime * 10f;
				_finalMoveVector = moveVector * moveSpeed * moveSpeedFactor * Time.deltaTime * 100f;
				myRigidbody.AddForce (_finalMoveVector);
			}
//			if(moveVector.magnitude != 0){
//				transform.up = new Vector2 (-moveVector.x, -moveVector.y);
//			}
		}

		

    }

	void ResetSound(){
		playingSound = false;
	}

	// translate a Direction enum to a normalized Vector3
	Direction Vector2Direction(Vector2 vec){
		if(vec.magnitude == 0f){
			Debug.Log ("Warning: vec.magnitude == 0f");
			return Direction.RIGHT;
		}

		Vector2 rightVector = new Vector2 (1f, 0f);

		float angle = Vector2.Angle (rightVector, vec);

		if(vec.y < 0f){
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
		else if (angle > 157.5f && angle <= 202.5f){
			return Direction.LEFT;
		}
		else if (angle > 202.5f && angle <= 247.5f){
			return Direction.DOWNLEFT;
		}
		else if (angle > 247.5f && angle <= 292.5f){
			return Direction.DOWN;
		}
		else if (angle > 292.5f && angle <= 357.5f){
			return Direction.DOWNRIGHT;
		}
		else{
			return Direction.RIGHT;
		}

	}

	Vector2 Direction2Vector(Direction dir){
		Vector2 dirvec = Vector2.zero;

		switch(dir){
		case Direction.DOWN:
			dirvec = Vector2.down;
			break;
		case Direction.DOWNLEFT:
			dirvec = Vector2.down + Vector2.left;
			break;
		case Direction.DOWNRIGHT:
			dirvec = Vector2.down + Vector2.right;
			break;
		case Direction.LEFT:
			dirvec = Vector2.left;
			break;
		case Direction.RIGHT:
			dirvec = Vector2.right;
			break;
		case Direction.UP:
			dirvec = Vector2.up;
			break;
		case Direction.UPLEFT:
			dirvec = Vector2.up + Vector2.left;
			break;
		case Direction.UPRIGHT:
			dirvec = Vector2.up + Vector2.right;
			break;
		default:
			dirvec = Vector2.down;
			break;
		}

		return dirvec.normalized;
	}
}

