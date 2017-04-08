// this script will set the wall to transparent
// to make the player visible when player 
// collides with the trigger


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; // Contains() method for array

[RequireComponent (typeof(SpriteRenderer))]
public class WallTransparency : MonoBehaviour {
	public float opacity = 0.5f;

	public string[] playerTags = { "AI", "Hacker" };

	public string playerBackLayerName = "BackChara";
	public string playerFrontLayerName = "ForeChara";

	public LayerMask moveableMask;

	public bool setTransparent = true;

	SpriteRenderer wallSprite;
	float defaultOpacity = 1f;

//	int stayObjCount = 0;
	Collider2D coll;

	// Use this for initialization
	void Start () {
		wallSprite = GetComponent<SpriteRenderer> ();
		defaultOpacity = wallSprite.color.a;

		Collider2D[] colls = GetComponents<Collider2D> ();
		foreach(Collider2D tempColl in colls){
			if(tempColl.isTrigger){
				coll = tempColl;
				break;
			}
		}


	}
	
//	// when the player enter the trigger
//	void OnTriggerEnter2D(Collider2D coll){
//		if(playerTags.Contains(coll.transform.tag)){
//			// increase the obj count
//			stayObjCount++;
//			if (stayObjCount == 1) {
//				// set the opacity
//				Color newColor = wallSprite.color;
//				newColor.a = opacity;
//				wallSprite.color = newColor;
////				// change the sorting layer from midground to foreground
//				wallSprite.sortingLayerName = playerFrontLayerName;
//			}
//		}
//	}
//
//	// when the player leaves the trigger
//	void OnTriggerExit2D(Collider2D coll){
//		if(playerTags.Contains(coll.transform.tag)){
//			// decrease the obj count
//			stayObjCount--;
//			if (stayObjCount == 0) {
//				// reset the opacity
//				Color newColor = wallSprite.color;
//				newColor.a = defaultOpacity;
//				wallSprite.color = newColor;
////				// change the sorting layer from midground to foreground
//				wallSprite.sortingLayerName = playerBackLayerName;
//			}
//
//		}
//	}

	void FixedUpdate(){
		if(coll == null){
			return;
		}
		//Debug.Log (coll);
		// Debug.Log (coll.IsTouchingLayers (moveableMask));
		if(coll.IsTouchingLayers(moveableMask)){
			SetAsTransparent ();
		}
		else{
			SetAsSolid ();
		}
	}


	void SetAsTransparent(){
		// set the opacity
		if (setTransparent) {
			Color newColor = wallSprite.color;
			newColor.a = opacity;
			wallSprite.color = newColor;
		}
		//				// change the sorting layer from midground to foreground
		wallSprite.sortingLayerName = playerFrontLayerName;
	}

	void SetAsSolid(){
		// reset the opacity
		if (setTransparent) {
			Color newColor = wallSprite.color;
			newColor.a = defaultOpacity;
			wallSprite.color = newColor;
		}
		//				// change the sorting layer from midground to foreground
		wallSprite.sortingLayerName = playerBackLayerName;
	}

}
