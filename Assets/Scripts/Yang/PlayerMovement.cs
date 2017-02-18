// Script by Yang Liu
using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent (typeof(DeviceReceiver))]
public class PlayerMovement: MonoBehaviour {

    public float moveSpeed = 50f;

	// commented out Unity Input scripts
	// public string horizontalAxisName = "Horizontal";
	// public string verticalAxisName = "Vertical";

	public enum Direction {UP, DOWN, LEFT, RIGHT, UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT};

	public Direction initialFacing = Direction.DOWN;
	//public Transform playerFeet;

	// different character sprites for 8 directions
	public Sprite downSprite;
	public Sprite downLeftSprite;
	public Sprite downRightSprite;
	public Sprite leftSprite;
	public Sprite rightSprite;
	public Sprite upSprite;
	public Sprite upLeftSprite;
	public Sprite upRightSprite;

	public float frameDuration = 0.16f;

	public bool moveEnabled;
	public bool turnEnabled;

	public AudioClip moveSound;
	// default: delay 1 second and play again
	public float playSoundGap = 1f;
	public float volumeScale = 0.8f;
	bool playingSound = false;

    Vector2 moveVector;
    Rigidbody2D myRigidbody;
	SpriteRenderer mySpriteRenderer;
	InputDevice myInputDevice;


	AudioSource myAudioSource;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		myAudioSource = GetComponent<AudioSource> ();

		moveEnabled = true;
		turnEnabled = true;

		// check if sprites with different facings is null
		// if is null, replace it with the default sprite
		Sprite defaultSprite = mySpriteRenderer.sprite;

		downSprite = downSprite ?? defaultSprite;
		downLeftSprite = downLeftSprite ?? defaultSprite;
		downRightSprite = downRightSprite ?? defaultSprite;
		leftSprite = leftSprite ?? defaultSprite;
		rightSprite = rightSprite ?? defaultSprite;
		upSprite = upSprite ?? defaultSprite;
		upLeftSprite = upLeftSprite ?? defaultSprite;
		upRightSprite = upRightSprite ?? defaultSprite;


		switch(initialFacing){
		case Direction.DOWN:
			mySpriteRenderer.sprite = downSprite;
			break;
		case Direction.DOWNLEFT:
			mySpriteRenderer.sprite = downLeftSprite;
			break;
		case Direction.DOWNRIGHT:
			mySpriteRenderer.sprite = downRightSprite;
			break;
		case Direction.LEFT:
			mySpriteRenderer.sprite = leftSprite;
			break;
		case Direction.RIGHT:
			mySpriteRenderer.sprite = rightSprite;
			break;
		case Direction.UP:
			mySpriteRenderer.sprite = upSprite;
			break;
		case Direction.UPLEFT:
			mySpriteRenderer.sprite = upLeftSprite;
			break;
		case Direction.UPRIGHT:
			mySpriteRenderer.sprite = upRightSprite;
			break;
		default:
			mySpriteRenderer.sprite = downSprite;
			break;
		}
		
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
			// change sprite according to moveVector
			Direction currentDir = Vector2Direction (moveVector);

			switch(currentDir){
			case Direction.DOWN:
				mySpriteRenderer.sprite = downSprite;
				break;
			case Direction.DOWNLEFT:
				mySpriteRenderer.sprite = downLeftSprite;
				break;
			case Direction.DOWNRIGHT:
				mySpriteRenderer.sprite = downRightSprite;
				break;
			case Direction.LEFT:
				mySpriteRenderer.sprite = leftSprite;
				break;
			case Direction.RIGHT:
				mySpriteRenderer.sprite = rightSprite;
				break;
			case Direction.UP:
				mySpriteRenderer.sprite = upSprite;
				break;
			case Direction.UPLEFT:
				mySpriteRenderer.sprite = upLeftSprite;
				break;
			case Direction.UPRIGHT:
				mySpriteRenderer.sprite = upRightSprite;
				break;
			default:
				mySpriteRenderer.sprite = downSprite;
				break;
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
			myRigidbody.velocity = moveVector * moveSpeed * Time.deltaTime * 10f;
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



		//Debug.Log (angle);
	}
}

