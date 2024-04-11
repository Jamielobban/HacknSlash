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

    private void OnTriggerEnter(Collider other)
    {
        if(_spawnCoroutine == null)
        {
            _spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    // Gets a Random position on the navmesh in colliders bounds
    protected override Vector3 ChooseRandomPositionOnNavMesh() => new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), _bounds.min.y, Random.Range(_bounds.min.z, _bounds.max.z));
}
