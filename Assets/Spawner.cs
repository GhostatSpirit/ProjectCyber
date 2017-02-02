using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject enemy;
    public Vector3 spawnCenter;
    public int enemyCount;
	// Use this for initialization
	void Start () {
        Spawn();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Spawn()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(spawnCenter.x - 20, spawnCenter.x + 20), Random.Range(spawnCenter.y - 20, spawnCenter.y + 20), spawnCenter.z);
            Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
        }

    }
}
