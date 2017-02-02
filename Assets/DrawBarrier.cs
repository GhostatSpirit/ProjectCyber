using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Line
{
    public Vector3[] pts;
    public Vector2[] pts2D;

    public Line(Vector3 p1, Vector3 p2)
    {
        pts = new Vector3[2];
        pts[0] = p1;
        pts[1] = p2;
        pts2D = new Vector2[2];
        pts2D[0] = new Vector2(p1.x, p1.y);
        pts2D[1] = new Vector2(p2.x, p2.y);
    }

    public void setEnd(Vector3 p)
    {
        pts[1] = p;
        pts2D[1] = new Vector2(p.x, p.y);
    }
}

public class DrawBarrier : MonoBehaviour 
{
    public GameObject player;
    public Material LineMat;

    InputDevice myInputDevice;
    float RTrigger = 0f;

    GameObject CrntBarrier;
    Queue<GameObject> Barriers;
    LineRenderer CrntLR;
    Line CrntLine;
    Queue<Line> Lines;
    EdgeCollider2D CrntEC;

    bool status = false;
    bool prevStatus = false;

    void Start ()
    {
        Barriers = new Queue<GameObject>();
        Lines = new Queue<Line>();
    }
	
	void FixedUpdate () 
    {
        myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
        RTrigger = myInputDevice.RightTrigger;

        if (RTrigger >= 0.3f)
            status = true;
        else
            status = false;

        if (status != prevStatus)
        {
            if (!prevStatus)
            {
                CrntBarrier = new GameObject("BarrierEntity");
                CrntBarrier.transform.parent = transform;
                CrntBarrier.AddComponent<TriggerEvent>();
                CrntLine = new Line(player.transform.position, player.transform.position);
                Lines.Enqueue(CrntLine);
                CrntLR = CrntBarrier.AddComponent<LineRenderer>();
                CrntLR.material = LineMat;
                CrntLR.SetPositions(CrntLine.pts);
                CrntEC = CrntBarrier.AddComponent<EdgeCollider2D>();
                CrntEC.isTrigger = true;
                Barriers.Enqueue(CrntBarrier);
            }
            else
            {
                CrntEC.points = CrntLine.pts2D;
                if (Barriers.Count != 0)
                    Destroy(Barriers.Dequeue(), 3);
            }
        }

        if (status)
        {
            CrntLine.setEnd(player.transform.position);
            CrntLR.SetPositions(CrntLine.pts);
        }

        prevStatus = status;
	}
}
