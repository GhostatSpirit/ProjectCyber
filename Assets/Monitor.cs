using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monitor : MonoBehaviour {

    public Text text;

    public GameObject player1;
    public GameObject player2;
    public Material LineMat;
    public float distanceThreshold = 5;
    public float explosionIncrementStep = 0.01f;
    public float explosionDecreaseStep = 0.02f;
    float explosionMeasurement = 0;
    public float forceMagnitude = 50f;
    public float explosionRadius = 50f;
    public float explosionFreezeTime = 2f;
    public float dragCoeff = 5f;

    public bool enableVisulization = false;

    float dist;
    LineRenderer LR;
    Line line;

    Vector3[] initLinePos = new Vector3[2];

    Vector3 explosionCenter;
    //Vector2 player1ForceDirection;
    //Vector2 player2ForceDirection;

    Collider2D[] explosionColliders;
    // Use this for initialization
    void Start ()
    {
        initLinePos[0] = Vector3.zero;
        initLinePos[1] = Vector3.zero;

        LR = gameObject.AddComponent<LineRenderer>();
        LR.material = LineMat;
        line = new Line(player1.transform.position, player2.transform.position);
        text.text = explosionMeasurement.ToString();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        dist = Vector2.Distance(player1.transform.position, player2.transform.position);
        if (dist < distanceThreshold)
        {
            explosionMeasurement += explosionIncrementStep;
            if(enableVisulization)
            {
                line.setEnd(player2.transform.position);
                line.setStart(player1.transform.position);
                LR.SetPositions(line.pts);
            }
        }
        else
        {
            explosionMeasurement -= explosionDecreaseStep;
            if (explosionMeasurement <= 0)
                explosionMeasurement = 0;
            if (enableVisulization)
                LR.SetPositions(initLinePos);
        }
        text.text = explosionMeasurement.ToString();

        if(explosionMeasurement >= 100)
        {
            explode();
        }
    }

    void explode()
    {
        explosionCenter = (player1.transform.position + player2.transform.position) / 2;
        explosionColliders = Physics2D.OverlapCircleAll(explosionCenter, explosionRadius);

        foreach (Collider2D exploded in explosionColliders)
        {
            Rigidbody2D rb = exploded.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(getForceDirection(rb, explosionCenter) * forceMagnitude, ForceMode2D.Impulse);
                StartCoroutine(disableMove(rb));
            }

        }
    }

    Vector2 getForceDirection(Rigidbody2D obj, Vector3 explosionCenter)
    {
        return (obj.transform.position - explosionCenter).normalized;
    }

    IEnumerator disableMove(Rigidbody2D rb)
    {
        rb.drag = dragCoeff;
        for (float i = 0; i < explosionFreezeTime; i += Time.deltaTime)
        {            
            if (rb.GetComponent<PlayerMovement>() != null)
                yield return rb.GetComponent<PlayerMovement>().moveEnabled = false;
            else
                yield return rb.GetComponent<Enemy_Movement>().moveEnabled = false;
        }
        rb.drag = 0;
        if (rb.GetComponent<PlayerMovement>() != null)
            yield return rb.GetComponent<PlayerMovement>().moveEnabled = true;
        else
            yield return rb.GetComponent<Enemy_Movement>().moveEnabled = true;

    }

}


