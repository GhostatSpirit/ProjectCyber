using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.FastLineRenderer;

public class Movement : MonoBehaviour {
    public Sprite front;
    public Sprite left;
    public Sprite right;
    public Vector2 speed = new Vector2(1, 1);
    private Vector2 movement;
    public bool is_drawing = false;
    public Vector3 prev_pos = Vector3.zero;
    public Vector3 crnt_pos = Vector3.zero;

    private int EC_count = -1;
    public GameObject FLR_Object;
    private FastLineRenderer FLR;
    private FastLineRendererProperties FLRP;
    private bool last = false;
    private GameObject crnt_edge_collider_OBJ;
    private EdgeCollider2D crnt_edge_collider;
    private List<List<Vector2>> EC_points;

    void Start()
    {
        FLR = FLR_Object.GetComponent<FastLineRenderer>();
        crnt_pos = prev_pos = GetComponent<Transform>().position;
        FLR.UseWorldSpace = true;
        FLRP = new FastLineRendererProperties();
        FLRP.Start = FLRP.End = crnt_pos;
        FLRP.Radius = 0.5f;
        FLRP.Color = new Color32(255, 0, 0, 255);
        FLRP.LineJoin = FastLineRendererLineJoin.AttachToPrevious;
        FLRP.SetLifeTime(1f, 999f, 0f);
        EC_points = new List<List<Vector2>>();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        float rtaxis = Input.GetAxis("Fire");

        if (rtaxis > 0.5)
            is_drawing = true;
        else
            is_drawing = false;

        movement = new Vector2(speed.x * inputX, speed.y * inputY);
        GetComponent<Rigidbody2D>().velocity = movement;
        crnt_pos = GetComponent<Transform>().position;
        if (System.Math.Abs(inputX) > 0.5 && System.Math.Abs(inputY) < 0.5)
        {
            if (inputX > 0)
                GetComponent<SpriteRenderer>().sprite = right;
            else
                GetComponent<SpriteRenderer>().sprite = left;
        }
        if (System.Math.Abs(inputY) > 0.5 && System.Math.Abs(inputX) < 0.5)
            GetComponent<SpriteRenderer>().sprite = front;

        // Draw line

        if (dist(crnt_pos, prev_pos) >= 5)
        {
            if (is_drawing != last)
            {
                if(last == true)
                   FLR.EndLine(FLRP);
                else
                {
                    crnt_edge_collider_OBJ = new GameObject();
                    crnt_edge_collider_OBJ.layer = 8;
                    crnt_edge_collider_OBJ.transform.position = crnt_pos;
                    crnt_edge_collider = crnt_edge_collider_OBJ.AddComponent<EdgeCollider2D>();
                    EC_points.Add(new List<Vector2>());
                    EC_count++;
                }
            }
            if(is_drawing)
            {
                EC_points[EC_count].Add(new Vector2(crnt_pos.x - crnt_edge_collider.transform.position.x, crnt_pos.y - crnt_edge_collider.transform.position.y));
                crnt_edge_collider.points = EC_points[EC_count].ToArray();
                FLRP.Start = crnt_pos;
                FLR.AppendLine(FLRP);
                FLR.Apply();
                prev_pos = crnt_pos;
            }
                last = is_drawing;
        }
    }

    private float dist(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y));
    }
}
