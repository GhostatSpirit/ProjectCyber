using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ChargingDart : MonoBehaviour {


    InputDevice myInputDevice;
    public enum chargingStatus { NotCharge, StartCharge,Charging, Release };
    public chargingStatus Status = chargingStatus.NotCharge;
    [ReadOnly]public float ChargingTime = 0 ;
	[ReadOnly]public float ChargingSum = 0;
    public float MaxChargingTime = 2;
    // Use this for initialization
    void Start () {
        Status = chargingStatus.NotCharge;	
	}
	
	// Update is called once per frame
	void Update () {
        myInputDevice = GetComponent<DeviceReceiver>().GetDevice();
        if (myInputDevice == null)
        {
            return;
        }
        /*
        if (myInputDevice.Action1.IsPressed == true)
        {
            Status = chargingStatus.Charge;
        }
        else
        {
            if (myInputDevice.Action1.WasPressed == true)
            {
                Status = chargingStatus.Release;
            }
            else
            {
                Status = chargingStatus.NotCharge;
            }
        }
        */

        // Charging
        // Debug.Log(ChargingTime);

        if (myInputDevice.Action1.IsPressed == true && myInputDevice.Action1.WasPressed == false)
        {
            Status = chargingStatus.StartCharge;
            ChargingTime += Time.deltaTime;
        }
        else if (myInputDevice.Action1.IsPressed == true && myInputDevice.Action1.WasPressed == true)
        {
            Status = chargingStatus.Charging;
            ChargingTime += Time.deltaTime;
            ChargingTime = (ChargingTime > MaxChargingTime) ? MaxChargingTime : ChargingTime;
        }
        else if (myInputDevice.Action1.IsPressed == false)
        {
            if (myInputDevice.Action1.WasReleased == true)
            {

                //  ChargingSum = ChargingTime;
                //  ChargingSum = ( ChargingTime > MaxChargingTime ) ?  MaxChargingTime : ChargingTime;
                
//                Debug.Log(Status);
                Status = chargingStatus.Release;
                ChargingSum = ChargingTime;
                ChargingTime = 0;
                // Debug.Log(ChargingTime);
            }
            else if (myInputDevice.Action1.WasPressed == false)
            {
                Status = chargingStatus.NotCharge;
                // Debug.Log(Status);
                ChargingTime = 0;
                ChargingSum = 0;

            }
        }

    }
}
