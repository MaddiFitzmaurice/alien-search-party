using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class for object pooling
public static class ObjectPooler
{
    // Create object pool using a prefab and amount to pool
    public static List<GameObject> CreateObjectPool(int amountToPool, GameObject objectToPool)
    {
        List<GameObject> pooledObjects = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++) 
        {
            GameObject obj = GameObject.Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        return pooledObjects;
    }

    //Retrieve a pooled object if it is not active in the hierarchy
    public static GameObject GetPooledObject(List<GameObject> pooledObjects)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    // Assign parent transform for pooled objects to organise hierarchy
    public static void AssignParentGroup(List<GameObject> pooledObjects, Transform parent)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].transform.parent = parent;
        }
    }
}
