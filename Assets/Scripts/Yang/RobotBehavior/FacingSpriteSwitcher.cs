using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class FacingSpriteSwitcher : MonoBehaviour {

	[ReadOnly]public Vector3 facing;

	public Vector3 facingPosition{
		set{
			if(value != transform.position){
				facing = (value - transform.position).normalized;
			}
		}
	}

	Direction faceDirection{
		get{
			if (newPerspective) {
				return Vector2NewDirection (facing);
			} else {
				return Vector2Direction (facing);
			}
		}
	}

	public Direction initialFacing = Direction.DOWN;

	public Sprite downSprite;
	public Sprite downLeftSprite;
	public Sprite downRightSprite;
	public Sprite leftSprite;
	public Sprite rightSprite;
	public Sprite upSprite;
	public Sprite upLeftSprite;
	public Sprite upRightSprite;


	public bool newPerspective = false;

	SpriteRenderer mySpriteRenderer;
	// Use this for initialization
	void Start () {
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		Sprite defaultSprite = mySpriteRenderer.sprite;

		downSprite = downSprite ?? defaultSprite;
		downLeftSprite = downLeftSprite ?? defaultSprite;
		downRightSprite = downRightSprite ?? defaultSprite;
		leftSprite = leftSprite ?? defaultSprite;
		rightSprite = rightSprite ?? defaultSprite;
		upSprite = upSprite ?? defaultSprite;
		upLeftSprite = upLeftSprite ?? defaultSprite;
		upRightSprite = upRightSprite ?? defaultSprite;

		UpdateSprite (initialFacing);
		facing = Direction2Vector (initialFacing);
	}
		
	// Update is called once per frame
	void Update () {
		if(facing.magnitude > 0f){
			UpdateSprite (faceDirection);
		}
	}

	void UpdateSprite(Direction faceDirection){
		switch (faceDirection) {
		case Direction.DOWN:
			if (mySpriteRenderer.sprite != downSprite)
				mySpriteRenderer.sprite = downSprite;
			break;
		case Direction.DOWNLEFT:
			if (mySpriteRenderer.sprite != downLeftSprite)
				mySpriteRenderer.sprite = downLeftSprite;
			break;
		case Direction.DOWNRIGHT:
			if (mySpriteRenderer.sprite != downRightSprite)
				mySpriteRenderer.sprite = downRightSprite;
			break;
		case Direction.LEFT:
			if (mySpriteRenderer.sprite != leftSprite)
				mySpriteRenderer.sprite = leftSprite;
			break;
		case Direction.RIGHT:
			if (mySpriteRenderer.sprite != rightSprite)
				mySpriteRenderer.sprite = rightSprite;
			break;
		case Direction.UP:
			if (mySpriteRenderer.sprite != upSprite)
				mySpriteRenderer.sprite = upSprite;
			break;
		case Direction.UPLEFT:
			if (mySpriteRenderer.sprite != upLeftSprite)
				mySpriteRenderer.sprite = upLeftSprite;
			break;
		case Direction.UPRIGHT:
			if (mySpriteRenderer.sprite != upRightSprite)
				mySpriteRenderer.sprite = upRightSprite;
			break;
		default:
			if (mySpriteRenderer.sprite != downSprite)
				mySpriteRenderer.sprite = downSprite;
			break;
		}
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

	Direction Vector2NewDirection(Vector2 vec){
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
		else if (angle > 165f && angle <= 195f){
			return Direction.LEFT;
		}
		else if (angle > 195f && angle <= 240f){
			return Direction.DOWNLEFT;
		}
		else if (angle > 240f && angle <= 300f){
			return Direction.DOWN;
		}
		else if (angle > 300f && angle <= 345f){
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
