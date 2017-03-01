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

	public float lineWidth;

	// a read-only Boss property that reads Boss from ControlStatus
	GameObject Boss{
		get{
			return GetComponent<ControlStatus> ().Boss;
		}
	}

	// a read-only Boss property that reads Hacker from ControlStatus
	GameObject Hacker{
		get{
			return GetComponent<ControlStatus> ().Hacker;
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
		ControlLine.transform.position = gameObject.transform.position;

		ControlLine.AddComponent<LineRenderer>();
		LineRenderer lr = ControlLine.GetComponent<LineRenderer>();
		lr.material = EnemyLineMaterial;
		Color color = Color.white;
		lr.startWidth = lineWidth;
		lr.endWidth = lineWidth;
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, gameObject.transform.position);
		lr.SetPosition(1, Boss.transform.position);

		ControlLine.AddComponent<EdgeCollider2D>();
		EdgeCollider2D BossLineEC = ControlLine.GetComponent<EdgeCollider2D>();
		BossLineEC.isTrigger = true;
		Vector2[] temparray = new Vector2[2];
		temparray[0] = new Vector2(0, 0);
		temparray[1] = new Vector2(Boss.transform.position.x- gameObject.transform.position.x, Boss.transform.position.y - gameObject.transform.position.y);
		BossLineEC.points = temparray;

		// Debug.Log(BossLineEC.points[0]);
		// Debug.Log(BossLineEC.points[1]);

		ControlLine.tag = "EnemyLine";

		// rename this new game object
		ControlLine.name = "ControlLine";

		ControlLine.transform.SetParent(gameObject.transform);
		// Debug.Log(gameObject.name + ControlLine.transform.position);
	}

	void Draw(GameObject start, GameObject end, Material Mat)
	{
		LineRenderer lr = ControlLine.GetComponent<LineRenderer>();
		lr.material = Mat;
		Color color = Color.white;
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, start.transform.position);
		lr.SetPosition(1, end.transform.position);
	}

	void Clean()
	{
		LineRenderer lr = ControlLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply")); ;
		Color color = Color.clear;
		lr.startColor = color;
		lr.endColor = color;
	}

	// Update is called once per frame
	void Update () {

		// Update LineRenderer
		if(controller == Controller.Boss)
		{
			Draw(gameObject, Boss, EnemyLineMaterial);
			ControlLine.tag = "EnemyLine";
		}
		if (controller == Controller.None)
		{
			Clean();// DO sth; Cleanlean the ControlLine

		}
		if (controller == Controller.Hacker)
		{
			Draw(gameObject, Hacker, PlayerLineMateial);
			ControlLine.tag = "PlayerLine";
		}



		// Update edge collider
		EdgeCollider2D BossLineEC = ControlLine.GetComponent<EdgeCollider2D>();
		BossLineEC.isTrigger = true;
		Vector2[] temparray = new Vector2[2];
		Vector3 Boss2Self = Boss.transform.position - transform.position;
		temparray[0] = new Vector2(0, 0);
		temparray[1] = ControlLine.transform.InverseTransformVector (Boss2Self);
		BossLineEC.points = temparray;


	}
}
