using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    protected override void OnEnable()
    {
        base.OnEnable();
        Invoke(nameof(EnableCoroutine), 0.15f);
    }

    protected override void OnDisable()
    {
        _spawnCoroutine = null;
    }
    private void EnableCoroutine() => _spawnCoroutine = StartCoroutine(SpawnEnemies());
    protected override Vector3 ChooseRandomPositionOnNavMesh()
    {
        Vector3 pointToInstantiate = Vector3.zero;
        Vector3 worldMinBounds = transform.TransformPoint(_bounds.min); 
        Vector3 worldMaxBounds = transform.TransformPoint(_bounds.max); 
            
        do
        {
            pointToInstantiate = new Vector3(Random.Range(worldMinBounds.x, worldMaxBounds.x), (worldMinBounds.y + worldMaxBounds.y ) * 0.5f, Random.Range(worldMinBounds.z, worldMaxBounds.z));
        } while (Vector3.Distance(transform.position, pointToInstantiate) < .5f && Vector3.Distance(transform.position, pointToInstantiate) > 10f);

         Debug.Log("Hit point: " + pointToInstantiate);
        return pointToInstantiate; 
    }
}
