using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoom : MonoBehaviour
{
    public Text text;
    public GameObject AI;
    public GameObject Hacker;
    public float distanceThreshold = 5;
    public float explodeThreshold = 5;
    public float explosionIncrementStep = 0.01f;
    public float explosionDecreaseStep = 0.02f;
    float explosionMeasurement = 0;
    public float forceMagnitude = 50f;
    public float explosionRadius = 50f;
    public float explosionFreezeTime = 2f;
    public float PlayerDragCoeff = 5f;
    public float EnemyDargCoeff = 1.8f;
    public float Dmg2Virus = 5f;
    public float Dmg2Boss = 25f;
    Vector3 explosionCenter;
    Collider2D[] explosionColliders;
    float dist;
    private PlayerEnergy AIEnergy = null;
    private PlayerEnergy HackerEnergy = null;

    IEnumerator disableMove(Rigidbody2D rb)
    {
        ObjectIdentity identity = rb.transform.GetComponent<ObjectIdentity>();
        if (identity.objType == ObjectType.Virus)
            rb.drag = EnemyDargCoeff;
        else
            rb.drag = PlayerDragCoeff;
        for (float i = 0; i < explosionFreezeTime; i += Time.deltaTime)
        {
            if (rb.GetComponent<PlayerMovement>() != null)
                yield return rb.GetComponent<PlayerMovement>().moveEnabled = false;
            else
                yield return rb.GetComponent<ChaseTarget>().moveEnabled = false;
        }

        rb.drag = 0;
        if (rb.GetComponent<PlayerMovement>() != null)
        yield return rb.GetComponent<PlayerMovement>().moveEnabled = true;
        else
        yield return rb.GetComponent<ChaseTarget>().moveEnabled = true;

    }

    // Use this for initialization
    void Start ()
    {
        AIEnergy = AI.GetComponent<PlayerEnergy>();
        HackerEnergy = Hacker.GetComponent<PlayerEnergy>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        dist = Vector2.Distance(AI.transform.position, Hacker.transform.position);
        if (dist < distanceThreshold)
            explosionMeasurement += explosionIncrementStep;
        else
        {
            explosionMeasurement -= explosionDecreaseStep;
            if (explosionMeasurement <= 0)
                explosionMeasurement = 0;
        }
        text.text = explosionMeasurement.ToString();

        if (explosionMeasurement >= explodeThreshold)
            explode();
    }

    void explode()
    {
        explosionCenter = (AI.transform.position + Hacker.transform.position) / 2;
        explosionColliders = Physics2D.OverlapCircleAll(explosionCenter, explosionRadius);

        foreach (Collider2D exploded in explosionColliders)
        {
            Rigidbody2D rb = exploded.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (rb.transform.GetComponent<ObjectIdentity>().objType == ObjectType.Boss)
                    rb.transform.GetComponent<HealthSystem>().Damage(Dmg2Boss);
                else if (rb.transform.GetComponent<ObjectIdentity>().objType == ObjectType.Virus)
                    rb.transform.GetComponent<HealthSystem>().Damage(Dmg2Virus);
                StartCoroutine(disableMove(rb));
                rb.AddForce(getForceDirection(rb, explosionCenter) * forceMagnitude, ForceMode2D.Impulse);
            }
        }
        AIEnergy.SubstractEnergy(AIEnergy.GetEnergy() / 2);
        HackerEnergy.SubstractEnergy(HackerEnergy.GetEnergy() / 2);
    }

    Vector2 getForceDirection(Rigidbody2D obj, Vector3 explosionCenter)
    {
        return (obj.transform.position - explosionCenter).normalized;
    }

}
