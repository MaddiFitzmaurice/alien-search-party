using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    public static Action AllAliensCaughtEvent;

    // How many Aliens will be in each pool
    [SerializeField]
    private int _pooledAmountAliens;

    // Alien Prefab Data
    [SerializeField]
    private GameObject[] _alienPrefabs;

    // Alien Grouping Data
    [SerializeField]
    private Transform[] _alienGroupings;

    // Alien Spawn points
    [SerializeField]
    private Transform[] _spawnPoints;

    // Aliens' Target Destinations
    [SerializeField]
    private Transform[] _farms;
    
    [SerializeField]
    private Transform[] _towns;

    // List of pooled list of Aliens
    private List<List<GameObject>> _aliensList;

    // Current level
    private Level _currentLevel;

    // Object that creates Alien pool lists
    private AlienCreator _alienCreator;

    // Keeping track of total number of aliens spawned
    private int _totalGreenAliensSpawned;
    private int _totalGreyAliensSpawned;
    private int _totalAliensSpawned;

    void OnEnable()
    {
         // Subscribe to Events
        StartLevelState.EnterStartLevelStateEvent += ResetSpawner;
        PlayState.EnterPlayStateEvent += StartSpawning;

        LevelManager.SendCurrentLevel += GetCurrentLevel;
        
        AlienBase.AlienReachedDestEvent += LoseEventHandler;
        AlienBase.AlienAbductEvent += AbductEventHandler;
    }

    void OnDisable()
    {
        // Unsubscribe from Events
        StartLevelState.EnterStartLevelStateEvent -= ResetSpawner;
        PlayState.EnterPlayStateEvent -= StartSpawning;

        LevelManager.SendCurrentLevel -= GetCurrentLevel;
        
        AlienBase.AlienReachedDestEvent -= LoseEventHandler;
        AlienBase.AlienAbductEvent -= AbductEventHandler;
    }

    void Awake()
    {
        // Instantiate alien creator object and create Alien pools
        _alienCreator = new AlienCreator();
        _aliensList = _alienCreator.CreateAliens(_pooledAmountAliens, _alienPrefabs, _alienGroupings);
    }

    void ResetSpawner()
    {
        StopAllCoroutines();
        ReturnAllAliensToPool();
        _totalGreenAliensSpawned = 0;
        _totalGreyAliensSpawned = 0;
        _currentLevel.SpawnRate = _currentLevel.SpawnRateBase;
    }

    // Return all aliens to pool for a reset
    void ReturnAllAliensToPool()
    {
        foreach (List<GameObject> list in _aliensList)
        {
            ObjectPooler.ReturnObjectsToPool(list);
        }
    }

    // Receive current level from LevelManager
    void GetCurrentLevel(Level currentLevel)
    {
        _currentLevel = currentLevel;
    }

    void StartSpawning()
    {
        StartCoroutine("SpawnAliens");
    }

    // Listens for when player abducts an Alien
    void AbductEventHandler(int activeAliensLeft)
    {        
        // Make sure no aliens are active
        if (activeAliensLeft == 0)
        {
            // Check if end of level reached
            if (_totalAliensSpawned == _currentLevel.AmountGreenAliens + _currentLevel.AmountGreyAliens)
            {
                AllAliensCaughtEvent?.Invoke();
                GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.EndLevelState);
            }

            if (MenuData.StoryModeOn && (_totalAliensSpawned % 10 == 0))
            {
                GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.BarkState);
            }
        }
    }

    // Keep Alien that lost the game on display
    void LoseEventHandler(GameObject loseAlien)
    {
        StopAllCoroutines();

        foreach (List<GameObject> list in _aliensList)
        {
            foreach (GameObject alien in list)
            {
                if (alien!= loseAlien && alien.activeInHierarchy)
                {
                    alien.SetActive(false);
                }
            }
        }
    }

    IEnumerator SpawnAliens()
    {
        while (true)
        {
            yield return new WaitForSeconds(_currentLevel.SpawnRate);

            int type = SetAlienType();

            if (type != -1)
            {
                Transform destination = SetDestination(type);
                Transform spawnPoint = SetSpawnPoint();

                // Grab an Alien from the pool and spawn it
                GameObject alien = ObjectPooler.GetPooledObject(_aliensList[type]);
                alien.GetComponent<AlienBase>().Spawn(spawnPoint, destination);

                //Change spawn rate
                if (_totalAliensSpawned % _currentLevel.ChangeRateAfter == 0)
                {
                    _currentLevel.SpawnRate -= _currentLevel.SpawnRateChange;
                }
            }

            // Stop spawn coroutine if reached max for the level
            if (_totalAliensSpawned == _currentLevel.AmountGreenAliens + _currentLevel.AmountGreyAliens)
            {
                yield break;
            }
            // Stop spawn coroutine to play Story barks
            else if (_totalAliensSpawned % 10 == 0 && MenuData.StoryModeOn)
            {
                yield break;
            }
        }
    }

    // Decide which Alien to spawn based on level specs
    int SetAlienType()
    {
        // If both Green and Grey are allowed to spawn
        if (_currentLevel.SpawnGreenAliens && _currentLevel.SpawnGreyAliens)
        {
            // If max amount hasn't been reached for Green or Grey
            if (_totalGreenAliensSpawned < _currentLevel.AmountGreenAliens &&
                _totalGreyAliensSpawned < _currentLevel.AmountGreyAliens)
            {
                return RandomSpawnChance();
            }
            // If max amount has been reached for Grey
            else if (_totalGreenAliensSpawned < _currentLevel.AmountGreenAliens &&
                _totalGreyAliensSpawned >= _currentLevel.AmountGreyAliens)
            {
                return (int)AlienType.Green;
            }
            // If max amount has been reached for Green
            else if (_totalGreyAliensSpawned < _currentLevel.AmountGreyAliens &&
                _totalGreenAliensSpawned >= _currentLevel.AmountGreenAliens)
            {
                return (int)AlienType.Grey;
            }
            else 
            // Stop spawning
            {
                return -1;
            }
        }
        // If only Green is allowed to spawn
        else if (_currentLevel.SpawnGreenAliens)
        {
            // If max hasn't been reached yet
            if (_totalGreenAliensSpawned < _currentLevel.AmountGreenAliens)
            {
                return (int)AlienType.Green;
            }
            // Hit max, stop spawning
            else
            {
                return -1;   
            }
        }
        // If only Grey is allowed to spawn
        else if (_currentLevel.SpawnGreyAliens)
        {
            // If max hasn't been reached yet
            if (_totalGreyAliensSpawned < _currentLevel.AmountGreyAliens)
            {
                return (int)AlienType.Grey;
            }
            // Hit max, stop spawning
            else
            {
                return -1;
            }
        }
        // Stop spawning
        else
        {
            return -1;
        }
    }
    
    // Determine alien's destination
    Transform SetDestination(int alienType)
    {
        Transform destination;

        // If alien spawned is Green, send to a random farm
        if (alienType == (int)AlienType.Green)
        {
            destination = _farms[UnityEngine.Random.Range(0, _farms.Length)];
            _totalGreenAliensSpawned += 1;
        }
        // Else if alien spawned is Grey, send to a random town
        else
        {
            destination = _towns[UnityEngine.Random.Range(0, _towns.Length)];
            _totalGreyAliensSpawned += 1;
        }

        _totalAliensSpawned = _totalGreenAliensSpawned + _totalGreyAliensSpawned;

        return destination;
    }

    // Set Alien's spawn point
    Transform SetSpawnPoint()
    {
        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
    }

    // Randomly choose Alien type based on loaded chance selected
    int RandomSpawnChance()
    {
        int rnd = UnityEngine.Random.Range(1, _currentLevel.SpawnChanceGrey + 1);

        return (rnd == _currentLevel.SpawnChanceGrey) ? 1 : 0;
    } 
}
