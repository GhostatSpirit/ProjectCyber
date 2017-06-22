using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using DG.Tweening;

public class FinalDialogueSystem : MonoBehaviour {

    public DeviceAssigner assigner;

    public Image[] dialogues;

    public Image continuePrompt;

    public Image thankPrompt;

    public GameObject finalButton;

    public Animator brainAnimator;

    public float showBrainDelay = 0.5f;

    public float initialDelay = 0.5f;
    public float switchDelay = 0.5f;

    public float thankDelay = 2f;

    InputDevice device0;
    InputDevice device1;

    AudioSource audioSource;


	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
		if(dialogues == null)
        {
            dialogues = new Image[0];
        }
        foreach(Image tempImage in dialogues)
        {
            tempImage.enabled = false;
        }
        continuePrompt.enabled = false;
        thankPrompt.enabled = false;
        finalButton.SetActive(false);

        StartCoroutine(DialogueIE());

        brainAnimator.enabled = false;
	}

    IEnumerator DialogueIE()
    {
        yield return new WaitForSeconds(showBrainDelay);
        brainAnimator.enabled = true;

        yield return new WaitForSeconds(initialDelay);
        
        for(int i = 0; i < dialogues.Length; ++i)
        {
            Image tempImage = dialogues[i];
            if (tempImage == null) continue;

            SetTransparentAndEnable(tempImage);

            tempImage.DOFade(1f, switchDelay);
            yield return new WaitForSeconds(switchDelay);
            continuePrompt.enabled = true;

            yield return new WaitUntil(() => buttonPressed == true);
            audioSource.Play();
            continuePrompt.enabled = false;

            tempImage.DOFade(0f, switchDelay);
            yield return new WaitForSeconds(switchDelay);
            tempImage.enabled = false;
        }

        SetTransparentAndEnable(thankPrompt);
        thankPrompt.DOFade(1f, thankDelay);
        yield return new WaitForSeconds(thankDelay);

        finalButton.SetActive(true);
        yield return null;
        
    }
	
	// Update is called once per frame
	void Update () {
        device0 = assigner.GetPlayerDevice(0);
        device1 = assigner.GetPlayerDevice(1);
	}

    bool buttonPressed
    {
        get
        {
            bool pressed = false;
            if (device0 != null && device0.Action1.WasPressed)
                pressed = true;
            if (device1 != null && device1.Action1.WasPressed)
                pressed = true;

            return pressed;
        }
    }

    void SetTransparentAndEnable(Image tempImage)
    {
        tempImage.enabled = true;

        Color tempColor = tempImage.color;
        tempColor.a = 0f;
        tempImage.color = tempColor;
    }

}
