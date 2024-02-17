using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCyborgSoldier : Interactive, IInteractable
{
    [SerializeField] string[] dialogues;
    [SerializeField] SimpleRTVoiceExample voice;
    [SerializeField] bool intro;
    int currentDialogue = 1;

    private void Start()
    {
        if (intro) StartCoroutine(IntroSpeach());
    }


    public void Interact()
    {
       // if (!canInteract) return;

        voice.Speak(dialogues[currentDialogue]);
    }

    IEnumerator IntroSpeach()
    {
        yield return new WaitForSeconds(1.5f);
        voice.Speak(dialogues[0]);
    }

}

