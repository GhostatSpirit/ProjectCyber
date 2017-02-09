using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHack : MonoBehaviour {
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

    void DrawLine(Vector3 start, Vector3 end, float duration = 0.2f)
    {
        myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        Color color = Color.red;
        lr.SetWidth(0.1f, 0.1f);
        lr.SetColors(color,color);
        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, FindClosestEnemy().transform.position);
       // Debug.Log("Line is drawn");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            Destroy(myLine, 0.0001f);
            //Invoke("Show",1f);
            DrawLine(gameObject.transform.position, FindClosestEnemy().transform.position);
        }
   
    }

}