using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTest : MonoBehaviour {

    public GameObject pauseMenu;

    [HideInInspector] public float pauseTime = 0;

    [HideInInspector] public float openTime = 3;

    float minTime = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (pauseMenu.activeInHierarchy == true && pauseTime < minTime)
        {
            pauseTime += Time.deltaTime;

        }

        if (pauseMenu.activeInHierarchy == false && openTime < minTime)
        {
            openTime += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.P))
        {
            if (pauseMenu.activeInHierarchy == true && pauseTime >= minTime)
            {
                Debug.Log("pause stop");
                pauseMenu.SetActive(false);
                pauseTime = 0;
            }
            else if(pauseMenu.activeInHierarchy == false && openTime >= minTime)
            {
                Debug.Log("pause start");
                pauseMenu.SetActive(true);
                openTime = 0;
            }
            
        }       
	}
}
