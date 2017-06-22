using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DeviceReceiver))]

public class ControlPauseMenu : MonoBehaviour {

    // for joystick
    
    InputDevice myInputDevice;
    
    public GameObject pauseMenu;
    public GameObject EventSys;

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
        myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
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
                //EventSys.GetComponent<StandaloneInputModule>().enabled = true;
                EventSys.GetComponent<InControlInputModule>().enabled = false;
                // EventSys.GetComponent<InputModuleActionAdapter>();
                Time.timeScale = 1;
                
                GetComponent<PlayerControl>().canControl = true;
                Debug.Log(GetComponent<PlayerControl>().canControl);
                pauseTime = 0;
            }
            else if (pauseMenu.activeInHierarchy == false && openTime >= minTime)
            {
                // pause 
                // Debug.Log("pause start");
                //EventSys.GetComponent<StandaloneInputModule>().enabled = false;
                EventSys.GetComponent<InControlInputModule>().enabled = true;
                Time.timeScale = 0;

                GetComponent<PlayerControl>().canControl = false;
                Debug.Log(GetComponent<PlayerControl>().canControl);
                pauseMenu.SetActive(true);
                openTime = 0;
            }

        }
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
