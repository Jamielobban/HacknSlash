using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerEnemies : MonoBehaviour
{
    private static ManagerEnemies _instance;

    public static ManagerEnemies Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("Enemies Manager");
                go.AddComponent<ManagerEnemies>();
            }
            return _instance;
        }
    }
    
    private float _enemiesScore = 0;

    public int timeToGetItem = 60;

    #region Settings
    [Header("Texts Settings:")]
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI currentSpawnedEnemies;
    public TextMeshProUGUI currentScoreText;

    [Header("Spawn Settings:")]
    public List<GameObject> spawners = new List<GameObject>();

    [Header("Enemies Settings: ")]
    public List<Enemy> enemies = new List<Enemy>();
    public Dictionary<Enemy, ObjectPool> enemyObjectsPools = new Dictionary<Enemy, ObjectPool>();
    public List<GameObject> parentObjectPools = new List<GameObject>(); 

    private int _spawnedEnemies = 0;
    public int SpawnedEnemies => _spawnedEnemies;

    // -- Timer Settings -- //
    private float _timerGlobal = 0f;
    public float CurrentGlobalTime => _timerGlobal;

    private float _timerItems = 0f;
    private int minutes = 0;
    private int seconds = 0;
    public bool isInEvent = false;
    private GameObject _currentSpawner = null;

    private int _currentSpawnerIndex = 0;

    // -- Scaling Propierties -- //
    public float scaleLifeMultiplier = 0f;
    public float scaleDamageMultiplier = 0f;

    #endregion

    public float maxMultiplierLife = 30f;
    public float maxMultiplierAttack = 1.8f;
    public float timeToReachMax = 1800f;

    private void Awake()
    {
        _instance = this;
        InitializePools();
        _currentSpawner = Instantiate(spawners[_currentSpawnerIndex]);
    }

    void Update()
    {
        _timerGlobal += Time.deltaTime;
        _timerItems += Time.deltaTime;
        UpdateTimeText();

        if (_timerItems >= timeToGetItem)
        {
            _timerItems = 0f;

            float lerpFactor = Mathf.Clamp01(_timerGlobal / timeToReachMax);
            scaleLifeMultiplier = Mathf.Lerp(1f, maxMultiplierLife, lerpFactor);
            scaleDamageMultiplier = Mathf.Lerp(1, maxMultiplierAttack, lerpFactor);
            //Take All enemies and Upgrade them (?)
            foreach (var pool in parentObjectPools)
            {
                for (int i = 0; i < pool.transform.childCount; i++)
                {
                    GameObject enemy = pool.transform.GetChild(i).gameObject;
                    if (enemy.activeSelf)
                    {
                        enemy.GetComponent<Enemy>().UpgradeEnemy(scaleLifeMultiplier, scaleDamageMultiplier);
                    }
                }
            }

            AbilityPowerManager.instance.ShowNewOptions();
            ResetScore();
        }
    }

    public void StartEvent()
    {
        isInEvent = true;
        if(_currentSpawner != null)
        {
           // _currentSpawner.GetComponent<InfiniteSpawner>().ClearAllEnemiesSpawned();
        }
    }

    public void EndEvent() => isInEvent = false;
    

    private void UpdateTimeText()
    {
        minutes = (int)(_timerGlobal / 60f);
        seconds = (int)(_timerGlobal % 60f);
        textTime.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void UpdateEnemiesSpawned() => currentSpawnedEnemies.text = "Spawned Enemies:" + _spawnedEnemies;

    public void UpdateScore()
    {
        currentScoreText.text = "Score: " + _enemiesScore;
        GameManager.Instance.Player.hud.UpdateProgressScoreBar(_enemiesScore, 1500);
    }

    private void InitializePools()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyObjectsPools.Add(enemies[i], ObjectPool.CreateInstance(enemies[i], 1));
        }
    }

    private void ResetScore()
    {
        _enemiesScore = 0;
        GameManager.Instance.Player.hud.ResetProgressScoreBar();
        UpdateScore();
    }

    public void AddEnemyScore(float val)
    {
        _enemiesScore += val;
        UpdateScore();
    }

    public void SetSpawnedEnemies(int val)
    {
        _spawnedEnemies += val;
        UpdateEnemiesSpawned();
    }

    public void NextSpawner()
    {
        Destroy(_currentSpawner);
        _currentSpawnerIndex++;
        _currentSpawner = Instantiate(spawners[_currentSpawnerIndex]);
    }

}
