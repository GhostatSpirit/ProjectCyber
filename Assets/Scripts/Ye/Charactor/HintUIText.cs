using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Text = UnityEngine.UI.Text;

public class HintUIText : MonoBehaviour {

    Text text;

    public string HintStringA = "Press A";
    public string HintStringB = "Press B";
    public string HintStringC = "Press C";
    public string HintStringNone = "None";

    [Flags] public enum HintStatus { None = 0, A = 1 , B = 2, C = 4 };

    public void Add(ref HintStatus hs, HintStatus[] _hsArray)
    {
        foreach( HintStatus _hs in _hsArray)
        {
            hs = hs | _hs;
        }
    }

    public void Remove(ref HintStatus hs, HintStatus[] _hsArray)
    {
        foreach( HintStatus _hs in _hsArray)
        {
            hs = hs & ~_hs;
        }
    }


    

    public HintStatus status = HintStatus.None;

    // Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        // Add(ref status, new HintStatus[] { HintStatus.A, HintStatus.B });
    }

	// Update is called once per frame
	void Update ()
    {

        Debug.Log(status);

        
        switch (status)
        {
            case HintStatus.A:
                text.text = HintStringA;
                break;
            case HintStatus.B:
                text.text = HintStringB;
                break;
            case HintStatus.C:
                text.text = HintStringC;
                break;
            case HintStatus.None:
                text.text = HintStringNone;
                break;
        }
        
    }
}
