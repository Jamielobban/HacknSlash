using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerEnemies : MonoBehaviour
{
    #region Settings
    public TextMeshProUGUI currentSpawnedEnemies;

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

    private GameObject _currentSpawner = null;
    private InfiniteSpawner _currentSpawnerScript = null;
    private int _currentSpawnerIndex = 0;
    private int _spawnedEnemies = 0;
    

    private float _timerSpawners = 0f;    
    private float _scaleLifeMultiplier = 0f;
    private float _scaleDamageMultiplier = 0f;

    public float startScaleLifeMultiplier;
    public float startScaleDamageMultiplier;

    #endregion

    public float ScaleLifeMult => _scaleLifeMultiplier;
    public float ScaleDamageMult => _scaleDamageMultiplier;
    public int SpawnedEnemies => _spawnedEnemies;
    public GameObject CurrentSpawner => _currentSpawner;
    public InfiniteSpawner CurrentSpawnerScript => _currentSpawnerScript;

    public void Initialize()
    {
        InitializePools();
        InitializeNewSpawner();
    }

    public void HandleEnemiesUpdate()
    {
        _timerSpawners += Time.deltaTime;      

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
    }

    public void UpgradeEnemies(float time)
    {
        float lerpFactor = Mathf.Clamp01(time / timeToReachMax);
        _scaleLifeMultiplier = Mathf.Lerp(startScaleDamageMultiplier, maxMultiplierLife + startScaleDamageMultiplier, lerpFactor);
        _scaleDamageMultiplier = Mathf.Lerp(startScaleDamageMultiplier, maxMultiplierAttack + maxMultiplierAttack, lerpFactor);
        Debug.Log(_scaleLifeMultiplier);
        Debug.Log(_scaleDamageMultiplier);
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

    public void UpdateEnemiesSpawned() => currentSpawnedEnemies.text = "Spawned Enemies:" + _spawnedEnemies;

    private void InitializePools()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyObjectsPools.Add(enemies[i], ObjectPool.CreateInstance(enemies[i], 0));
        }
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

    public void ClearEnemies()
    {
        if (_currentSpawner != null)
        {
            _currentSpawnerScript.ClearAllEnemiesSpawned();
            ResetSpawnedEnemies();
        }
    }

    public void ResetSpawnedEnemies() => _spawnedEnemies = 0;

    private void NextSpawner()
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
