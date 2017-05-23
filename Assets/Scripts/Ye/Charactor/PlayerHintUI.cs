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

    public Sprite pressYsprite;

	public Sprite pressB2sprite;

    public float moveX;


    public enum HintStatus { PressA, PressB, PressY ,None , PressB2};

    public HintStatus hint = HintStatus.None;

    public Camera cam ;

    Image image;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cam.WorldToScreenPoint(target.transform.position);
        transform.position += new Vector3(moveX,0,0);

        if (hint == HintStatus.None)
        {
            image.sprite = None;
        }
        else if (hint == HintStatus.PressA)
        {
            image.sprite = pressAsprite;
        }
        else if (hint == HintStatus.PressB)
        {
            image.sprite = pressBsprite;
        }
        else if (hint == HintStatus.PressY)
        {
            image.sprite = pressYsprite;
        }
		else if (hint == HintStatus.PressB2){
			if(pressB2sprite){
				image.sprite = pressB2sprite;
			}
		}
    }
}
