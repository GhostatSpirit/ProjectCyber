using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

public class LineCut : MonoBehaviour {

	public bool couldCut = false;

	// Transform means the cutter's transform, i.e. this.transform
	public event Action<Transform> OnLineCut;

	void LineCutBehavior(Collider2D other){
		ControlStatus cs = other.gameObject.GetComponentInParent<ControlStatus> ();
		if(cs == null){
			return;
		}
		bool controlledByBoss = 
			cs.controller == Controller.Boss;

		if (other.gameObject.tag == "EnemyLine" && controlledByBoss && couldCut ) // && other.gameObject.GetComponent<LineRenderer>().startColor != Color.clear && other.gameObject.GetComponent<LineRenderer>().endColor != Color.clear)
		{
			// reaching an enemyline, cut it
			if(OnLineCut != null){
				OnLineCut (this.transform);
			}
			GameObject TargetLine = other.gameObject;
			ControlStatus TargetCS = TargetLine.GetComponentInParent<ControlStatus>();
			TargetCS.controller = Controller.None;

		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		LineCutBehavior (other);
    }

	void OnTriggerStay2D(Collider2D other)
	{
		LineCutBehavior (other);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		LineCutBehavior (other);
	}


	// this methods would return the estimated hit point (not accurate)
	// given the colliding object's transform
	Vector2 GetHitPoint(Transform other){
		Rigidbody2D AIrb;
		Vector2 VAI;
		Vector2 AIPos;
		Vector2 AIPos1;
		Vector2 LineStart;
		Vector2 LineEnd;
		Vector2 Hit;


		AIrb = GetComponent<Rigidbody2D>();
		LineRenderer otherLR = other.GetComponent<LineRenderer> ();

		if(AIrb == null || otherLR == null){
			Debug.Log ("GetHitPoint: missing rigidbody2D or LineRenderer");
			return Vector2.zero;
		}

		LineStart = otherLR.GetPosition(0);
		LineEnd = otherLR.GetPosition(1);
	

		AIPos = AIrb.position;
		VAI = AIrb.velocity;
		AIPos1 = new Vector2 ( VAI.x+AIPos.x , VAI.y + AIPos.y);
		float D = (AIPos1.x - AIPos.x) * (LineEnd.y - LineStart.y)-(LineEnd.x - LineStart.x)*(AIPos1.y - AIPos.y);
		float b1 = (AIPos1.y - AIPos.y) * AIPos.x + (AIPos.x - AIPos1.x) * AIPos.y;
		float b2 = (LineEnd.y - LineStart.y) * LineStart.x + (LineStart.x - LineEnd.x) * LineStart.y; ;
		float D1 = b2 * (AIPos1.x - AIPos.x) - b1 * (LineEnd.x - LineStart.x);
		float D2 = b2 * (AIPos1.y - AIPos.y) - b1 * (LineEnd.y - LineStart.y);
		float X0 = D1 / D;
		float Y0 = D2 / D;
		Hit = new Vector2(X0, Y0);

		return Hit;
	}
}
