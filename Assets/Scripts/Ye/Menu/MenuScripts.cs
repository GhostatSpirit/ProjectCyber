using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using InControl;

public class MenuScripts : MonoBehaviour {

    // public string sceneName;

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
        StartCoroutine(delayedForChangeScene(sceneName));
    }

    IEnumerator delayedForChangeScene(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);

    }

    public void Resume(AudioSource audioSource)
    {
        // audioSource.Play();
        // Debug.Log("time start");
        StartCoroutine(delayedSound(audioSource));
        
    }

    IEnumerator delayedSound(AudioSource audioSource)
    {
        // sound play
        audioSource.Play();
        yield return new WaitForSecondsRealtime(0.5f);
        // time start flow
        Time.timeScale = 1;
        menu.SetActive(false);
        eventSys.GetComponent<InControlInputModule>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        // ai and hacker can control now
        hacker.GetComponent<PlayerControl>().canControl = true;
        ai.GetComponent<PlayerControl>().canControl = true;
        ai.GetComponent<ControlPauseMenu>().pauseTime = 0;
        hacker.GetComponent<ControlPauseMenu>().pauseTime = 0;
        yield return null;
    }

    IEnumerator nowControl(Transform trans)
    {
        //eventSys.GetComponent<StandaloneInputModule>().enabled = true;
        eventSys.GetComponent<InControlInputModule>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        hacker.GetComponent<PlayerControl>().canControl = true;
        ai.GetComponent<PlayerControl>().canControl = true;
        yield return null;
    }

    public void ButtonSound(AudioSource audioSource)
    {
        audioSource.Play();
    }
}
