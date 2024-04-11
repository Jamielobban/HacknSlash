using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfiniteSpawner : MonoBehaviour
{
    #region Spawner Stats
    [Header("Stadistics: ")]
    [SerializeField] protected float _timeToSpawn;
    [SerializeField] protected bool _isBurstSpawner;
    public Enums.SpawnMethod enemySpawnMethod = Enums.SpawnMethod.RoundRobin;
    [SerializeField] protected List<Enemy> _enemies = new List<Enemy>();

    public float TimeToSpawn
    {
        get => _timeToSpawn;
        set => _timeToSpawn = Mathf.Max(0f, value);
    }
    public bool IsBurstSpawner
    {
        get => _isBurstSpawner;
        set => _isBurstSpawner = value;
    }
    #endregion

    protected NavMeshTriangulation _triangulation;
    //protected RoomManager _roomManager;
    protected float _timer = 0f;

    public float timeMinToSpawn = .8f;
    public float timeMaxToSpawn = 2.5f;

    private ManagerEnemies _managerEnemies;
    private List<Enemy> _spawnedEnemies = new List<Enemy>();

    public int enemiesCap;

    public float delayStartTime = 0f;
    private float _timerDelay;

    public bool canStartSpawn = false;
    protected virtual void Start()
    {
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
            if (_timer > _timeToSpawn && _managerEnemies.SpawnedEnemies <= enemiesCap && !_managerEnemies.isInEvent)
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
                _managerEnemies.SetSpawnedEnemies(+1);
                _timer = 0;
            }
        }        
    }

    public void ClearAllEnemiesSpawned()
    {
        foreach (var enemy in _spawnedEnemies)
        {
            //enemy.animations.EnemyDieApply();
            enemy.IsDead = true;
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

    protected virtual void DoSpawnEnemy(Enemy e, Vector3 spawnPos)
    {
        PoolableObject poolable = _managerEnemies.enemyObjectsPools[e].GetObject();

        if (poolable != null)
        {
            Enemy enemy = poolable.GetComponent<Enemy>();
            enemy.target = GameManager.Instance.Player.transform;
            enemy.spawner = this.gameObject;
            enemy.OnSpawnEnemy();
            AddEnemy(enemy);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 50f, -1))
            {
                enemy.Agent.Warp(hit.position);
                enemy.Agent.enabled = true;
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

    public void AddEnemy(Enemy enemy)
    {
        if(!_spawnedEnemies.Contains(enemy))
        {
            _spawnedEnemies.Add(enemy);
        }
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if (_spawnedEnemies.Contains(enemy))
        {
            _spawnedEnemies.Remove(enemy);
        }
    }

    protected virtual Vector3 ChooseRandomPositionOnNavMesh()
    {
        return Vector3.zero;
    }

}
