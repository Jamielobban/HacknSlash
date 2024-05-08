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
    public GameObject campEnemies;        

    private bool hasReadedItems = false;
    private bool hasReadedChest = false;
    private bool hasReadedEvents = false;
    private bool hasReadEndTutorial = false;
    private bool hasReadCombos = false;

    private void Awake()
    {
        campEnemies.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
        AudioManager.Instance.PlayMusic(Enums.Music.MusicaCombbat);
        bc.Speak();
    }

    void SpeakInTime() => bc.Speak();

    void Update()
    {
        if(LevelManager.Instance.CurrentGlobalTime >= 20 && !hasReadCombos)
        {
            hasReadCombos = true;
            bc.Speak();
        }
        else if(LevelManager.Instance.CurrentGlobalTime >= 61 && !hasReadedItems)
        {
            hasReadedItems = true;
            bc.Speak();
        }
        else if(LevelManager.Instance.CurrentGlobalTime >= 80 && !hasReadedChest)
        {
            hasReadedChest = true;
            campEnemies.SetActive(true);
            bc.Speak();
        }
        else if(LevelManager.Instance.CurrentGlobalTime >= 100 && !hasReadedEvents && campEnemies.GetComponent<CampManager>().chest.isUnlocked)
        {
            hasReadedEvents = true;
            Invoke(nameof(SpeakInTime), 1);
            campEnemies.SetActive(false);
        }
        else if(!hasReadEndTutorial && LevelManager.Instance.CurrentGlobalTime >= 130)
        {
            hasReadEndTutorial = true;
            Invoke(nameof(StartEnd), 1.5f);
            LevelManager.Instance.isInEvent = true;
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
        GameManager.Instance.Player.inventory.ClearInventory();
        GameManager.Instance.UpdateState(Enums.GameState.StartPlaying);

        GameManager.Instance.LoadLevel(Constants.SCENE_MAIN, fillLoadingGo);
    }   

}
