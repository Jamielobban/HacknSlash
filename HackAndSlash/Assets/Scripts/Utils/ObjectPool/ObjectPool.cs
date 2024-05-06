using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject _parent;
    private PoolableObject _prefab;
    private int _size;
    private List<PoolableObject> _availableObjectsPool;
    private ObjectPool(PoolableObject Prefab, int Size)
    {
        _prefab = Prefab;
        _size = Size;
        _availableObjectsPool = new List<PoolableObject>(Size);
    }

    public static ObjectPool CreateInstance(PoolableObject Prefab, int Size)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);

        pool._parent = new GameObject(Prefab + " Pool");
        LevelManager.Instance.EnemiesManager.parentObjectPools.Add(pool._parent);
        pool.CreateObjects();

        return pool;
    }

    private void CreateObjects()
    {
        for (int i = 0; i < _size; i++)
        {
            CreateObject(Vector3.zero);
        }
    }

    private void CreateObject(Vector3 pos)
    {
        PoolableObject poolableObject = GameObject.Instantiate(_prefab, pos, Quaternion.identity, _parent.transform);
        poolableObject.parent = this;
        //poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        ReturnObjectToPool(poolableObject);
    }

    public PoolableObject GetObject(Vector3 position)
    {
        if(_availableObjectsPool.Count == 0) // auto expand pool size if out of objects
        {
            CreateObject(position);
        }
        PoolableObject instance = _availableObjectsPool[0];

        _availableObjectsPool.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void ReturnObjectToPool(PoolableObject Object)
    {
        _availableObjectsPool.Add(Object);
    }
}
