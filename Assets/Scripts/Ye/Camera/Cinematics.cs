using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class Cinematics : MonoBehaviour {

    ControlStatus CS;
    public Camera cam;
    public Transform hacker;
    public Transform ai;
    public GameObject target;

    // Use this for initialization
    void Start () {
        CS = GetComponent<ControlStatus>();
        CS.OnLinkedByPlayer += CameraBehaviour;
    }
	
	// Update is called once per frame
	void Update () {

    }

    void CameraBehaviour(Transform trans)
    {
        hacker.GetComponent<PlayerControl>().canControl = false;
        ai.GetComponent<PlayerControl>().canControl = false;
        hacker.GetComponent<HealthSystem>().StartImmune();
        ai.GetComponent<HealthSystem>().StartImmune();

        ProCamera2DCinematics proCam = cam.GetComponent<ProCamera2DCinematics>();
        proCam.AddCinematicTarget( target.transform, 0f); 

        proCam.Play();
        proCam.OnCinematicFinished.AddListener(StartMovement);
        proCam.OnCinematicFinished.AddListener(EndImmune);
        proCam.RemoveCinematicTarget(target.transform);
        CS.OnLinkedByPlayer -= CameraBehaviour;
    }

    void StartMovement()
    {
        hacker.GetComponent<PlayerControl>().canControl = true;
        ai.GetComponent<PlayerControl>().canControl = true;
    }

    void EndImmune()
    {
        hacker.GetComponent<HealthSystem>().EndImmune();
        ai.GetComponent<HealthSystem>().EndImmune();
    }
}
