using Crosstales.RTVoice.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    public void Speak(float volume = 1) => StartCoroutine(SpeakCoroutine(volume));
    
    IEnumerator SpeakCoroutine(float volume)
    {
        ListWrapperDialogueElement textData = dialogues[currentDialogue].collection[currentDialogueLine];
        voice.Speak(textData.elementText, name, volume);
        if (textData.elementKey != -1)
            AudioManager.Instance.PlayFx((Enums.Effects)(textData.elementKey));
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
            Speak(volume);
        }

    }
    public void Interact()
    {
        if (!GetCanInteract) return;

        InteractPerformed();

        SetCanInteract(false);
    }
}