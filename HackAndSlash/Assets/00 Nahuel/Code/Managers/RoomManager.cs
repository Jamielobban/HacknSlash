using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{


    public Dictionary<Enemy, ObjectPool> enemyObjectsPools = new Dictionary<Enemy, ObjectPool>();

    public float scaleDivision = 4;
    public GameObject spawnerPrefab;
    public GameObject player;
    public int numStartEnemiesSpawn = 5;
    //Temporal public
    public List<GameObject> _enemiesToKill = new List<GameObject>();

    public bool firstEnemySpawned = false;
    public List<Enemy> enemies = new List<Enemy>();

    private int stageLevel = 0;
    private GameObject _spawnPoint;
    private int _currentEnemiesToSpawn;
    private static RoomManager _instance;

    public TextMeshProUGUI textStage, textRemaining;
    public static RoomManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<RoomManager>();
                if(_instance == null)
                {
                    GameObject go = new GameObject("Room Manager");
                    go.AddComponent<RoomManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    public float GetScaleFactor() => 1 + (stageLevel / scaleDivision);
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;

        _currentEnemiesToSpawn = numStartEnemiesSpawn;

        _spawnPoint = GameObject.Find("SpawnerInstantiatePoint");
        InitializePools();
       // ItemManager.instance.AddItemsToList();
    }
    private void Start()
    {
        NextStage();
    }
    private void Update()
    {
        if(AllEnemiesKilled && firstEnemySpawned)
        {
            NextStage();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            KillAllEnemies();
        }
    }

    private void InitializePools()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyObjectsPools.Add(enemies[i], ObjectPool.CreateInstance(enemies[i], _currentEnemiesToSpawn));
        }
    }

    private void GenerateStage()
    {
        stageLevel++;
        textStage.text = "Stage " + stageLevel;
    }

    private void NextStage()
    {
        stageLevel++;
        textStage.text = "Stage " + stageLevel;
        //if(stageLevel > 1)
        //{
        //    AbilityPowerManager.instance.itemChoice.SetActive(true);
        //    AbilityPowerManager.instance.ShowNewOptions();
        //}
        textStage.gameObject.GetComponent<TextMeshProFadeObject>().FadeIn();
        GenerateSpawner(0.3f, 0f); // Insta Spawner
        GenerateSpawner(0.7f, 1f); // Time Spawner
        _currentEnemiesToSpawn += 2;
    }

    private void GenerateSpawner(float spawnPercentage, float spawnDelay)
    {
        GameObject go = Instantiate(spawnerPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
        SpawnerBase _spawn = go.GetComponent<SpawnerBase>();
        _spawn.EnemiesToSpawn = Mathf.RoundToInt(_currentEnemiesToSpawn * spawnPercentage);
        _spawn.TimeToSpawn = spawnDelay;
        _spawn.IsBurstSpawner = true;
        go.SetActive(true);
    }

    private bool AllEnemiesKilled => _enemiesToKill.Count == 0;

    public void AddEnemy(GameObject enemy)
    {
        if (!_enemiesToKill.Contains(enemy))
        {
            _enemiesToKill.Add(enemy);
        }
        UpdateRemainingEnemiesHUD(_enemiesToKill.Count);
    }
    private void KillAllEnemies()
    {
        foreach (var enemy in _enemiesToKill)
        {
            enemy.SetActive(false);
            enemy.GetComponent<Enemy>().ResetEnemy();
        }
        _enemiesToKill.Clear();
    }
    public void RemoveEnemy(GameObject enemy)
    {
        if(_enemiesToKill.Contains(enemy))
        {
            _enemiesToKill.Remove(enemy);
        }
        UpdateRemainingEnemiesHUD(_enemiesToKill.Count);
    }

    private void UpdateRemainingEnemiesHUD(int value)
    {
        textRemaining.text = ("Enemies Remaining: " + value);
    }

    public void ResetRoomManager()
    {
        stageLevel = 0;
        foreach (var enemy in _enemiesToKill)
        {
            enemy.SetActive(false);
            enemy.GetComponent<Enemy>().ResetEnemy();
        }
        _enemiesToKill.Clear();
        firstEnemySpawned = false;
        NextStage();
    }
}
