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
        do
        {
             pointToInstantiate = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), _bounds.min.y, Random.Range(_bounds.min.z, _bounds.max.z));
            
        } while (Vector3.Distance(GameManager.Instance.Player.transform.position, pointToInstantiate) < minFarDistToSpawn);

        return pointToInstantiate;
    }
}
