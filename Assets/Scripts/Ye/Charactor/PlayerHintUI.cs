using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = UnityEngine.UI.Text;

public class PlayerHintUI : MonoBehaviour {

    public GameObject target;

    public enum HintStatus { A, B, C, None};

    public HintStatus hint = HintStatus.None;

    public Camera cam ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cam.WorldToScreenPoint(target.transform.position);
        if (hint == HintStatus.None)
        {
            GetComponent<Text>().text = "";
        }
        else if (hint == HintStatus.A)
        {
            GetComponent<Text>().text = " A ";
        }
        else if (hint == HintStatus.B)
        {
            GetComponent<Text>().text = " B ";
        }
        else if (hint == HintStatus.C)
        {
            GetComponent<Text>().text = " C ";
        }
    }
}
