using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AlienType {Green, Grey};

public class SpawnManager : MonoBehaviour
{
    // How many Aliens are in the pool
    public int PooledAmountAliensGreen;
    public int PooledAmountAliensGrey;

    // Time after level starts to start spawning Aliens
    [SerializeField]
    private float _startSpawnTime;

    // Alien Data
    public GameObject[] AlienPrefabs;

    public Transform[] AlienGroupings;

    public Transform[] SpawnPoints;

    // Targets
    public Transform[] Farms;
    public Transform[] Towns;

    // Levels
    private Level[] _levels;

    // List of pooled Aliens
    private List<GameObject>[] _aliensList;

    private int _currentLevel;

    private int _totalGreenAliensSpawned;
    private int _totalGreyAliensSpawned;

    void Start()
    {
        _levels = GameManager.Instance.Levels;
        _totalGreenAliensSpawned = 0;
        _totalGreyAliensSpawned = 0;

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
        _currentLevel = GameManager.Instance.Level;
        InvokeRepeating("SpawnAliens", _startSpawnTime, _levels[_currentLevel].SpawnRate);
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

        _totalGreenAliensSpawned = 0;
        _totalGreyAliensSpawned = 0;
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
        // Spawn type of Alien based on level
        int alienType;
        alienType = ChooseAlienType();

        // If max amount of Aliens spawned for the level reached, stop spawning
        if (alienType == -1)
        {
            // Check to see if player has abducted all aliens
            var aliensLeft = FindObjectsOfType<AlienBase>();
            if (aliensLeft.Length == 0)
            {
                // Advance to next level
                GameManager.Instance.Level += 1;
                GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.NoPlayState);
            }
            return;
        }
        // If max hasn't been reached yet, keep spawning
        else 
        {
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
                    _totalGreenAliensSpawned += 1;
                }
                // Else if alien spawned is Grey, send to a random town
                else
                {
                    destination = Towns[Random.Range(0, Towns.Length)];
                    _totalGreyAliensSpawned += 1;
                }

                // Reset alien parameters and spawn
                alien.GetComponent<AlienBase>().Reset();
                alien.GetComponent<AlienBase>().Spawn(SpawnPoints[Random.Range(0, SpawnPoints.Length)], destination);
            }
        }
    }

    int ChooseAlienType()
    {
        // Decide which Alien to spawn based on current level specs
        // If both Green and Grey are allowed to spawn
        int type;

        if (_levels[_currentLevel].SpawnGreenAliens && _levels[_currentLevel].SpawnGreyAliens)
        {
            // If max amount hasn't been reached for Green or Grey
            if (_totalGreenAliensSpawned < _levels[_currentLevel].AmountGreenAliens &&
                _totalGreyAliensSpawned < _levels[_currentLevel].AmountGreyAliens)
            {
                type = RandomSpawnChance();
            }
            // If max amount has been reached for Grey
            else if (_totalGreenAliensSpawned < _levels[_currentLevel].AmountGreenAliens &&
                _totalGreyAliensSpawned >= _levels[_currentLevel].AmountGreyAliens)
            {
                type = (int)AlienType.Green;
            }
            // If max amount has been reached for Green
            else if (_totalGreyAliensSpawned < _levels[_currentLevel].AmountGreyAliens &&
                _totalGreenAliensSpawned >= _levels[_currentLevel].AmountGreenAliens)
            {
                type = (int)AlienType.Grey;
            }
            else 
            // Stop spawning
            {
                type = -1;
            }
        }
        // If only Green is allowed to spawn
        else if (_levels[_currentLevel].SpawnGreenAliens)
        {
            // If max hasn't been reached yet
            if (_totalGreenAliensSpawned < _levels[_currentLevel].AmountGreenAliens)
            {
                type = (int)AlienType.Green;
            }
            // Hit max, stop spawning
            else
            {
                type = -1;    
            }
        }
        // If only Grey is allowed to spawn
        else if (_levels[_currentLevel].SpawnGreyAliens)
        {
            // If max hasn't been reached yet
            if (_totalGreyAliensSpawned < _levels[_currentLevel].AmountGreyAliens)
            {
                type = (int)AlienType.Grey;
            }
            // Hit max, stop spawning
            else
            {
                type = -1;    
            }
        }
        // Stop spawning
        else
        {
            type = -1;
        }

        return type;
    }

    // Randomly choose Alien type based on loaded chance selected
    int RandomSpawnChance()
    {
        int rnd = Random.Range(1, _levels[_currentLevel].SpawnChanceGrey + 1);

        return (rnd == _levels[_currentLevel].SpawnChanceGrey) ? 1 : 0;
    }
}
