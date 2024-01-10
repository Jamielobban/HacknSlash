using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SpawnMethod { RoundRobin, Random, };
public class EnemySpawner : MonoBehaviour
{
    #region Spawner Stats
    public int enemiesToSpawn = 5;
    public float timeToSpawn = 1f;
    public List<Enemy> enemies = new List<Enemy>();
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    public GameObject player;
    #endregion

    private NavMeshTriangulation Triangulation;
    private Coroutine _spawnEnemiesCoroutine; // !!Only call once
    public bool _isBurstSpawner;
    private int _vertexIndex;
    private RoomManager _roomManager;
    private void Awake()
    {
        _roomManager = RoomManager.Instance;
    }
    private void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();
       //StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(timeToSpawn);
        int _spawnedEnemies = 0;
        while (_spawnedEnemies < enemiesToSpawn)
        {
            if (enemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(_spawnedEnemies);
            }
            else if (enemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }
            _spawnedEnemies++;
            yield return wait;
        }
        if (_isBurstSpawner)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _spawnEnemiesCoroutine = null;
        }
    }
    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int index = spawnedEnemies % enemies.Count;
        DoSpawnEnemy(index, ChooseRandomPositionOnNavMesh());
    }

    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        if(Triangulation.vertices == null)
        {
            Triangulation = NavMesh.CalculateTriangulation();
        }
        do
        {
            _vertexIndex = Random.Range(0, Triangulation.vertices.Length);
        } while (Vector3.Distance(Triangulation.vertices[_vertexIndex], FindObjectOfType<PlayerControl>().transform.position) < 5);
        return Triangulation.vertices[_vertexIndex];
    }

    public void DoSpawnEnemy(int index, Vector3 spawnPosition)
    {
        PoolableObject poolable = _roomManager.enemyObjectsPools[index].GetObject();
        if (poolable != null)
        {
            Enemy enemy = poolable.GetComponent<Enemy>();
            // *** ROOM MANAGER ***///
            _roomManager.AddEnemy(enemy.gameObject);
            if (!_roomManager.firstEnemySpawned)
            {
                _roomManager.firstEnemySpawned = true;
            }
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 50f, -1))
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
            Debug.LogError($"Unable to fetch enemy of type {spawnPosition} from object pool. Out of objects?");
        }
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, enemies.Count), ChooseRandomPositionOnNavMesh());
    }

    private void OnEnable()
    {
        _spawnEnemiesCoroutine = StartCoroutine(Spawner());
    }

    private void OnDisable()
    {
        _spawnEnemiesCoroutine = null;
        Destroy(gameObject);
    }
}



