using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public enum SpawnMethod { RoundRobin, Random, };
public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public int enemiesToSpawn = 5;
    public float timeToSpawn = 1f;
    public List<Enemy> enemies = new List<Enemy>();
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    private Dictionary<int, ObjectPool> _enemyObjectsPools = new Dictionary<int, ObjectPool>();
    private NavMeshTriangulation Triangulation;

    public List<GameObject> spawners = new List<GameObject>();
    public List<GameObject> spawnersInstantiate = new List<GameObject>();
    public GameObject containerDemon, containerSkeleton;
    private int _vertexIndex;
    private void Awake()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            _enemyObjectsPools.Add(i, ObjectPool.CreateInstance(enemies[i], enemiesToSpawn));
        }
        containerDemon = GameObject.Find("DemonMage 1 (DemonMage) Pool");
        containerSkeleton = GameObject.Find("SkeletonZombie (SkeletonZombie) Pool");
    }
    private void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();

        StartCoroutine(Spawner());
    }

    public void InstantiateBaseSpawners()
    {
        foreach (var spawner in spawners)
        {
            GameObject go = Instantiate(spawner, gameObject.transform);
            spawnersInstantiate.Add(go);
        }
    }

    public void DestroyBaseSpawners()
    {
        foreach (var spawner in spawnersInstantiate)
        {
            Destroy(spawner);
        }
        spawnersInstantiate.Clear();
        for (int i = 0; i < containerDemon.transform.childCount; i++)
        {
            containerDemon.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < containerSkeleton.transform.childCount; i++)
        {
            containerSkeleton.transform.GetChild(i).gameObject.SetActive(false);
        }
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
    }
    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int index = spawnedEnemies % enemies.Count;
        DoSpawnEnemy(index, ChooseRandomPositionOnNavMesh());
    }

    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        do
        {
            _vertexIndex = Random.Range(0, Triangulation.vertices.Length);
        } while (Vector3.Distance(Triangulation.vertices[_vertexIndex], FindObjectOfType<PlayerControl>().transform.position) < 5);
        return Triangulation.vertices[_vertexIndex];
    }

    public void DoSpawnEnemy(int index, Vector3 spawnPosition)
    {
        PoolableObject poolable = _enemyObjectsPools[index].GetObject();
        if (poolable != null)
        {
            Enemy enemy = poolable.GetComponent<Enemy>();

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
}



