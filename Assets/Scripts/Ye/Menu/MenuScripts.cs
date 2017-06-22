using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuScripts : MonoBehaviour {

    public string sceneName;

    public GameObject menu;

    public GameObject hacker;

    public GameObject ai;

    public GameObject eventSys;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Resume()
    {
        // Debug.Log("time start");

        Time.timeScale = 1;
        menu.SetActive(false);
        StartCoroutine(nowControl(transform));
        ai.GetComponent<ControlPauseMenu>().pauseTime = 0;
        hacker.GetComponent<ControlPauseMenu>().pauseTime = 0;
    }

    IEnumerator nowControl(Transform trans)
    {
        eventSys.GetComponent<StandaloneInputModule>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        hacker.GetComponent<PlayerControl>().canControl = true;
        ai.GetComponent<PlayerControl>().canControl = true;
    }

}
