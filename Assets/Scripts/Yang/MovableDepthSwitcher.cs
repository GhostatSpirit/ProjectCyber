using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableDepthSwitcher : MonoBehaviour {
	SpriteRenderer movableSprite;

	public string wallBackLayerName = "BackWall";
	public string wallFrontLayerName = "ForeWall";

	public LayerMask wallMask;

	Collider2D coll;
	Collider2D[] colliders;

	ContactFilter2D filter;
	// Use this for initialization
	void Start () {
		movableSprite = GetComponentInParent<SpriteRenderer> ();
		coll = GetComponent<Collider2D> ();

		colliders = new Collider2D[1];
		filter = new ContactFilter2D ();
		// get trigger hits
		filter.useTriggers = true;
		filter.useLayerMask = true;
		filter.useDepth = false;
		filter.useNormalAngle = false;
		filter.SetLayerMask (wallMask);
	}

	// Update is called once per frame
	void FixedUpdate(){
        int collCount = coll.OverlapCollider (filter, colliders);
		bool hitWallTrigger = false;
//		Debug.Log (collCount);
		if(collCount == 0){
			SetForeWall ();
			return;
		}
		foreach(Collider2D targetColl in colliders){
			if(!targetColl){
				continue;
			}
//			Debug.Log (targetColl.transform);
			if (targetColl.GetComponent<WallTransparencyAlt> ()) {
				// hit a wall
				hitWallTrigger = true;
			} else {
				ObjectIdentity oi = targetColl.GetComponent<ObjectIdentity> ();
				if(oi && oi.objType == ObjectType.DepthField){
					hitWallTrigger = true;
				}
			}

		}
		if(hitWallTrigger){
			SetBackWall ();
		}else{
			SetForeWall ();
		}


	}

	void SetBackWall(){
		movableSprite.sortingLayerName = wallBackLayerName;
	}

	void SetForeWall(){
		movableSprite.sortingLayerName = wallFrontLayerName;
	}
}
