using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject spawnerPrefab;
    public GameObject player;
    [SerializeField] private int enemiesToSpawn = 5;
    private int stageLevel = 0;
    //Temporal public
    public List<GameObject> _enemiesToKill = new List<GameObject>();
    private GameObject _spawnPoint;

    public bool firstEnemySpawned = false;
    public Dictionary<int, ObjectPool> enemyObjectsPools = new Dictionary<int, ObjectPool>();
    public List<Enemy> enemies = new List<Enemy>();

    private static RoomManager _instance;
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

    public void Active() { }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyObjectsPools.Add(i, ObjectPool.CreateInstance(enemies[i], enemiesToSpawn));
        }
        _spawnPoint = GameObject.Find("SpawnerInstantiatePoint");
        NextStage();
    }
    private void Update()
    {
        if(AllEnemiesKilled && firstEnemySpawned)
        {
            NextStage();
        }
        Debug.Log("Enemies Remaining: " + _enemiesToKill.Count);
    }
    private void NextStage()
    {
        stageLevel++;
        Debug.Log("Current Stage Level: " + stageLevel);
        GenerateSpawner(0.3f, 0f); // Insta Spawner
        GenerateSpawner(0.7f, 1f); // Time Spawner
        enemiesToSpawn++;
    }

    private void GenerateSpawner(float spawnPercentage, float spawnDelay)
    {
        GameObject go = Instantiate(spawnerPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
        EnemySpawner _spawn = go.GetComponent<EnemySpawner>();
        _spawn.enemiesToSpawn = Mathf.RoundToInt(enemiesToSpawn * spawnPercentage);
        _spawn.timeToSpawn = spawnDelay;
        _spawn._isBurstSpawner = true;
        go.SetActive(true);
    }

    private bool AllEnemiesKilled => _enemiesToKill.Count == 0;

    public void AddEnemy(GameObject enemy)
    {
        if (!_enemiesToKill.Contains(enemy))
        {
            _enemiesToKill.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if(_enemiesToKill.Contains(enemy))
        {
            _enemiesToKill.Remove(enemy);
        }
    }
}
