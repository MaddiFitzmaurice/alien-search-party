using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class for object pooling
public static class ObjectPooler
{
    // Create object pool using a prefab and amount to pool
    public static List<GameObject> CreateObjectPool(int _amountToPool, GameObject _objectToPool)
    {
        List<GameObject> pooledObjects = new List<GameObject>();

        for (int i = 0; i < _amountToPool; i++) 
        {
            GameObject obj = GameObject.Instantiate(_objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        return pooledObjects;
    }

    //Retrieve a pooled object if it is not active in the hierarchy
    public static GameObject GetPooledObject(List<GameObject> _pooledObjects)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        return null;
    }

    // Assign parent transform for pooled objects to organise hierarchy
    public static void AssignParentGroup(List<GameObject> _pooledObjects, Transform _parent)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            _pooledObjects[i].transform.parent = _parent;
        }
    }
}
