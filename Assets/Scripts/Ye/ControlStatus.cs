using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStatus : MonoBehaviour {

	public enum Controller {Boss, None, Hacker, Destroyer};

	public Controller controller = Controller.Boss;

    public int BossControl;
    GameObject BossLine;
    
    //public float speed;
    

    public GameObject Boss()
    {
        GameObject Bo;
        Bo = GameObject.FindGameObjectWithTag("Boss");
        return Bo;
    }

    void Start()
    {

        BossLine = new GameObject();
        BossLine.transform.position = gameObject.transform.position;
        BossLine.AddComponent<LineRenderer>();
        
		LineRenderer lr = BossLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        Color color = Color.blue;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.startColor = color;
        lr.endColor = color;
        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, Boss().transform.position);
        
		BossLine.AddComponent<EdgeCollider2D>();
        EdgeCollider2D BossLineEC = BossLine.GetComponent<EdgeCollider2D>();
        BossLineEC.isTrigger = true;
        
		Vector2[] temparray = new Vector2[2];
        temparray[0] = new Vector2(0, 0);
        temparray[1] = new Vector2(Boss().transform.position.x- gameObject.transform.position.x, Boss().transform.position.y - gameObject.transform.position.y);
        BossLineEC.points = temparray;
        
		//Debug.Log(BossLineEC.points[0]);
        //Debug.Log(BossLineEC.points[1]);
        BossLine.tag = "EnemyLine";
        BossLine.transform.SetParent(gameObject.transform);
        Debug.Log(gameObject.name + BossLine.transform.position);
    }

    void Draw(GameObject start, GameObject end)
    {
        LineRenderer lr = BossLine.GetComponent<LineRenderer>();
        Color color = Color.blue;
        lr.startColor = color;
        lr.endColor = color;
        lr.SetPosition(0, start.transform.position);
        lr.SetPosition(1, end.transform.position);
    }

    void Clean()
    {
        LineRenderer lr = BossLine.GetComponent<LineRenderer>();
        Color color = Color.clear;
        lr.startColor = color;
        lr.endColor = color;
    }
    
    void Update()
    {
		if (controller == Controller.Boss)  // Controlled by Boss
        {
            Draw(gameObject,Boss());
        }
		if (controller == Controller.None)  // Uncontrolled
        { 
            Clean();
        }
		if (controller == Controller.Hacker) // Controlled by Hacker
        {
            Clean();
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Boss().transform.position, speed * Time.deltaTime);
        }
		if (controller == Controller.Destroyer)
        {
            Destroy(gameObject);
        }
    }
    
}
