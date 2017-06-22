using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using InControl;

public class MenuScripts : MonoBehaviour {

    public string sceneName;

    public GameObject menu;

    public GameObject hacker;

    public GameObject ai;

    public GameObject eventSys;

    // public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(delayedForChangeScene());
    }

    IEnumerator delayedForChangeScene()
    {
        yield return new WaitForSeconds(1f);
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
        //eventSys.GetComponent<StandaloneInputModule>().enabled = true;
        eventSys.GetComponent<InControlInputModule>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        hacker.GetComponent<PlayerControl>().canControl = true;
        ai.GetComponent<PlayerControl>().canControl = true;
    }

    public void ButtonSound(AudioSource audioSource)
    {
        // audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        // DontDestroyOnLoad(gameObject);
    }

}
