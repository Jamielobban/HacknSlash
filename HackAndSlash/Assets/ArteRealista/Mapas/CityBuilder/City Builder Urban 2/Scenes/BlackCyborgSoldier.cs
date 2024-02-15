using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCyborgSoldier : MonoBehaviour, IInteractable
{
    [SerializeField] string[] dialogues;
    [SerializeField] SimpleRTVoiceExample voice;
    int currentDialogue = 0;
    [SerializeField] Material triggerMat, normalMat;
    
    public void Interact()
    {
        voice.Speak(dialogues[currentDialogue]);
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponentInChildren<Renderer>().material = triggerMat;
    }
    private void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<Renderer>().material = normalMat;
    }
}
