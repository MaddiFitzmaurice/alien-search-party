using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AlienType {Green, Grey};

public class SpawnManager : MonoBehaviour
{
    public int AmountAliensGreen;
    public int AmountAliensGrey;

    public float SpawnTime;
    public GameObject[] AlienPrefabs;

    public Transform[] AlienGroupings;

    public Transform[] SpawnPoints;
    public Transform[] Farms;
    public Transform[] Towns;

    // List of pooled Aliens
    private List<GameObject>[] _aliensList;

    void Start()
    {
        CreateAliens();
        InvokeRepeating("SpawnAliens", 1.0f, SpawnTime);
    }

    void CreateAliens()
    {
        _aliensList = new List<GameObject>[2];
        _aliensList[(int)AlienType.Green] = SetupAliens(AlienType.Green, AmountAliensGreen, AlienPrefabs[(int)AlienType.Green], AlienGroupings[(int)AlienType.Green]);
        _aliensList[(int)AlienType.Grey] = SetupAliens(AlienType.Grey, AmountAliensGrey, AlienPrefabs[(int)AlienType.Grey], AlienGroupings[(int)AlienType.Grey]);
    }

    // Helper function to setup alien object pooling for different types
    List<GameObject> SetupAliens(AlienType type, int amount, GameObject prefab, Transform parent)
    {
        List<GameObject> alienList = ObjectPooler.CreateObjectPool(amount, prefab);
        ObjectPooler.AssignParentGroup(alienList, parent);

        return alienList;
    }

    void SpawnAliens()
    {
        // Grab an alien type based on chance
        int rndType = RandomSpawnChance();
        GameObject alien = ObjectPooler.GetPooledObject(_aliensList[rndType]);

        // Make sure alien is available from the pool
        if (alien)
        {
            Debug.Log(alien);
            // Determine alien's destination
            Transform destination;

            // If alien spawned is Green, send to a random farm
            if (rndType == (int)AlienType.Green)
            {
                destination = Farms[Random.Range(0, Farms.Length)];
            }
            // Else if alien spawned is Grey, send to a random town
            else
            {
                destination = Towns[Random.Range(0, Towns.Length)];
            }

            // Reset alien parameters and spawn
            alien.GetComponent<AlienBase>().Reset(SpawnPoints[Random.Range(0, SpawnPoints.Length)], destination);
        }
    }

    int RandomSpawnChance()
    {
        int rnd = Random.Range(1, 6);

        return (rnd == 5) ? 1 : 0;
    }
}
