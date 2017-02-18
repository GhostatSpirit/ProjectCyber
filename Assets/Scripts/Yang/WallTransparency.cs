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

	SpriteRenderer wallSprite;
	float defaultOpacity = 1f;

	int stayObjCount = 0;


	// Use this for initialization
	void Start () {
		wallSprite = GetComponent<SpriteRenderer> ();
		defaultOpacity = wallSprite.color.a;

	}
	
	// when the player enter the trigger
	void OnTriggerEnter2D(Collider2D coll){
		if(playerTags.Contains(coll.transform.tag)){
			// increase the obj count
			stayObjCount++;
			if (stayObjCount == 1) {
				// set the opacity
				Color newColor = wallSprite.color;
				newColor.a = opacity;
				wallSprite.color = newColor;
//				// change the sorting layer from midground to foreground
//				wallSprite.sortingOrder = SortingLayer.GetLayerValueFromName ("Foreground");
			}
		}
	}

	// when the player leaves the trigger
	void OnTriggerExit2D(Collider2D coll){
		if(playerTags.Contains(coll.transform.tag)){
			// decrease the obj count
			stayObjCount--;
			if (stayObjCount == 0) {
				// reset the opacity
				Color newColor = wallSprite.color;
				newColor.a = defaultOpacity;
				wallSprite.color = newColor;
//				// change the sorting layer from midground to foreground
//				wallSprite.sortingOrder = SortingLayer.GetLayerValueFromName ("Foreground");
			}

		}
	}
}
