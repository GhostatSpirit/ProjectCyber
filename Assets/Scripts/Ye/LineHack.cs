using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class LineHack : MonoBehaviour
{

    InputDevice myInputDevice;
    static GameObject myLine;


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
    void Show()
    {
        Debug.Log(FindClosestEnemy());
        Debug.Log("Showed");
    }

    void Start()
    {
        myLine = new GameObject();
        myLine.transform.position = gameObject.transform.position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        Color color = Color.clear;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.startColor = color;
        lr.endColor = color;
        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, FindClosestEnemy().transform.position);

    }

    void Draw(GameObject start , GameObject end )
    {
        LineRenderer lr= myLine.GetComponent<LineRenderer>();
        Color color = Color.red; 
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

        if (myInputDevice.RightTrigger.IsPressed == true )
        {
            // Destroy(myLine, 0.0000001f);
            // Invoke("Show",1f);
            Draw(gameObject, FindClosestEnemy());
           
        }

        
        if (myInputDevice.RightTrigger.WasReleased== true)
        {
            Clean();
        }
        

        /*/
                if (Input.GetKey(KeyCode.G))
                {
                    Destroy(myLine, 0.0001f);
                    //Invoke("Show",1f);
                    DrawLine(gameObject.transform.position, FindClosestEnemy().transform.position);
                }

            
        /*/
    }
}