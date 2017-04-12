using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingHint : MonoBehaviour {

    public GameObject HackerHint;

    public GameObject AIHint;

    ControlStatus CS;

	// Use this for initialization
	void Start () {
        CS = GetComponent<ControlStatus>();	
	}
	
	// Update is called once per frame
	void Update () {
	    CS = GetComponent<ControlStatus>();
        Debug.Log(CS.controller);

        if(CS.controller == Controller.Boss)
        {
            AIHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.PressA ;
        }

        if (CS.controller == Controller.None)
        {
            AIHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.None;
            HackerHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.PressA;
        }

        if (CS.controller == Controller.Hacker)
        {
            HackerHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.None;
            GetComponent<TrainingHint>().enabled = false;
        }
    }
}
