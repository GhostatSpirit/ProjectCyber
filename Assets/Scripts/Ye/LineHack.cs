using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class LineHack : MonoBehaviour
{

    InputDevice myInputDevice;
    static GameObject myLine;
    GameObject Initial;
    public float speed;

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
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        //Color color = Color.clear;
        lr.startWidth = 0.03f;
        lr.endWidth = 0.03f;
        //lr.startColor = color;
        //lr.endColor = color;
        //lr.SetPosition(0, Initial.GetComponent<ControlStatus>().Boss().transform.position);
        //lr.SetPosition(1, FindClosestEnemy().transform.position);
        myLine.transform.SetParent(gameObject.transform);
    }

    void Draw(GameObject start , GameObject end )
    {
        LineRenderer lr= myLine.GetComponent<LineRenderer>();
        Color color = Color.yellow; 
        lr.startColor = color;
        lr.endColor = color;
        lr.SetPosition(0, start.transform.position);
        lr.SetPosition(1, end.transform.position);
    } 
    void Clean()
    {
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
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

        if (myInputDevice.RightTrigger.IsPressed == true)
        {
            if (FindClosestEnemy()==true)
            {
                if (myLine.GetComponent<LineRenderer>().startColor != Color.yellow)
                {

                    GameObject Enemy = FindClosestEnemy();
                    ControlStatus CS = Enemy.GetComponent<ControlStatus>();
                    if (CS.BossControl == 0)
                    {
                        Initial = Enemy;
                        CS.BossControl = -1;
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
            if (Initial.GetComponent<ControlStatus>().BossControl == -1)
            {
                Draw(gameObject, Initial);
            }

            if (Initial.transform.position == Initial.GetComponent<ControlStatus>().Boss().transform.position)
            {
                Clean();
                Initial.GetComponent<ControlStatus>().BossControl = -2;
                Initial = null;
            }
        }

        

        /*/
        if (myInputDevice.RightTrigger.WasReleased== true)
        {
            Clean();
        }
        /*/

    }
}