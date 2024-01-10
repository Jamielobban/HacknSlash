using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class BurstAreaSpawn : MonoBehaviour
{
    #region Stats
    public List<Enemy> enemies = new List<Enemy>();
    public int spawnCount = 10; // how many enemies to spawn when enter area
    public float spawnDelay = 0.5f;
    public bool isBurstSpawner;
    [SerializeField] private SpawnMethod _spawnMethod = SpawnMethod.Random;
    #endregion

    private Coroutine _spawnEnemiesCoroutine; // !!Only call once
    private Bounds _bounds;

    //--- Base Spawner ---//
    [SerializeField] private Dictionary<int, ObjectPool> _enemyObjectsPools = new Dictionary<int, ObjectPool>();
    private NavMeshTriangulation Triangulation;
    private int _vertexIndex;

    private void Awake()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            _enemyObjectsPools.Add(i, ObjectPool.CreateInstance(enemies[i], 0));
        }
        Collider _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }

    private void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(_spawnEnemiesCoroutine == null)
        //{
        //    _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        //}
    }

    public void DoSpawnEnemy(int index, Vector3 spawnPosition)
    {
        PoolableObject poolable = _enemyObjectsPools[index].GetObject();
        if (poolable != null)
        {
            Enemy enemy = poolable.GetComponent<Enemy>();
            // *** ROOM MANAGER ***///
            RoomManager.Instance.AddEnemy(enemy.gameObject);
            if (!RoomManager.Instance.firstEnemySpawned)
            {
                RoomManager.Instance.firstEnemySpawned = true;
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


    private Vector3 GetRandomPosInBounds()
    {
        return new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), _bounds.min.y, Random.Range(_bounds.min.z, _bounds.max.z));
    }
    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);
        for (int i = 0; i < spawnCount; i++)
        {
            if(_spawnMethod == SpawnMethod.RoundRobin)
            {
                DoSpawnEnemy(enemies.FindIndex((enemy) => enemy.Equals(enemies[i % enemies.Count])), GetRandomPosInBounds());
            }
            else if(_spawnMethod == SpawnMethod.Random)
            {
                int index = Random.Range(0, enemies.Count);
                DoSpawnEnemy(enemies.FindIndex((enemy) => enemy.Equals(enemies[index])), GetRandomPosInBounds());
            }
            yield return wait;
        }
        if(isBurstSpawner)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _spawnEnemiesCoroutine = null;
        }
    }
    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        do
        {
            _vertexIndex = Random.Range(0, Triangulation.vertices.Length);
        } while (Vector3.Distance(Triangulation.vertices[_vertexIndex], FindObjectOfType<PlayerControl>().transform.position) < 5);
        return Triangulation.vertices[_vertexIndex];
    }

    private void OnEnable()
    {
        _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
    }

    private void OnDisable()
    {
        _spawnEnemiesCoroutine = null;
    }

}
