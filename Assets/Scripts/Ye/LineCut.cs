using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCut : MonoBehaviour {

    Rigidbody2D AIrb;
    Vector2 VAI;
    Vector2 AIPos;
    Vector2 AIPos1;
    Vector2 LineStart;
    Vector2 LineEnd;
    Vector2 Hit;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyLine")
        {
            AIrb = GetComponent<Rigidbody2D>();
            AIPos = AIrb.position;
            VAI = AIrb.velocity;
            LineStart = other.GetComponent<LineRenderer>().GetPosition(0);
            LineEnd = other.GetComponent<LineRenderer>().GetPosition(1);
            AIPos1 = new Vector2 ( VAI.x+AIPos.x , VAI.y + AIPos.y);
            float D = (AIPos1.x - AIPos.x) * (LineEnd.y - LineStart.y)-(LineEnd.x - LineStart.x)*(AIPos1.y - AIPos.y);
            float b1 = (AIPos1.y - AIPos.y) * AIPos.x + (AIPos.x - AIPos1.x) * AIPos.y;
            float b2 = (LineEnd.y - LineStart.y) * LineStart.x + (LineStart.x - LineEnd.x) * LineStart.y; ;
            float D1 = b2 * (AIPos1.x - AIPos.x) - b1 * (LineEnd.x - LineStart.x);
            float D2 = b2 * (AIPos1.y - AIPos.y) - b1 * (LineEnd.y - LineStart.y);
            float X0 = D1 / D;
            float Y0 = D2 / D;
            Hit = new Vector2(X0, Y0);
            Debug.Log("Hit");
            Debug.Log(X0);
            Debug.Log(Y0);
            Destroy(other.gameObject);
        }
    }
}
