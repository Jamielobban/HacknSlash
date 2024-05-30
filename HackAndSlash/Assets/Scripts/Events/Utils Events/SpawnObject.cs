using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timeToSpawnObject;
    private bool isObjectSpawned = false;

    private float _currentTime = 0f;
    void Start()
    {
        
    }

    void Update()
    {

        _currentTime += Time.deltaTime;

        if(_currentTime >= timeToSpawnObject && !isObjectSpawned)
        {
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            isObjectSpawned = true;
        }
    }
}
