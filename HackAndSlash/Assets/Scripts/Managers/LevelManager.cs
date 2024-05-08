using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Level Manager");
                go.AddComponent<LevelManager>();
            }
            return _instance;
        }
    }

    public TextMeshProUGUI textTime;
    public bool isInEvent = false;
    public int timeToGetItem = 60;
    public int timeToEndGame;

    private float _timerGlobal = 0f;
    private float _timerItems = 0f;

    private ManagerEnemies _enemiesManager;
    private ScoreManager _scoreManager;
    public bool isTutorial;
    public bool isWin = false;

    // -- GETTERS -- //
    public ManagerEnemies EnemiesManager => _enemiesManager;
    public ScoreManager ScoreManager => _scoreManager;
    public float CurrentGlobalTime => _timerGlobal;

    private void Awake()
    {
        _instance = this;
        _enemiesManager = GetComponent<ManagerEnemies>();
        _scoreManager = GetComponent<ScoreManager>();
        _enemiesManager.Initialize();
        if(!isTutorial)
            GameManager.Instance.UpdateState(Enums.GameState.Playing); // Starts Play
    }

    void Start()
    {
    }

    void Update()
    {
        _timerGlobal += Time.deltaTime;
        _timerItems += Time.deltaTime;

        UpdateTimeText();


        if (_timerItems >= timeToGetItem)
        {
            _timerItems = 0f;
            _enemiesManager.UpgradeEnemies(_timerGlobal);
        }

        _enemiesManager.HandleEnemiesUpdate();

        if (_timerGlobal >= timeToEndGame && !isWin)
        {
            isWin = true;
            FindObjectOfType<WinInteractable>().OnWin();
        }
    }
    public void StartEvent()
    {
        isInEvent = true;
        _enemiesManager.ClearEnemies();
    }

    public void EndEvent()
    {
        isInEvent = false;
        _enemiesManager.ResetSpawnedEnemies();
    }

    private void UpdateTimeText()
    {
        int minutes = (int)(_timerGlobal / 60f);
        int seconds = (int)(_timerGlobal % 60f);
        textTime.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
