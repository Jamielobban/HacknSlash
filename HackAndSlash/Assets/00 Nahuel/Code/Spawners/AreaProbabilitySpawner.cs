using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProbabilitySpawner : ProbabilitySpawner
{
    private Bounds _bounds;

    protected override void Awake()
    {
        base.Awake();
        Collider _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }
    // Si quieres que se active al entrar a un sitio concreto
    private void OnTriggerEnter(Collider other)
    {
        //if (_spawnCoroutine == null)
        //{
        //    _spawnCoroutine = StartCoroutine(SpawnEnemies());
        //}
    }

    protected override Vector3 ChooseRandomPositionOnNavMesh() => new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), _bounds.min.y, Random.Range(_bounds.min.z, _bounds.max.z));

}
