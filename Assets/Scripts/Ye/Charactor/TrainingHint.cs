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

		ChangeBossHint ();

		CS.OnCutByPlayer += ChangeNoneHint;
		CS.OnLinkedByPlayer += ChangeHackerHint;
	}
	
	// Update is called once per frame
	void Update () {
	    CS = GetComponent<ControlStatus>();
        // Debug.Log(CS.controller);
    }

	void ChangeBossHint(){
		AIHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.PressA ;
	}

	void ChangeNoneHint(Transform objTrans){
		AIHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.None;
		HackerHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.PressA;
	}

	void ChangeHackerHint(Transform objTrans){
		HackerHint.GetComponent<PlayerHintUI>().hint = PlayerHintUI.HintStatus.None;
		GetComponent<TrainingHint>().enabled = false;
	}
}
