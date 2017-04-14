using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
public class SceneExitApplication : MonoBehaviour {
	public DeviceAssigner assigner;

	InputDevice device;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			SceneManager.LoadScene (0);
		}

		if(assigner){
			device = assigner.GetPlayerDevice (0);
			if(device != null){
				if(device.Command.WasPressed){
					SceneManager.LoadScene (0);
				}
			}
			device = assigner.GetPlayerDevice (1);
			if(device != null){
				if(device.Command.WasPressed){
					SceneManager.LoadScene (0);
				}
			}
		}
	}
}
