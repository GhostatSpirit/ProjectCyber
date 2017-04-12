using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = UnityEngine.UI.Text;
using Image = UnityEngine.UI.Image;

public class PlayerHintUI : MonoBehaviour {

    public GameObject target;

    public Sprite None;

    public Sprite pressAsprite;

    public Sprite pressBsprite;

    public float moveX;

    public enum HintStatus { PressA, PressB, None };

    public HintStatus hint = HintStatus.None;

    public Camera cam ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cam.WorldToScreenPoint(target.transform.position);
        transform.position += new Vector3(moveX,0,0);

        if (hint == HintStatus.None)
        {
            GetComponent<Image>().sprite = None;
        }
        else if (hint == HintStatus.PressA)
        {
            GetComponent<Image>().sprite = pressAsprite;
        }
        else if (hint == HintStatus.PressB)
        {
            GetComponent<Image>().sprite = pressBsprite;
        }
        
    }
}
