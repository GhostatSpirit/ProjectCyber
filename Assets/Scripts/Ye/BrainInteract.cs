using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class BrainInteract : MonoBehaviour {

    GameObject ai;
    GameObject hacker;
    InputDevice aiInputDevice;
    InputDevice hackerInputDevice;

    public float interactiveDistance = 10;
    float aiDistance;
    float hackerDistance;
    float counter = 0;

	// Use this for initialization
	void Start () {
        ai = GetComponent<FinalBrain>().AI;
        hacker = GetComponent<FinalBrain>().Hacker;
        
	}
	
	// Update is called once per frame
	void Update () {
        aiInputDevice = ai.GetComponent<DeviceReceiver>().GetDevice();
        hackerInputDevice = hacker.GetComponent<DeviceReceiver>().GetDevice();
        if (aiInputDevice == null)
        {
            return;
        }
        if(hackerInputDevice == null)
        {
            return;
        }

        aiDistance = Vector2.Distance(gameObject.transform.position, ai.transform.position);
        hackerDistance = Vector2.Distance(gameObject.transform.position, hacker.transform.position);

        if ((aiDistance <= interactiveDistance && aiInputDevice.Action3.IsPressed)||(hackerDistance <= interactiveDistance && hackerInputDevice.Action3.IsPressed)||(counter == 1))
        {
            GetComponent<FinalBrain>().BS = BrainStatus.active;
            counter = 1;
        }
       else
        {
            GetComponent<FinalBrain>().BS = BrainStatus.negative;
        }



    }
}
