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
    
    #region Settings
    [Header("Texts Settings:")]
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI currentSpawnedEnemies;
    public TextMeshProUGUI currentScoreText;

    [Header("Spawn Settings:")]
    public List<GameObject> spawners = new List<GameObject>();
    public bool lastSpawnerLastOne = false;

    [Header("Enemies Settings: ")]
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public Dictionary<EnemyBase, ObjectPool> enemyObjectsPools = new Dictionary<EnemyBase, ObjectPool>();
    public List<GameObject> parentObjectPools = new List<GameObject>(); 

    [Header("Scaling Settings: ")]
    public float maxMultiplierLife = 30f;
    public float maxMultiplierAttack = 1.8f;
    public float timeToReachMax = 1800f;
    public int timeToGetItem = 60;
    public int timeToEndGame;
    public bool isInEvent = false;

    private GameObject _currentSpawner = null;
    private InfiniteSpawner _currentSpawnerScript = null;
    private int _currentSpawnerIndex = 0;
    private int _spawnedEnemies = 0;
    private float _enemiesScore = 0;
    
    private float _timerGlobal = 0f;
    private float _timerItems = 0f;
    private float _timerSpawners = 0f;
    private int minutes = 0;
    private int seconds = 0;
    
    private float _scaleLifeMultiplier = 0f;
    private float _scaleDamageMultiplier = 0f;

    public float ScaleLifeMult => _scaleLifeMultiplier;
    public float ScaleDamageMult => _scaleDamageMultiplier;
    #endregion
    
    public int SpawnedEnemies => _spawnedEnemies;
    public float CurrentGlobalTime => _timerGlobal;

    private void Awake()
    {
        _instance = this;
        InitializePools();
        InitializeNewSpawner();
    }

    void Update()
    {
        _timerGlobal += Time.deltaTime;
        _timerItems += Time.deltaTime;
        _timerSpawners += Time.deltaTime;
        
        UpdateTimeText();

        if (_timerItems >= timeToGetItem)
        {
            _timerItems = 0f;
            UpgradeEnemies();
            ItemsLootBoxManager.Instance.ShowNewOptions();
            ResetScore();
        }

        if (_timerSpawners >= _currentSpawnerScript.lifeTime)
        {
            if (spawners.Count - 1 > _currentSpawnerIndex)
            {
                NextSpawner();
            }
            else if(spawners.Count - 1 <= _currentSpawnerIndex && lastSpawnerLastOne)
            {
                if(_currentSpawner != null)
                {
                    ResetSpawnedEnemies();
                    UpdateEnemiesSpawned();
                    Destroy(_currentSpawner);
                }
            }
            _timerSpawners = 0f;
        }

        if(_timerGlobal >= timeToEndGame)
        {
            //Open Boss Mode
        }

    }

    private void UpgradeEnemies()
    {
        float lerpFactor = Mathf.Clamp01(_timerGlobal / timeToReachMax);
        _scaleLifeMultiplier = Mathf.Lerp(1f, maxMultiplierLife, lerpFactor);
        _scaleDamageMultiplier = Mathf.Lerp(1, maxMultiplierAttack, lerpFactor);
        foreach (var pool in parentObjectPools)
        {
            for (int i = 0; i < pool.transform.childCount; i++)
            {
                GameObject enemy = pool.transform.GetChild(i).gameObject;
                if (enemy.activeSelf)
                {
                    enemy.GetComponent<EnemyBase>().UpgradeEnemy(_scaleLifeMultiplier, _scaleDamageMultiplier);
                }
            }
        }
    }
    
    public void StartEvent()
    {
        isInEvent = true;
        if(_currentSpawner != null)
        {
            _currentSpawner.GetComponent<InfiniteSpawner>().ClearAllEnemiesSpawned();
            ResetSpawnedEnemies();
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


    public bool IsMaxScore() => _enemiesScore >= GameManager.Instance.Player.hud.maxScore;


    public void UpdateScore()
    {
        currentScoreText.text = "Score: " + _enemiesScore;
        GameManager.Instance.Player.hud.UpdateProgressScoreBar(_enemiesScore);
    }

    private void InitializePools()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyObjectsPools.Add(enemies[i], ObjectPool.CreateInstance(enemies[i], 0));
        }
    }

    public void ResetScore()
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

    public void AddSpawnedEnemies(int val)
    {
        _spawnedEnemies += val;
        if (_spawnedEnemies <= 0)
        {
            _spawnedEnemies = 0;
        }
        UpdateEnemiesSpawned();
    }

    private void ResetSpawnedEnemies() => _spawnedEnemies = 0;

    public void NextSpawner()
    {
        ResetSpawnedEnemies();
        UpdateEnemiesSpawned();
        
        Destroy(_currentSpawner);
        _currentSpawnerIndex++;
        InitializeNewSpawner();
    }

    private void InitializeNewSpawner()
    {
        _currentSpawner = Instantiate(spawners[_currentSpawnerIndex]);
        _currentSpawnerScript = spawners[_currentSpawnerIndex].GetComponent<InfiniteSpawner>();
        _currentSpawner.transform.parent = GameManager.Instance.Player.transform;
        _currentSpawner.transform.localRotation = Quaternion.identity;
        _currentSpawner.transform.localPosition = Vector3.zero;
        _currentSpawner.transform.localScale = Vector3.one;
    }

}
