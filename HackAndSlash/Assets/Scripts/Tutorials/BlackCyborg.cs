using Crosstales.RTVoice.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackCyborg : Interactive, IInteractable
{
    [Header("DIALOGUE AREA")]

    [SerializeField] List<ListWrapperDialogue> dialogues;
    [SerializeField] SimpleRTVoiceExample voice;

    int currentDialogue = 0;
    int currentDialogueLine = 0;

    public Action OnDialogueLineDone;
    public Action OnDialogueEnded;
    public Action OnConversationEnded;
    private void DialogueDone() => OnDialogueLineDone?.Invoke(); //Aqui es ficara la funció de color a verd
    private void ConversationEnded() => OnConversationEnded?.Invoke(); //Aqui es ficara la funció de color a verd
    public void Speak() => StartCoroutine(SpeakCoroutine());

    IEnumerator SpeakCoroutine()
    {
        voice.Speak(dialogues[currentDialogue].collection[currentDialogueLine], name);
        yield return new WaitUntil(() => voice.playing == false);

        if (currentDialogueLine == dialogues[currentDialogue].collection.Count - 1)
        {
            if (currentDialogue == dialogues.Count - 1)
            {
                ConversationEnded();
            }
            else
            {
                DialogueDone();
                currentDialogue++;
            }
        }
        else
        {
            currentDialogueLine++;
            yield return new WaitForSeconds(0.5f);
            Speak();
        }

    }
    public void Interact()
    {
        if (!GetCanInteract) return;

        InteractPerformed();

        SetCanInteract(false);
    }


}