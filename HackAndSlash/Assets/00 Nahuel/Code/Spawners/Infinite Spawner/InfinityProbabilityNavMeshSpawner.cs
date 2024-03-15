using UnityEngine;
using UnityEngine.AI;

public class InfinityProbabilityNavMeshSpawner : InfinityProbabilitySpawner
{
    public float maxFarDistToSpawn = 50f;
    public float minFarDistToSpawn = 5f;

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
        } while (Vector3.Distance(_triangulation.vertices[_vertexIndex], FindObjectOfType<PlayerControl>().transform.position) < minFarDistToSpawn && Vector3.Distance(_triangulation.vertices[_vertexIndex], FindObjectOfType<PlayerControl>().transform.position) > maxFarDistToSpawn);
        return _triangulation.vertices[_vertexIndex];
    }
}
