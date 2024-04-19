using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityAreaProbabilitySpawner : InfinityProbabilitySpawner
{
    private Bounds _bounds;
    public float minFarDistToSpawn = 5f;
    protected override void Start()
    {
        base.Start();
        Collider _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }

    protected override Vector3 ChooseRandomPositionOnNavMesh()
    {
        Vector3 pointToInstantiate;
        Vector3 worldMinBounds = transform.TransformPoint(_bounds.min); // Convert local min bounds to world space
        Vector3 worldMaxBounds = transform.TransformPoint(_bounds.max); 
        do
        {
             pointToInstantiate = new Vector3(Random.Range(worldMinBounds.x, worldMaxBounds.x), (worldMinBounds.y + worldMaxBounds.y ) * 0.5f, Random.Range(worldMinBounds.z, worldMaxBounds.z));
        } while (Vector3.Distance(GameManager.Instance.Player.transform.position, pointToInstantiate) < minFarDistToSpawn);
        Debug.Log("Spawn Position: " + pointToInstantiate);
        return pointToInstantiate;
    }
}
