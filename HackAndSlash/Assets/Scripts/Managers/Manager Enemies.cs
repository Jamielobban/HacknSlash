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

    [Header("Enemies Settings: ")]
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public Dictionary<EnemyBase, ObjectPool> enemyObjectsPools = new Dictionary<EnemyBase, ObjectPool>();
    public List<GameObject> parentObjectPools = new List<GameObject>(); 

    [Header("Scaling Settings: ")]
    public float maxMultiplierLife = 30f;
    public float maxMultiplierAttack = 1.8f;
    public float timeToReachMax = 1800f;
    public int timeToGetItem = 60;
    
    public bool isInEvent = false;

    private GameObject _currentSpawner = null;
    private InfiniteSpawner _currentSpawnerScript = null;
    private int _currentSpawnerIndex = 0;
    private int _spawnedEnemies = 0;
    private float _enemiesScore = 0;
    
    private float _timerGlobal = 0f;
    private float _timerItems = 0f;
    private int minutes = 0;
    private int seconds = 0;
    
    private float _scaleLifeMultiplier = 0f;
    private float _scaleDamageMultiplier = 0f;
    #endregion
    
    public int SpawnedEnemies => _spawnedEnemies;
    public float CurrentGlobalTime => _timerGlobal;

    private void Awake()
    {
        _instance = this;
        InitializePools();
        _currentSpawner = Instantiate(spawners[_currentSpawnerIndex]);
        _currentSpawnerScript = spawners[_currentSpawnerIndex].GetComponent<InfiniteSpawner>();
        _currentSpawner.transform.parent = GameManager.Instance.Player.transform;
    }

    void Update()
    {
        _timerGlobal += Time.deltaTime;
        _timerItems += Time.deltaTime;
        UpdateTimeText();

        if (_timerItems >= timeToGetItem)
        {
            _timerItems = 0f;
            UpgradeEnemies();
            AbilityPowerManager.instance.ShowNewOptions();
            ResetScore();
        }

        if (_timerGlobal >= _currentSpawnerScript.lifeTime)
        {
            if (spawners.Count - 1 > _currentSpawnerIndex)
            {
                NextSpawner();
            }
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
        GameManager.Instance.Player.hud.UpdateProgressScoreBar(_enemiesScore);
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

    public void AddSpawnedEnemies(int val)
    {
        _spawnedEnemies += val;
        UpdateEnemiesSpawned();
    }

    private void ResetSpawnedEnemies() => _spawnedEnemies = 0;

    public void NextSpawner()
    {
        ResetSpawnedEnemies();
        UpdateEnemiesSpawned();
        
        Destroy(_currentSpawner);
        _currentSpawnerIndex++;
        _currentSpawner = Instantiate(spawners[_currentSpawnerIndex]);
        _currentSpawnerScript = spawners[_currentSpawnerIndex].GetComponent<InfiniteSpawner>();
        _currentSpawner.transform.parent = GameManager.Instance.Player.transform;
    }

}
