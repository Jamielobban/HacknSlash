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
    private void DialogueDone() => OnDialogueLineDone?.Invoke(); //Aqui es ficara la funci� de color a verd
    private void ConversationEnded() => OnConversationEnded?.Invoke(); //Aqui es ficara la funci� de color a verd
    public void Speak(float volume = 0) => StartCoroutine(SpeakCoroutine(volume));
    
    IEnumerator SpeakCoroutine(float volume)
    {
        int copyCD = currentDialogue;
        int copyCDL = currentDialogueLine;

        ListWrapperDialogueElement textData = dialogues[copyCD].collection[copyCDL];

        Debug.Log(textData.elementKey);

        voice.Speak(textData.elementText, name, volume);

        if (textData.elementKey != -1)
            AudioManager.Instance.PlayDelayFx((Enums.Effects)(textData.elementKey), 0.3f);
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
                currentDialogueLine = 0;
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