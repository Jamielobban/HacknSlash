using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshProbabilitySpawner : ProbabilitySpawner
{
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(SpawnEnemies());
    }

    protected override Vector3 ChooseRandomPositionOnNavMesh()
    {
        if (_triangulation.vertices == null)
        {
            _triangulation = NavMesh.CalculateTriangulation();
        }
        int _vertexIndex;
        do
        {
            _vertexIndex = Random.Range(0, _triangulation.vertices.Length);
        } while (Vector3.Distance(_triangulation.vertices[_vertexIndex], FindObjectOfType<PlayerManager>().transform.position) < 5);
        return _triangulation.vertices[_vertexIndex];
    }
}
