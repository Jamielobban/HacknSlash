using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCombat : MonoBehaviour
{    
    [SerializeField] BlackCyborg bc;

    [SerializeField] SimpleRTVoiceExample voice;
    public GameObject loadingGo;
    public Image fillLoadingGo;
    public GameObject eventActivable;
    public GameObject campEnemies;        

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
        bc.Speak();
    }

    void SpeakInTime() => bc.Speak();

    void Update()
    {
        if(ManagerEnemies.Instance.CurrentGlobalTime >= 61 && !hasReadedItems)
        {
            hasReadedItems = true;
            bc.Speak();
        }
        else if(ManagerEnemies.Instance.CurrentGlobalTime >= 80 && !hasReadedChest)
        {
            hasReadedChest = true;
            campEnemies.SetActive(true);
            bc.Speak();
        }
        else if(ManagerEnemies.Instance.CurrentGlobalTime >= 100 && !hasReadedEvents && campEnemies.GetComponent<CampManager>().chest.isUnlocked)
        {
            hasReadedEvents = true;
            eventActivable.SetActive(true);
            Invoke(nameof(SpeakInTime), 1);
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
        //GameManager.Instance.PauseGame();
        Invoke(nameof(SpeakInTime), 1);
        Invoke(nameof(TutorialFinished), 9.5f);
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

}
