using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjust : MonoBehaviour {
    Camera Main  ;
    public float Max;
    public float Min;
    public GameObject AI;
    public GameObject Hacker;
    //float Initial;
    float ratio;
    float AHDistance;
    /*/
    public GameObject AI()
    {
        GameObject Ai;
        Ai = GameObject.FindGameObjectWithTag("AI");
        return Ai;
    }

    public GameObject Hacker()
    {
        GameObject Hack;
        Hack = GameObject.FindGameObjectWithTag("Hacker");
        return Hack;
    }
    /*/
    // Use this for initialization
    void Start () {
        Main = gameObject.GetComponent<Camera>();
        Main.orthographicSize = Min;
        Vector2 AIPos = AI.transform.position;
        Vector2 HackerPos = Hacker.transform.position;
        Vector3 Temp = new Vector3((AIPos.x + HackerPos.x) / 2, (AIPos.y + HackerPos.y) / 2 , -3);
        gameObject.transform.position = Temp;
        //Initial = Vector2.Distance(AIPos, HackerPos);
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 AIPos = AI.transform.position;
        Vector2 HackerPos = Hacker.transform.position;
        Vector3 Temp = new Vector3((AIPos.x + HackerPos.x) / 2, (AIPos.y + HackerPos.y) / 2 , -3);
        gameObject.transform.position = Temp;
        /*/
        if (Input.GetKey(KeyCode.Q))
        {
            Main.orthographicSize = Main.orthographicSize + 1 * Time.deltaTime;
            if(Main.orthographicSize >8)
            {
                Main.orthographicSize = 8;
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            Main.orthographicSize = Main.orthographicSize - 1 * Time.deltaTime;
            if (Main.orthographicSize < 4)
            {
                Main.orthographicSize = 4;
            }
        }
        /*/
        AHDistance=Vector2.Distance(AIPos,HackerPos);
        /*/
        if (AHDistance > Max/2)
        {
            Main.orthographicSize = Max/2;
        }
        if (AHDistance < Min/2)
        {
            Main.orthographicSize = Min/2;
        }
        /*/
        /*/
         if (Initial > AHDistance)
                {
                    Main.orthographicSize = Main.orthographicSize - ratio * Time.deltaTime;
                    if (Main.orthographicSize < AHDistance)
                    {
                        Main.orthographicSize = AHDistance;
                    }
                }
         if (Initial <  AHDistance)
                {
                    Main.orthographicSize = Main.orthographicSize + ratio * Time.deltaTime;
                    if (Main.orthographicSize > AHDistance)
                    {
                        Main.orthographicSize = AHDistance;
                    }
                }   
            
        /*/
        float size = (float) 0.8 * AHDistance;
        if (size<Min)
        {
            size = Min;
        }
        if (size > Max)
            size = Max;
        Main.orthographicSize = size;

        //Initial = AHDistance;

    }
}
