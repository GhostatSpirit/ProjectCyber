using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class FacingSpriteSwitcher : MonoBehaviour {

	[ReadOnly]public Vector3 facing;

	Direction faceDirection{
		get{
			return Vector2Direction (facing);
		}
	}

	public Sprite downSprite;
	public Sprite downLeftSprite;
	public Sprite downRightSprite;
	public Sprite leftSprite;
	public Sprite rightSprite;
	public Sprite upSprite;
	public Sprite upLeftSprite;
	public Sprite upRightSprite;


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
	}
	
	// Update is called once per frame
	void Update () {
		if(facing.magnitude > 0f){
			switch(faceDirection){
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
