using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UI_Pointer : MonoBehaviour {

    public float x = 0;
    public float y = 0;
    float z = 0;

    Vector3 moveVector;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame


    void Update () {

        //transform.Rotate(new Vector3(0,0,30));
        // Debug.Log(GetComponentInParent<Image>().fillAmount);
        Debug.Log(Mathf.Cos(Mathf.PI));
        Debug.Log(Mathf.Sin(Mathf.PI));
        x = Mathf.Cos( Mathf.PI/2f);
        y = Mathf.Sin( GetComponentInParent<Image>().fillAmount * Mathf.PI);

        moveVector = new Vector3(x, y, z);
        // Mathf.Sin(GetComponentInParent<Image>().fillAmount * 360),Mathf.Cos(GetComponentInParent<Image>().fillAmount * 360) ,0);
        var currentRot = Quaternion.LookRotation(new Vector3(0,0,1), moveVector);
        var newRot = Quaternion.Lerp(transform.rotation, currentRot,
            Time.deltaTime);

        transform.rotation = newRot;
    }
}
