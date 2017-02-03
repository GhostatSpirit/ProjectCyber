using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject target;
    public GameObject enemy;
    public Vector3 spawnCenter;
    public int spawnAreaSize = 20;
    public int enemyCount = 20;

    public float waveInterval;
    public float singleInterval;
    public float startInterval;
	// Use this for initialization
	void Start () {
       StartCoroutine(Spawn());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(startInterval);
        while (true)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(spawnCenter.x - spawnAreaSize, spawnCenter.x + spawnAreaSize), Random.Range(spawnCenter.y - spawnAreaSize, spawnCenter.y + spawnAreaSize), spawnCenter.z);
                Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
                enemy.GetComponent<Enemy_Movement>().target = target;
                yield return new WaitForSeconds(singleInterval);
            }
            yield return new WaitForSeconds(waveInterval);
        }
    }
}
