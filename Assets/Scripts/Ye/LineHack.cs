using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class LineHack : MonoBehaviour
{

    InputDevice myInputDevice;

    public Material HackLineMat;
    public float width;
    static GameObject myLine;
    GameObject Initial;
    public float speed;
    float Status;
    

    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void Start()
    {
        speed = 15f;
        Initial = FindClosestEnemy();
        myLine = new GameObject();
        myLine.transform.position = gameObject.transform.position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.material = HackLineMat;
        //Color color = Color.clear;
        lr.startWidth = width;
        lr.endWidth = width;
        //lr.startColor = color;
        //lr.endColor = color;
        //lr.SetPosition(0, Initial.GetComponent<ControlStatus>().Boss().transform.position);
        //lr.SetPosition(1, FindClosestEnemy().transform.position);
        myLine.transform.SetParent(gameObject.transform);
        Status = 0;
    }

    void Draw(GameObject start , GameObject end )
    {
        LineRenderer lr= myLine.GetComponent<LineRenderer>();
        lr.material = HackLineMat;
        Color color = Color.white; 
        lr.startColor = color;
        lr.endColor = color;
        lr.SetPosition(0, start.transform.position);
        lr.SetPosition(1, end.transform.position);
        lr.textureMode = LineTextureMode.Tile;

        float length = Vector2.Distance(start.transform.position, end.transform.position);
        lr.material.SetTextureScale("_MainTex", new Vector2(length * 2, 1));
    } 

    void Clean()
    {
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        Color color = Color.clear;
        lr.startColor = color;
        lr.endColor = color;
        
    }
    

    void Update()
    {
        myInputDevice = GetComponent<DeviceReceiver>().GetDevice();

        if (myInputDevice == null)
        {
            return;
        }

        float horizontal = myInputDevice.LeftStickX;
        float vertical = myInputDevice.LeftStickY;
        /*/
        if (myInputDevice.RightTrigger.IsPressed == true)
        {
            if (FindClosestEnemy()==true)
            {
                if (Status == 0 )
                {
                    GameObject Enemy = FindClosestEnemy();
                    ControlStatus CS = Enemy.GetComponent<ControlStatus>();
                    if (CS.controller == ControlStatus.Controller.None)
                    {
                        Initial = Enemy;
                        CS.controller = ControlStatus.Controller.Hacker;
                        
                    }
                    else
                    {
                        Clean();
                    }
                }
            }
        }
        if (Initial != null)
        {
            if (Initial.GetComponent<ControlStatus>().controller == ControlStatus.Controller.Hacker)
            {
                Draw(gameObject, Initial);
                Status = 1;
            }

            if (Initial.transform.position == Initial.GetComponent<ControlStatus>().Boss.transform.position)
            {
                Clean();
                Initial.GetComponent<ControlStatus>().controller = ControlStatus.Controller.Destroyer;
                Initial = null;
                Status = 0;
            }
        }
        /*/
        

        /*/
        if (myInputDevice.RightTrigger.WasReleased== true)
        {
            Clean();
        }
        /*/

    }
}