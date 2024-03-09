using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCombat : MonoBehaviour
{
    [SerializeField] string[] dialoguesIntro;
    [SerializeField] string[] dialoguesItems;
    [SerializeField] string[] dialoguesEvent;


    [SerializeField] SimpleRTVoiceExample voice;

    public GameObject eventActivable;
    
    readonly string name = "Cyborg Sergeant";

    private bool hasReadedItems = false;
    private bool hasReadedEvents = false;

    private void Awake()
    {
    }

    void Start()
    {
        eventActivable.SetActive(false);
        GameManager.Instance.PauseGame();
        StartCoroutine(Speak(dialoguesIntro));
    }

    void Update()
    {
      //  voice.Speak(dialogues[currentText], name);
        if(ManagerEnemies.Instance.CurrentGlobalTime >= 62 && !hasReadedItems)
        {
            hasReadedItems = true;
            GameManager.Instance.PauseGame();
            StartCoroutine(Speak(dialoguesItems));
        }
        else if(ManagerEnemies.Instance.CurrentGlobalTime >= 80 && !hasReadedEvents)
        {
            hasReadedEvents = true;
            GameManager.Instance.PauseGame();
            StartCoroutine(Speak(dialoguesEvent));
        }
    }

    IEnumerator Speak(string[] _dialogues)
    {
        yield return new WaitForSecondsRealtime(.75f);

        for (int i = 0; i < _dialogues.Length; i++)
        {
            voice.Speak(_dialogues[i], name);
            yield return new WaitUntil(() => voice.playing == false);
            yield return new WaitForSecondsRealtime(0.8f);
        }
        GameManager.Instance.PauseGame();
    }

}
