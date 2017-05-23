using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UI_Pointer : MonoBehaviour {

    public float velocity = 8f; 
    
    float x = 0;
    float y = -1;
    float z = 0;


    Vector3 moveVector;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame


    void Update () {

        x = Mathf.Cos( (-90 - GetComponentsInParent<Image>()[1].fillAmount * 360) * Mathf.Deg2Rad);
        y = Mathf.Sin( (-90 - GetComponentsInParent<Image>()[1].fillAmount * 360) * Mathf.Deg2Rad);


        moveVector = new Vector3(x, y, z);
        var currentRot = Quaternion.LookRotation(new Vector3(0,0,1), moveVector);
        var newRot = Quaternion.Lerp(transform.rotation, currentRot,Time.deltaTime * velocity);

        transform.rotation = newRot;
    }
}
