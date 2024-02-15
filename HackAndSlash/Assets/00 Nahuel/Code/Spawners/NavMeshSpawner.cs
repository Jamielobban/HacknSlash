using UnityEngine;
using UnityEngine.AI;

// -- Place an enemy at any position in the NavMesh of a map -- //
public class NavMeshSpawner : SpawnerBase
{
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
        } while (Vector3.Distance(_triangulation.vertices[_vertexIndex], FindObjectOfType<PlayerControl>().transform.position) < 5);
        return _triangulation.vertices[_vertexIndex];
    }

    protected override void OnEnable() => _spawnCoroutine = StartCoroutine(SpawnEnemies());        
    protected override void OnDisable() => _spawnCoroutine = null;
}



