/* Written by Yang Liu
 * This script:
 * 1. create a new GameObject called "ControlLine" on Start(),
 * 2. update the LineRenderer and EdgeCollider on "ControlLine" each frame
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ControlStatus))]
public class LineUpdate : MonoBehaviour {
	
	GameObject ControlLine;

	public Material EnemyLineMaterial;
	public Material PlayerLineMateial;

	public string sortingLayerName = "ControlLine";
	public int orderInLayer = 0;

	public float lineWidth = 0.15f;

	public bool drawOnEnable = true;

	// private field for LineRenderer and EdgeCollider
	LineRenderer lr;
	EdgeCollider2D LineEC;

	// a read-only Boss property that reads Boss from ControlStatus
	GameObject _boss;
	GameObject Boss{
		get{
			_boss = GetComponent<ControlStatus> ().Boss.gameObject;
			if(_boss){
				// try to find control line node
				ControlLineNode node = _boss.GetComponentInChildren<ControlLineNode> ();
				if (node)
					_boss = node.gameObject;
			}

			return _boss;
		}
	}

	// a read-only Boss property that reads Hacker from ControlStatus
	GameObject _hacker;
	GameObject Hacker{
		get{
			_hacker = GetComponent<ControlStatus> ().Hacker.gameObject;
			if(_hacker){
				// try to find control line node
				ControlLineNode node = _hacker.GetComponentInChildren<ControlLineNode> ();
				if (node)
					_hacker = node.gameObject;
			}

			return _hacker;
		}
	}

	Transform _self;
	Transform self{
		get{
			if(_self == null){
				_self = transform;
				ControlLineNode node = GetComponentInChildren<ControlLineNode> ();
				if (node)
					_self = node.transform;
			}
			return _self;
		}
	}

	// a read-only controller property that reads from ControlStatus
	Controller controller{
		get{
			return GetComponent<ControlStatus> ().controller;
		}
	}

	// Use this for initialization
	void Start () {

		ControlLine = new GameObject();
		ControlLine.transform.position = self.position;

		// create and initialize LineRenderer
		lr = ControlLine.AddComponent<LineRenderer>();
		lr.sortingOrder = orderInLayer;
		lr.sortingLayerID = SortingLayer.NameToID (sortingLayerName);

		lr.material = EnemyLineMaterial;
		Color color = Color.white;
		lr.startWidth = lineWidth;
		lr.endWidth = lineWidth;
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, self.position);
		lr.SetPosition(1, Boss.transform.position);

		// create and initialize EdgeCollider2D
		ControlLine.AddComponent<EdgeCollider2D>();
		LineEC = ControlLine.GetComponent<EdgeCollider2D>();
		LineEC.isTrigger = true;
		Vector2[] temparray = new Vector2[2];
		temparray[0] = new Vector2(0, 0);
		temparray[1] = new Vector2(Boss.transform.position.x- self.position.x, Boss.transform.position.y - self.position.y);
		LineEC.points = temparray;

		// set the objectIdentity to line
		ObjectIdentity oi = ControlLine.AddComponent<ObjectIdentity> ();
		oi.objType = ObjectType.Line;

		// initialize the tag as "EnemyLine"
		ControlLine.tag = "EnemyLine";
		// set the layer as "EnemyLine"
		// ControlLine.layer = LayerMaskToLayerNum(LayerMask.GetMask ("EnemyLine"));

		// rename this new game object
		ControlLine.name = "ControlLine";

		ControlLine.transform.SetParent(this.transform);
		// Debug.Log(gameObject.name + ControlLine.transform.position);

		// modify the actions
		ControlStatus cs = GetComponent<ControlStatus> ();
		if(cs){
			cs.OnCutByEnemy += DisableLine;
			cs.OnCutByPlayer += DisableLine;
			cs.OnLinkedByEnemy += EnableLine;
			cs.OnLinkedByPlayer += EnableLine;
		}

		if(!drawOnEnable){
			DisableLine ();
		}

	}

	void Draw(GameObject start, GameObject end, Material Mat)
	{
		lr.material = Mat;
		Color color = Color.white;
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, start.transform.position);
		lr.SetPosition(1, end.transform.position);
		//lr.enabled = true;
	}
		

	// Update is called once per frame
	void Update () {

		// Update LineRenderer
		if(controller == Controller.Boss)
		{
			Draw(self.gameObject, Boss, EnemyLineMaterial);
			UpdateCollider (Boss);
			//LineEC.enabled = true;
			ControlLine.tag = "EnemyLine";
		}
		if (controller == Controller.None)
		{
			// lr.enabled = false;
			LineEC.enabled = false;
			ControlLine.tag = "Untagged";

		}
		if (controller == Controller.Hacker)
		{
			Draw(self.gameObject, Hacker, PlayerLineMateial);
			UpdateCollider (Hacker);
			//LineEC.enabled = true;
			ControlLine.tag = "PlayerLine";
		}



		// Update edge collider
//		LineEC.isTrigger = true;
//		if (Boss) {
//			Vector2[] temparray = new Vector2[2];
//			Vector3 Boss2Self = Boss.transform.position - transform.position;
//			temparray [0] = new Vector2 (0, 0);
//			temparray [1] = ControlLine.transform.InverseTransformVector (Boss2Self);
//			LineEC.points = temparray;
//		} else {
//			LineEC.enabled = false;
//		}

	}

	void UpdateCollider(GameObject target){
		Vector2[] temparray = new Vector2[2];
		Vector3 target2Self = target.transform.position - ControlLine.transform.position;
		temparray [0] = new Vector2 (0, 0);
		temparray [1] = ControlLine.transform.InverseTransformVector (target2Self);
		LineEC.points = temparray;
	}


	public void EnableLine(){
		if(LineEC){
			LineEC.enabled = true;
		}
		if(lr){
			lr.enabled = true;
		}
	}

	public void DisableLine(){
		if(LineEC){
			LineEC.enabled = false;
		}
		if(lr){
			lr.enabled = false;
		}
	}

	public void EnableLine(Transform virusTrans){
		if(LineEC){
			LineEC.enabled = true;
		}
		if(lr){
			lr.enabled = true;
		}
	}

	public void DisableLine(Transform virusTrans){
		if(LineEC){
			LineEC.enabled = false;
		}
		if(lr){
			lr.enabled = false;
		}
	}

	// translate layerMask value into layer number [0 - 31]
	int LayerMaskToLayerNum(LayerMask layerMask){
		int layerNumber = 0;
		int layer = layerMask.value;
		while(layer > 0){
			layer = layer >> 1;
			layerNumber++;
		}
		return layerNumber - 1;
	}

		
}
