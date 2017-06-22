using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.EventSystems;

public class ControlPauseMenu : MonoBehaviour {

    //for joystick
    public Transform deviceAssigner;
    InputDevice myInputDevice;
    public int playerIndex = 0;

    public GameObject pauseMenu;
    public GameObject standAlone;

    // time for open and close pause menu
    [HideInInspector] public float pauseTime = 0;

    [HideInInspector] public float openTime = 3;

    // min response time
    public float minTime = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
        // Debug.Log("  open:  "+openTime + "  pause:   " + pauseTime);
        // check for Incontrol
        myInputDevice = deviceAssigner.
        GetComponent<DeviceAssigner>().GetPlayerDevice(playerIndex);
        if (myInputDevice == null)
        {
            return;
        }

        // for close response
        if (pauseMenu.activeInHierarchy == true && pauseTime < minTime)
        {
            // Debug.Log("pause and not response");
            pauseTime += Time.unscaledDeltaTime;

        }

        // for open response
        if (pauseMenu.activeInHierarchy == false && openTime < minTime)
        {
            openTime += Time.unscaledDeltaTime;
        }

        if (myInputDevice.Command.IsPressed == true)
        {
            // Debug.Log("press");
            

            if (pauseMenu.activeInHierarchy == true && pauseTime >= minTime)
            {
                // resume
                // Debug.Log("pause stop");
                pauseMenu.SetActive(false);
                standAlone.GetComponent<StandaloneInputModule>().enabled = true;
                Time.timeScale = 1;
                GetComponent<PlayerControl>().canControl = true;
                pauseTime = 0;
            }
            else if (pauseMenu.activeInHierarchy == false && openTime >= minTime)
            {
                // pause 
                // Debug.Log("pause start");
                standAlone.GetComponent<StandaloneInputModule>().enabled = false;
                Time.timeScale = 0;
                GetComponent<PlayerControl>().canControl = false;
                pauseMenu.SetActive(true);
                openTime = 0;
            }

        }





        //if (myInputDevice.Command.IsPressed == true)
        //{
        //    GetComponent<PlayerControl>().canControl = false;
        //    Time.timeScale = 0;
        //    pauseMenu.SetActive(true);
        //}


    }


    IEnumerator OpenMenu(GameObject pauseMenu)
    {
        pauseMenu.SetActive(true);
        yield return new WaitForSeconds(5f);
    }

    IEnumerator CloseMenu(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);
        yield return new WaitForSeconds(5f);
    }

    // If command is pressed, open the pause menu or close it
    IEnumerator Menu(GameObject pauseMenu)
    {
        if (myInputDevice.Command.IsPressed == true)
        {
            if (pauseMenu.activeInHierarchy)
            {
                Debug.Log("menu active");
                StartCoroutine(CloseMenu(pauseMenu));
                Time.timeScale = 1;
                yield return new WaitForSeconds(1f);

            }
            else
            {
                Debug.Log("menu inactive");
                Time.timeScale = 0;
                StartCoroutine(OpenMenu(pauseMenu));
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }

    }
}
