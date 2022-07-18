using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AlienType {Green, Grey};

public class SpawnManager : MonoBehaviour
{
    // How many Aliens are in the pool
    public int PooledAmountAliensGreen;
    public int PooledAmountAliensGrey;

    public float SpawnTime;

    // Alien Data
    public GameObject[] AlienPrefabs;

    public Transform[] AlienGroupings;

    public Transform[] SpawnPoints;

    // Targets
    public Transform[] Farms;
    public Transform[] Towns;

    // Levels
    public Level[] Levels;

    // List of pooled Aliens
    private List<GameObject>[] _aliensList;

    void Start()
    {
        CreateAliens();

        // Subscribe to Events
        GameManager.Instance.PlayState.EnterPlayState += StartSpawning;
        GameManager.Instance.NoPlayState.EnterNoPlayState += StopSpawning;
        GameManager.Instance.EndState.EnterEndState += StopSpawning;
    }

    void OnDisable()
    {
        // Unsubscribe from Events
        GameManager.Instance.PlayState.EnterPlayState -= StartSpawning;
        GameManager.Instance.NoPlayState.EnterNoPlayState -= StopSpawning;
        GameManager.Instance.EndState.EnterEndState -= StopSpawning;
    }

    void StartSpawning()
    {
        InvokeRepeating("SpawnAliens", 1.0f, SpawnTime);
    }

    void StopSpawning()
    {
        // Stop the spawn method
        CancelInvoke();

        // Clear all Aliens in the map
        foreach (List<GameObject> alienList in _aliensList)
        {
            foreach (GameObject alien in alienList)
            {
                alien.GetComponent<AlienBase>().Reset();
            }
        }
    }

    void CreateAliens()
    {
        _aliensList = new List<GameObject>[2];
        _aliensList[(int)AlienType.Green] = SetupAliens(AlienType.Green, PooledAmountAliensGreen, AlienPrefabs[(int)AlienType.Green], AlienGroupings[(int)AlienType.Green]);
        _aliensList[(int)AlienType.Grey] = SetupAliens(AlienType.Grey, PooledAmountAliensGrey, AlienPrefabs[(int)AlienType.Grey], AlienGroupings[(int)AlienType.Grey]);
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
        // Implement spawning based on level
        int currentLevel = GameManager.Instance.Level;

        int alienType;

        // Decide which Alien to spawn based on Level
        if (Levels[currentLevel].SpawnGreenAliens && Levels[currentLevel].SpawnGreyAliens)
        {
            alienType = RandomSpawnChance();
        }
        else if (Levels[currentLevel].SpawnGreenAliens)
        {
            alienType = (int)AlienType.Green;
        }
        else if (Levels[currentLevel].SpawnGreyAliens)
        {
            alienType = (int)AlienType.Grey;
        }
        else
        {
            return;
        }

        GameObject alien = ObjectPooler.GetPooledObject(_aliensList[alienType]);

        // Make sure alien is available from the pool
        if (alien)
        {
            // Determine alien's destination
            Transform destination;

            // If alien spawned is Green, send to a random farm
            if (alienType == (int)AlienType.Green)
            {
                destination = Farms[Random.Range(0, Farms.Length)];
            }
            // Else if alien spawned is Grey, send to a random town
            else
            {
                destination = Towns[Random.Range(0, Towns.Length)];
            }

            // Reset alien parameters and spawn
            alien.GetComponent<AlienBase>().Reset();
            alien.GetComponent<AlienBase>().Spawn(SpawnPoints[Random.Range(0, SpawnPoints.Length)], destination);
        }
    }

    void SpawnAliensOld()
    {
        // Grab an alien type based on chance
        int rndType = RandomSpawnChance();
        GameObject alien = ObjectPooler.GetPooledObject(_aliensList[rndType]);

        // Make sure alien is available from the pool
        if (alien)
        {
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
            alien.GetComponent<AlienBase>().Reset();
            alien.GetComponent<AlienBase>().Spawn(SpawnPoints[Random.Range(0, SpawnPoints.Length)], destination);
        }
    }

    int RandomSpawnChance()
    {
        int rnd = Random.Range(1, 6);

        return (rnd == 5) ? 1 : 0;
    }
}
