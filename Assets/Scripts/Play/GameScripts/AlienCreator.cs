using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlienType { Green, Grey };

public class AlienCreator
{
    // Constructor
    public AlienCreator() {}
    
    // Create pools for different Alien types
    public List<List<GameObject>> CreateAliens(int amountToPool, GameObject[] alienPrefabs,
        Transform[] alienGroupings)
    {
        List<List<GameObject>> alienGroupsList = new List<List<GameObject>>();
        alienGroupsList.Add(SetupAliens(amountToPool, 
            alienPrefabs[(int)AlienType.Green], alienGroupings[(int)AlienType.Green]));
        alienGroupsList.Add(SetupAliens(amountToPool,
            alienPrefabs[(int)AlienType.Grey], alienGroupings[(int)AlienType.Grey]));

        return alienGroupsList;
    }

    List<GameObject> SetupAliens(int amountToPool, GameObject prefab, Transform parent)
    {
        List<GameObject> alienList = ObjectPooler.CreateObjectPool(amountToPool, prefab);
        ObjectPooler.AssignParentGroup(alienList, parent);

        return alienList;
    }
}
