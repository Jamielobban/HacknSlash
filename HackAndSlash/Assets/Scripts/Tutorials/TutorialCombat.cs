using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCombat : MonoBehaviour
{
    [SerializeField] string[] dialoguesIntro;
    [SerializeField] string[] dialoguesItems;
    [SerializeField] string[] dialoguesChest;
    [SerializeField] string[] dialoguesEvent;
    [SerializeField] string[] endTutorial;


    [SerializeField] SimpleRTVoiceExample voice;
    public GameObject loadingGo;
    public Image fillLoadingGo;
    public GameObject eventActivable;
    public GameObject campEnemies;

    
    
    readonly string _name = "Cyborg Sergeant";

    private bool hasReadedItems = false;
    private bool hasReadedChest = false;
    private bool hasReadedEvents = false;
    private bool hasReadEndTutorial = false;

    private void Awake()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
        campEnemies.SetActive(false);
    }

    void Start()
    {
        AudioManager.Instance.PlayMusic(Enums.Music.MusicaCombbat);
        eventActivable.SetActive(false);
        StartCoroutine(Speak(dialoguesIntro));
    }

    void Update()
    {
        if(ManagerEnemies.Instance.CurrentGlobalTime >= 61 && !hasReadedItems)
        {
            hasReadedItems = true;
            StartCoroutine(Speak(dialoguesItems));
        }
        else if(ManagerEnemies.Instance.CurrentGlobalTime >= 82 && !hasReadedChest)
        {
            hasReadedChest = true;
            campEnemies.SetActive(true);
            StartCoroutine(Speak(dialoguesChest));
        }
        else if(ManagerEnemies.Instance.CurrentGlobalTime >= 100 && !hasReadedEvents && campEnemies.GetComponent<CampManager>().chest.isUnlocked)
        {
            hasReadedEvents = true;
            eventActivable.SetActive(true);
            StartCoroutine(Speak(dialoguesEvent));
            campEnemies.SetActive(false);
        }
        else if(eventActivable.activeSelf && !hasReadEndTutorial)
        {
            if(FindObjectOfType<EventMap>()?.CurrentEventState == Enums.EventState.FINISHED)
            {
                hasReadEndTutorial = true;
                Invoke(nameof(StartEnd), 1.5f);
                ManagerEnemies.Instance.isInEvent = true;
            }
        }
    }

    private void StartEnd()
    {
        GameManager.Instance.PauseGame();
        StartCoroutine(Speak(endTutorial));
        Invoke(nameof(TutorialFinished), 4f);
    }
    private void TutorialFinished()
    {
        GameManager.Instance.isTutorialCompleted = true;
        loadingGo.SetActive(true);
        Invoke(nameof(NextScene), 1f);
    }

    private void NextScene()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Playing);
        GameManager.Instance.LoadLevel(Constants.SCENE_MAIN, fillLoadingGo);
    }

    IEnumerator Speak(string[] _dialogues)
    {
        yield return new WaitForSecondsRealtime(.15f);

        for (int i = 0; i < _dialogues.Length; i++)
        {
            voice.Speak(_dialogues[i], _name, 1);
            yield return new WaitUntil(() => voice.playing == false);
            yield return new WaitForSecondsRealtime(0.8f);
        }
        if(GameManager.Instance.IsPaused)
        {
            GameManager.Instance.PauseGame();
        }
    }

}
