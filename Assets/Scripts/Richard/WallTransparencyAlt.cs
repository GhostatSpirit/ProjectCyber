// this script will set the wall to transparent
// to make the player visible when player 
// collides with the trigger


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; // Contains() method for array

[RequireComponent (typeof(MeshRenderer))]
public class WallTransparencyAlt : MonoBehaviour {
	public float opacity = 0.45f;

	public string[] playerTags = { "AI", "Hacker" };

//	public string wallBackLayerName = "BackWall";
//	public string wallFrontLayerName = "ForeWall";

	public LayerMask moveableMask;

	public bool setTransparent = true;

    MeshRenderer[] wallMesh;
    List<float> defaultOpacity = new List<float>();


//	int stayObjCount = 0;
	Collider2D coll;

	// Use this for initialization
	void Start () {
        wallMesh = transform.parent.transform.parent.GetComponentsInChildren<MeshRenderer> ();
        foreach(MeshRenderer child in wallMesh)
        {
            defaultOpacity.Add(child.material.color.a);
        }

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
		// Debug.Log (coll);
		// Debug.Log (coll.IsTouchingLayers (moveableMask));
		if(coll.IsTouchingLayers(moveableMask)){
            // Debug.Log("Touching!");
			SetAsTransparent ();
		}
		else{
            //Debug.Log("Not Touching!");
            SetAsSolid ();
		}
	}


	void SetAsTransparent(){
		// set the opacity
		if (setTransparent) {
            foreach(MeshRenderer mesh in wallMesh)
            {
                Color newColor = mesh.material.color;
                newColor.a = opacity;
                mesh.material.color = newColor;
            }
            //Color newColor = wallMesh.mater.color;
			//newColor.a = opacity;
			//wallSprite.color = newColor;
		}
		//				// change the sorting layer from midground to foreground
		// wallSprite.sortingLayerName = playerFrontLayerName;
	}

	void SetAsSolid(){
        // reset the opacity
        if (setTransparent)
        {
            int i = 0;
            foreach (MeshRenderer mesh in wallMesh)
            {
                Color newColor = mesh.material.color;
                newColor.a = defaultOpacity[i];
                mesh.material.color = newColor;
                i++;
            }
        }
            //if (setTransparent) {
            //	Color newColor = wallSprite.color;
            //	newColor.a = defaultOpacity;
            //	wallSprite.color = newColor;
            //}
            //				// change the sorting layer from midground to foreground
            // wallSprite.sortingLayerName = playerBackLayerName;
        }

}
