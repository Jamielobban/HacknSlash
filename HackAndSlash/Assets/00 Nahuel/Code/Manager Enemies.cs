using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    //-//-//-//-//-//
    // Timer -> every X time object with chance by enemiesKilled and enemiesPoints acumulated in the time period
    // Spawning enemies all the time each X seg, with enemies cap
    // If enemies are to far from player deactive and respawn
    //-//-//-//-//-//

    private float _enemiesKilled = 0;
    private float _enemiesScore = 0;

    public int timeToGetItem = 2;
    #region Settings
    [Header("Texts Settings:")]
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI currentSpawnedEnemies;
    public TextMeshProUGUI score;


    // -- Spawner Settings -- //
    [Header("Spawn Settings:")]
    public List<GameObject> spawners = new List<GameObject>();


    // -- Enemies Settings --//
    [Header("Enemies Settings: ")]
    public int enemiesCap = 100;
    public List<Enemy> enemies = new List<Enemy>();
    public Dictionary<Enemy, ObjectPool> enemyObjectsPools = new Dictionary<Enemy, ObjectPool>();
    private int _spawnedEnemies = 0;
    public int SpawnedEnemies => _spawnedEnemies;

    // -- Timer Settings -- //
    private float _timer = 0f;
    private int minutes = 0;
    private int seconds = 0;
    #endregion

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializePools();
        GameObject go = Instantiate(spawners[0]);
    }

    void Update()
    {
        _timer += Time.deltaTime;

        UpdateTimeText();

        if(_timer >= timeToGetItem)
        {
            Debug.Log("item get");
            _enemiesScore = 0;
            _enemiesKilled = 0;
            timeToGetItem += 2;
        }
    }

    private void UpdateTimeText()
    {
        minutes = (int)(_timer / 60f);
        seconds = (int)(_timer % 60f);
        textTime.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void UpdateEnemiesSpawned() => currentSpawnedEnemies.text = "Spawned Enemies:" + _spawnedEnemies;

    private void InitializePools()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyObjectsPools.Add(enemies[i], ObjectPool.CreateInstance(enemies[i], 1));
        }
    }

    public void AddEnemyScore(float val) => _enemiesScore += val;
    public void AddEnemyKilled() => _enemiesKilled++;

    public void SetSpawnedEnemies(int val)
    {
        _spawnedEnemies += val;
        UpdateEnemiesSpawned();
    }
}
