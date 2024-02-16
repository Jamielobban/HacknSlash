using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCyborgSoldier :  Interactive
{
    [SerializeField] string[] dialogues;
    [SerializeField] SimpleRTVoiceExample voice;
    int currentDialogue = 0;
    
    protected override void TriggerAction()
    {
        voice.Speak(dialogues[currentDialogue]);
    }

   
}
