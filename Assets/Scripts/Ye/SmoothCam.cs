using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCam : MonoBehaviour {
    Camera Main;
    public float smoothTime = 0.3f;
    public float Max;
    public float Min;
   
    public GameObject AI;
    public GameObject Hacker;
    public float WHRatio = 16f / 9f;
    public float AspectRatio = 4f / 3f ;

    float Initial;
    float AHDistance;
    float AHX;
    float AHY;
    float size;
    

    // Use this for initialization
    void Start () {
        Main = gameObject.GetComponent<Camera>();

    }

	// Update is called once per frame
	void Update () {
        Vector3 AIPosition = new Vector3(AI.transform.position.x, AI.transform.position.y, -1);
        Vector3 HackerPosition = new Vector3(Hacker.transform.position.x, Hacker.transform.position.y, -1);
        Vector3 targetPosition = new Vector3((AIPosition.x + HackerPosition.x)/2, (AIPosition.y + HackerPosition.y)/2, -1);
        // transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        // AHDistance = Vector2.Distance(AIPosition, HackerPosition);
        AHX = Mathf.Abs( AIPosition.x - HackerPosition.x );
        AHY = Mathf.Abs( AIPosition.y - HackerPosition.y);

        

        if (AHX / AHY <= WHRatio)
        {
            size =  AspectRatio * AHY;
        }

        if (AHX / AHY > WHRatio)
        {
            size = AHX / WHRatio * AspectRatio ;
        }

        /*/
        if (AHX / AHY < WHRatio)
        {
            Main.orthographicSize = AHY * AspectRatio;
        }
        /*/

        //float size = (float) 0.8 * AHDistance ;


        if (size < Min)
        {
            size = Min;
        }
        else
        {
            Main.orthographicSize = size;
        }

        /*/
        if (size > Max)
            size = Max;
        /*/

        /*
        Main.orthographicSize = Mathf.Lerp(Initial, size, t);
        t += 0.5f * Time.deltaTime;
        if (t > 1.0f)
        {
            float temp = size;
            size = Initial;
            Initial = temp;
            t = 0.0f;
        }
        */

        // Main.orthographicSize = Initial;
        // aspect change added;
       
        // Initial = size;

        transform.position = Vector3.Lerp (transform.position, targetPosition, 0.05f);
    }
}
