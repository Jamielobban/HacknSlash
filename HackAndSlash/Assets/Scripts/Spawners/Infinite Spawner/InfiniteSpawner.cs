using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfiniteSpawner : MonoBehaviour
{
    #region Spawner Stats
    [Header("Stadistics: ")]
    public int enemiesCap;
    [Tooltip("Time to destroy spawner, must be in seconds")] public float lifeTime;
    public float timeMinToSpawn = .8f;
    public float timeMaxToSpawn = 2.5f;
    public float delayStartTime = 0f;
    public Enums.SpawnMethod enemySpawnMethod = Enums.SpawnMethod.RoundRobin;
    [SerializeField] protected List<EnemyBase> _enemies = new List<EnemyBase>();
    [SerializeField] protected bool canStartSpawn = false;
    [SerializeField] protected bool _isBurstSpawner;
    #endregion

    protected float _timeToSpawn;
    protected NavMeshTriangulation _triangulation;
    protected float _timer = 0f;
    private ManagerEnemies _managerEnemies;
    private List<EnemyBase> _spawnedEnemies = new List<EnemyBase>();
    private float _timerDelay;

    protected virtual void Start()
    {
        _timeToSpawn = Random.Range(timeMinToSpawn, timeMaxToSpawn);
        _triangulation = NavMesh.CalculateTriangulation();
        _managerEnemies = ManagerEnemies.Instance;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if(!canStartSpawn)
        {
            _timerDelay += Time.deltaTime;
            if (_timerDelay > delayStartTime)
            {
                canStartSpawn = true;
            }
        }
        else
        {
            if (_timer > _timeToSpawn && _managerEnemies.SpawnedEnemies < enemiesCap && !_managerEnemies.isInEvent)
            {
                //Spawn Enemy
                switch (enemySpawnMethod)
                {
                    case Enums.SpawnMethod.RoundRobin:
                        SpawnRoundRobinEnemy(_managerEnemies.SpawnedEnemies);
                        break;
                    case Enums.SpawnMethod.Random:
                        SpawnRandomEnemy();
                        break;
                    case Enums.SpawnMethod.Probability:
                        SpawnProbabilityEnemy();
                        break;
                    default:
                        Debug.LogError("Unsuported spawn method");
                        break;
                }
                
                _timer = 0;
            }
        }        
    }

    public void ClearAllEnemiesSpawned()
    {
        foreach (var enemy in _spawnedEnemies)
        {
            enemy.IsDead = true;
            enemy.OnDespawnEnemy();
        }
        _spawnedEnemies.Clear();
    }

    protected void SpawnRandomEnemy()
    {
        DoSpawnEnemy(_enemies[Random.Range(0, _enemies.Count)], ChooseRandomPositionOnNavMesh());
    }
    protected void SpawnRoundRobinEnemy(int spawnedEnemies)
    {

        DoSpawnEnemy(_enemies[spawnedEnemies % _enemies.Count], ChooseRandomPositionOnNavMesh());
    }

    protected virtual void SpawnProbabilityEnemy()
    {
        // Fill in ProbabilitySpawner Class
    }

    protected virtual void DoSpawnEnemy(EnemyBase e, Vector3 spawnPos)
    {
        PoolableObject poolable = _managerEnemies.enemyObjectsPools[e].GetObject(spawnPos);

        if (poolable != null)
        {
            EnemyBase enemyBase = poolable.GetComponent<EnemyBase>();
            enemyBase.target = GameManager.Instance.Player.transform;
            enemyBase.spawner = this.gameObject;
            enemyBase.OnSpawnEnemy();
            AddEnemy(enemyBase);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 100f, -1))
            {
                enemyBase.Agent.Warp(hit.position);
                enemyBase.Agent.enabled = true;
                poolable.gameObject.SetActive(true);
                ManagerEnemies.Instance.AddSpawnedEnemies(+1);
            }
            else
            {
                Debug.LogError($"Unable to place NavmeshAgent on Navmesh chau");
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {spawnPos} from object pool. Out of objects?");
        }
    }

    public void AddEnemy(EnemyBase enemyBase)
    {
        if(!_spawnedEnemies.Contains(enemyBase))
        {
            _spawnedEnemies.Add(enemyBase);
        }
    }
    public void RemoveEnemy(EnemyBase enemyBase)
    {
        if (_spawnedEnemies.Contains(enemyBase))
        {
            _spawnedEnemies.Remove(enemyBase);
        }
    }

    protected virtual Vector3 ChooseRandomPositionOnNavMesh()
    {
        return Vector3.zero;
    }

}
