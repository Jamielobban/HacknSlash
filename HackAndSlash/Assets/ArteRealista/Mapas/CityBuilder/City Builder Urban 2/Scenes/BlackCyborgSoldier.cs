using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCyborgSoldier :  Interactive, IInteractable
{
    [SerializeField] string[] dialogues;
    [SerializeField] SimpleRTVoiceExample voice;
    int currentDialogue = 0;

    public void Interact()
    {
        if (!canInteract) return;

        voice.Speak(dialogues[currentDialogue]);
    }

   
}
