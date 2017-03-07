using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.SceneManagement;
public class Reload : MonoBehaviour
{
    InputDevice myInputDevice;
    void Update()
    {
        myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
        if (myInputDevice == null)
        {
            return;
        }
        if (myInputDevice.Action2.IsPressed)
        {
			
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
        }
    }
}