using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

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
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}