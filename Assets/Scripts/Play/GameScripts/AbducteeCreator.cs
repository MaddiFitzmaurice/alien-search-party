using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbducteeType { Green, Grey, Human };

public class AbducteeCreator
{
    // Constructor
    public AbducteeCreator() {}
    
    public List<GameObject> CreateHumans(int amountToPool, GameObject humanPrefab, Transform humansGrouping)
    {
        List<GameObject> humans = ObjectPooler.CreateObjectPool(amountToPool, humanPrefab);
        ObjectPooler.AssignParentGroup(humans, humansGrouping);
        return humans;
    }

    // Create pools for different Alien types
    public List<List<GameObject>> CreateAliens(int amountToPool, GameObject[] alienPrefabs,
        Transform[] alienGroupings)
    {
        List<List<GameObject>> alienGroupsList = new List<List<GameObject>>();
        alienGroupsList.Add(SetupAliens(amountToPool, 
            alienPrefabs[(int)AbducteeType.Green], alienGroupings[(int)AbducteeType.Green]));
        alienGroupsList.Add(SetupAliens(amountToPool,
            alienPrefabs[(int)AbducteeType.Grey], alienGroupings[(int)AbducteeType.Grey]));

        return alienGroupsList;
    }

    List<GameObject> SetupAliens(int amountToPool, GameObject prefab, Transform parent)
    {
        List<GameObject> alienList = ObjectPooler.CreateObjectPool(amountToPool, prefab);
        ObjectPooler.AssignParentGroup(alienList, parent);

        return alienList;
    }
}
