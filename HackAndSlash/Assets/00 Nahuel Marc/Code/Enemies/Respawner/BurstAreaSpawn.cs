using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BurstAreaSpawn : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private List<Enemy> _enemies = new List<Enemy>();
    [SerializeField] private SpawnMethod _spawnMethod = SpawnMethod.Random;
    [SerializeField] private int _spawnCount = 10; // how many enemies to spawn when enter area
    [SerializeField] private float _spawnDelay = 0.5f;

    private Coroutine _spawnEnemiesCoroutine; // !!Only call once

    private Bounds _bounds;

    public bool isBurstSpawner;

    private void Awake()
    {
        _spawner = FindAnyObjectByType<EnemySpawner>();
        Collider _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_spawnEnemiesCoroutine == null)
        {
            _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    private Vector3 GetRandomPosInBounds()
    {
        return new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), _bounds.min.y, Random.Range(_bounds.min.z, _bounds.max.z));
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);
        for (int i = 0; i < _spawnCount; i++)
        {
            if(_spawnMethod == SpawnMethod.RoundRobin)
            {
                _spawner.DoSpawnEnemy(_spawner.enemies.FindIndex((enemy) => enemy.Equals(_enemies[i % _enemies.Count])), GetRandomPosInBounds());
            }
            else if(_spawnMethod == SpawnMethod.Random)
            {
                int index = Random.Range(0, _enemies.Count);
                _spawner.DoSpawnEnemy(_spawner.enemies.FindIndex((enemy) => enemy.Equals(_enemies[index])), GetRandomPosInBounds());
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


}
