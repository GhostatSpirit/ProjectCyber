using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStatus : MonoBehaviour {
    public enum Controller { Boss, None, Hacker, Destroyer };
    public Controller controller = Controller.Boss;//old version use bosscontrol 

    GameObject ControlLine;
    
    public Material EnemyLineMaterial;
    public Material PlayerLineMateial;

    public float speed;
    
    public GameObject Boss;
    public GameObject Hacker;

    public float width;

    /*/
    public GameObject Boss()
    {
        GameObject Bo;
        Bo = GameObject.FindGameObjectWithTag("Boss");
        return Bo;
    }
    /*/

    void Start()
    {
        ControlLine = new GameObject();
        ControlLine.transform.position = gameObject.transform.position;

        ControlLine.AddComponent<LineRenderer>();
        LineRenderer lr = ControlLine.GetComponent<LineRenderer>();
        lr.material = EnemyLineMaterial;
        Color color = Color.white;
        lr.startWidth = width;
        lr.endWidth = width;
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
    
    void Update()
    {
        /*/
        if (BossControl == 1)  // Controlled by Boss
        {
            Draw(gameObject,Boss());
        }
        if (BossControl == 0)  // Uncontrolled
        { 
            Clean();
        }
        if (BossControl == -1) // Controlled by Hacker
        {
            Clean();
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Boss().transform.position, speed * Time.deltaTime);
        }
        if (BossControl == -2)
        {
            Destroy(gameObject);
        }
        /*/

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
        if (controller == Controller.Destroyer)
        {
            // Destroy Line
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
