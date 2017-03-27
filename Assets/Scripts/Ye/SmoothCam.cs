using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCam : MonoBehaviour {
    Camera Main;
    public float smoothTime = 0.3f;
    public float Max;
    public float Min;
    private Vector3 velocity = Vector3.zero;
    public GameObject AI;
    public GameObject Hacker;

    static float t = 0.0f;
    float Initial;
    float AHDistance;
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
        AHDistance = Vector2.Distance(AIPosition, HackerPosition);
        float size = (float) 0.8 * AHDistance ;

        if (size < Min)
        {
            size = Min;
        }
        if (size > Max)
            size = Max;

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

        Main.orthographicSize = Initial;
        Main.aspect = ;
        Initial = size;

        //Main.orthographicSize = (Screen.height / 100f ) / 4f;
        transform.position = Vector3.Lerp (transform.position, targetPosition, 0.01f);
    }
}
