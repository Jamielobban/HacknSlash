using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCyborgSoldier : Interactive, IInteractable
{
    [TextArea]
    [SerializeField] string[] dialogues;
    [SerializeField] SimpleRTVoiceExample voice;
    [SerializeField] bool intro;
    int currentDialogue = 0;
    readonly string name = "Cyborg Sergeant";

    private void Start()
    {
        if (intro) StartCoroutine(IntroSpeach());
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.H))
        {
            Interact();
        }
    }


    public void Interact()
    {
       // if (!canInteract) return;

        voice.Speak(dialogues[currentDialogue], name);
    }

    IEnumerator IntroSpeach()
    {
        yield return new WaitForSeconds(1.5f);
        voice.Speak(dialogues[0], name);
    }

}

