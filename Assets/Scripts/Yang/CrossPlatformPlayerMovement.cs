// Script by Yang Liu
using UnityEngine;
using System.Collections;

// Using the standard asset joystick:


using UnityStandardAssets.CrossPlatformInput;

public class CrossPlatformPlayerMovement: MonoBehaviour {

    public float moveSpeed = 50f;

	public bool crossPlatform = false;
	public string horizontalAxisName = "Horizontal";
	public string verticalAxisName = "Vertical";

	public enum Direction {NORTH, SOUTH, EAST, WEST};

	public Direction initialFacing = Direction.SOUTH;
	//public Transform playerFeet;

    Vector2 moveVector;
    Rigidbody2D myRigidbody;

	Vector2 lastDirection;
	Vector3 lastPosition;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();

		lastDirection = DirectionToVector(initialFacing);

		lastPosition = myRigidbody.position;

	}
	
	// Update is called once per frame
	void Update () {
		// get the axis values, construct a vector and normalize it
		float horizontal = 0f, vertical = 0f;
		if (crossPlatform) {
			horizontal = CrossPlatformInputManager.GetAxis (horizontalAxisName) * moveSpeed;
			vertical = CrossPlatformInputManager.GetAxis (verticalAxisName) * moveSpeed;
		}
		else{
			horizontal = Input.GetAxis (horizontalAxisName) * moveSpeed;
			vertical = Input.GetAxis (horizontalAxisName) * moveSpeed;
		}
        moveVector = new Vector2(horizontal, vertical);
			
		if(moveVector.magnitude > 1f){
			moveVector.Normalize ();
		}

	}

    void FixedUpdate() {
		
		myRigidbody.velocity = moveVector * moveSpeed * Time.deltaTime * 10f;

		if(myRigidbody.velocity.magnitude < 0.01f){
			myRigidbody.velocity = Vector3.zero;
			myRigidbody.position = lastPosition;
		}

		//Debug.Log (myRigidbody.velocity);

		if(moveVector.magnitude != 0f){
			transform.up = moveVector.normalized;
			lastDirection = moveVector.normalized;
		}else{
			transform.up = lastDirection;
		}

		lastPosition = myRigidbody.position;

    }

	// translate a Direction enum to a normalized Vector3
	Vector3 DirectionToVector(Direction dir){
		if(dir == Direction.NORTH){
			return new Vector3 (0f, 1f, 0f);
		}else if(dir == Direction.SOUTH){
			return new Vector3 (0f, -1f, 0f);
		}else if(dir == Direction.EAST){
			return new Vector3 (1f, 0f, 0f);
		}else if (dir == Direction.WEST){
			return new Vector3 (-1f, 0f, 0f);
		}else{
			return new Vector3 (0f, 0f, 0f);
		}
	}
}
