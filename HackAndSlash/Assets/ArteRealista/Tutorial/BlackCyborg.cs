using Crosstales.RTVoice.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackCyborg : Interactive, IInteractable
{
    [Header("DIALOGUE AREA")]

    [TextArea]
    [SerializeField] string[] dialogues;
    [SerializeField] SimpleRTVoiceExample voice;

    int currentDialogue = 0;
    
    public Action OnDialogueLineDone;
    public void NextDialogue() => currentDialogue++;    
    private void DialogueLineDone() => OnDialogueLineDone?.Invoke(); //Aqui es ficara la funció de color a verd
    public void Speak() => StartCoroutine(SpeakCoroutine());     

    IEnumerator SpeakCoroutine()
    {
        voice.Speak(dialogues[currentDialogue], name);
        yield return new WaitUntil(() => voice.playing == false);
        DialogueLineDone();
    }  
    public void Interact()
    {
        if (!canInteract) return;

        InteractPerformed();
    }
}
