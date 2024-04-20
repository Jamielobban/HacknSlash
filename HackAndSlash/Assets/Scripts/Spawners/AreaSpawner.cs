using System.Collections;
using UnityEngine;

// -- Place an enemy at any position in the NavMesh between collider's bounds -- //
[RequireComponent(typeof(Collider))]
public class AreaSpawner : SpawnerBase
{
    private Bounds _bounds;

    protected override void Awake()
    {
        base.Awake();
        Collider _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }

    protected override IEnumerator SpawnEnemies()
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
            allEnemiesSpawned = true;
            if (_isBurstSpawner)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _spawnCoroutine = null;
            }
    }

    protected override void OnEnable()
    {
        if(_spawnCoroutine == null)
        {
            _spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    protected override void OnDisable()
    {
        if(_spawnCoroutine != null)
        {
            _spawnCoroutine = null;
        }
    }

    // Gets a Random position on the navmesh in colliders bounds
    protected override Vector3 ChooseRandomPositionOnNavMesh()
    {
        Vector3 newPosition;
        Vector3 worldMinBounds = transform.TransformPoint(_bounds.min); 
        Vector3 worldMaxBounds = transform.TransformPoint(_bounds.max); 
        
        newPosition = new Vector3(Random.Range(worldMinBounds.x, worldMaxBounds.x), worldMinBounds.y, Random.Range(worldMinBounds.z, worldMaxBounds.z));
        return newPosition;
    } 
}
