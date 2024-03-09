using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCombat : MonoBehaviour
{
    [SerializeField] string[] dialoguesIntro;
    [SerializeField] string[] dialoguesItems;
    [SerializeField] string[] dialoguesEvent;
    [SerializeField] string[] endTutorial;


    [SerializeField] SimpleRTVoiceExample voice;
    public GameObject loadingGo;
    public Image fillLoadingGo;
    public GameObject eventActivable;
    
    readonly string name = "Cyborg Sergeant";

    private bool hasReadedItems = false;
    private bool hasReadedEvents = false;
    private bool hasReadEndTutorial = false;

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
            eventActivable.SetActive(true);
        }
        else if(eventActivable.activeSelf && !hasReadEndTutorial)
        {
            if(eventActivable.GetComponent<EventMap>()?.CurrentEventState == Enums.EventState.FINISHED)
            {
                hasReadEndTutorial = true;
                ManagerEnemies.Instance.isInEvent = true;
                GameManager.Instance.PauseGame();
                StartCoroutine(Speak(endTutorial));
                GameManager.Instance.isTutorialCompleted = true;
                loadingGo.SetActive(true);
                Invoke(nameof(NextScene), 1f);
            }
        }
    }

    private void NextScene()
    {
        GameManager.Instance.LoadLevel("DanielIceMap", fillLoadingGo);
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
