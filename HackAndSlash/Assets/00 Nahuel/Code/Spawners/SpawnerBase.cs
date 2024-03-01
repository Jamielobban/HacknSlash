using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// --- Base Class For Spawners hierarchy --- //

public class SpawnerBase : MonoBehaviour
{
    #region Spawner Stats
    [Header("Stadistics: ")]
    [SerializeField] protected int _enemiesToSpawn;
    [SerializeField] protected float _timeToSpawn;
    [SerializeField] protected bool _isBurstSpawner;
    public Enums.SpawnMethod enemySpawnMethod = Enums.SpawnMethod.RoundRobin;
    [SerializeField] protected List<Enemy> _enemies = new List<Enemy>();

    public int EnemiesToSpawn
    {
        get => _enemiesToSpawn;
        set => _enemiesToSpawn = Mathf.Max(0, value);
    }
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

    protected Coroutine _spawnCoroutine; // !! Can only be called once !! //
    protected NavMeshTriangulation _triangulation;
    protected RoomManager _roomManager;

    protected virtual void Awake()
    {
        _roomManager = RoomManager.Instance;
    }

    protected virtual void Start()
    {
        _triangulation = NavMesh.CalculateTriangulation();
    }

    protected IEnumerator SpawnEnemies()
    {      
        WaitForSeconds wait = new WaitForSeconds(_timeToSpawn);

        int _spawnedEnemies = 0;
        while(_spawnedEnemies < _enemiesToSpawn)
        {
            switch (enemySpawnMethod)
            {
                case Enums.SpawnMethod.RoundRobin:
                    SpawnRoundRobinEnemy(_spawnedEnemies);
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
            _spawnedEnemies++;
            yield return wait;
        }
        if(_isBurstSpawner)
        {
            Destroy(gameObject);
        }
        else
        {
            _spawnCoroutine = null;
        }
    }

    protected virtual void DoSpawnEnemy(Enemy e, Vector3 spawnPos)
    {
        PoolableObject poolable = _roomManager.enemyObjectsPools[e].GetObject();

        if (poolable != null)
        {
            Enemy enemy = poolable.GetComponent<Enemy>();

            _roomManager.AddEnemy(enemy.gameObject); // Mete los enemigos que instancio en Enemies to Kill

            if(!_roomManager.firstEnemySpawned)
            {
                _roomManager.firstEnemySpawned = true;
            }
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 50f, -1))
            {
                enemy.movements.Agent.Warp(hit.position);
                enemy.movements.Agent.enabled = true;
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

    // Fill it with the search for the most suitable position to spawn the enemies. //
    protected virtual Vector3 ChooseRandomPositionOnNavMesh()
    {
        return Vector3.zero;
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
    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }


}
