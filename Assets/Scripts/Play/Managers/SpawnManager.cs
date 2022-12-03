using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    // SpawnManager Events
    public static Action<GameObject> AbducteeWinLoseEvent;
    public static Action<int> ActiveAbducteesEvent;

    // How many Aliens/Humans will be in each pool
    [SerializeField]
    private int _poolAmount;
    [SerializeField]
    private int _poolAmountTutorial;

    // Alien/Human Prefab Data
    [SerializeField]
    private GameObject[] _abducteePrefabs;

    // Alien Grouping Data
    [SerializeField]
    private Transform[] _abducteeGroupings;

    // Spawn points
    [SerializeField]
    private Transform[] _alienSpawnPoints;
    [SerializeField]
    private Transform[] _humanSpawnPoints;

    // Aliens' Target Destinations
    [SerializeField]
    private Transform[] _farms;
    
    [SerializeField]
    private Transform[] _towns;

    // Abductee pool lists
    private List<List<GameObject>> _aliensList;
    private List<GameObject> _humans;

    // Current level
    private Level _currentLevel;

    // Object that creates Abductee pool lists
    private AbducteeCreator _abducteeCreator;

    // Keeping track of total number of aliens spawned
    private int _totalGreenAliensSpawned;
    private int _totalGreyAliensSpawned;
    private int _totalHumansSpawned;
    private int _totalAbducteesSpawned;

    // Keep track of active abductees
    private int _totalAbducteesActive;

    void OnEnable()
    {
        // Subscribe to Events
        StartLevelState.EnterStartLevelStateEvent += ResetSpawner;
        PlayState.EnterPlayStateEvent += StartSpawning;
        EndLevelState.EnterEndLevelStateEvent += StopSpawning;

        LevelManager.SendCurrentLevel += GetCurrentLevel;
        
        Alien.AlienReachedDestEvent += LoseEventHandler;
        Abductee.AbductEvent += AbductEventHandler;
        Abductee.SpawnEvent += SpawnEventHandler;
    }

    void OnDisable()
    {
        // Unsubscribe from Events
        StartLevelState.EnterStartLevelStateEvent -= ResetSpawner;
        PlayState.EnterPlayStateEvent -= StartSpawning;
        EndLevelState.EnterEndLevelStateEvent -= StopSpawning;

        LevelManager.SendCurrentLevel -= GetCurrentLevel;
        
        Alien.AlienReachedDestEvent -= LoseEventHandler;
        Abductee.AbductEvent -= AbductEventHandler;
        Abductee.SpawnEvent -= SpawnEventHandler;
    }

    void Awake()
    {
        // Instantiate abductee creator object and create Alien pools and Human pool
        _abducteeCreator = new AbducteeCreator();
        _aliensList = _abducteeCreator.CreateAliens(_poolAmount, _abducteePrefabs, _abducteeGroupings);
        _humans = _abducteeCreator.CreateHumans(_poolAmountTutorial, _abducteePrefabs[(int)AbducteeType.Human],
            _abducteeGroupings[(int)AbducteeType.Human]);
    }

    // Return all abductees to pool, reset spawn num trackers, reset spawn rate 
    void ResetSpawner()
    {
        StopSpawning();
        ReturnAllAliensToPool(null);
        _totalAbducteesActive = 0;
        _totalGreenAliensSpawned = 0;
        _totalGreyAliensSpawned = 0;
        _totalHumansSpawned = 0;
        _currentLevel.SpawnRate = _currentLevel.SpawnRateBase;
    }

    // Return all aliens to pool for a reset except Alien that lost the game if applicable
    void ReturnAllAliensToPool(GameObject loseAlien)
    {
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

    // Receive current level from LevelManager
    void GetCurrentLevel(Level currentLevel)
    {
        _currentLevel = currentLevel;
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnAbductees());
    }

    void StopSpawning()
    {
        StopAllCoroutines();
    }

    // Listens for when player abducts an Abductee
    void AbductEventHandler()
    {
        _totalAbducteesActive--;
        ActiveAbducteesEvent?.Invoke(_totalAbducteesActive);  

        // Make sure no abductees are active
        if (_totalAbducteesActive == 0)
        {
            // Check if end of level reached
            if (_totalAbducteesSpawned == _currentLevel.AmountGreenAliens + _currentLevel.AmountGreyAliens + _currentLevel.AmountHumans)
            {
                AbducteeWinLoseEvent?.Invoke(null);
                GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.EndLevelState);
            }

            // Bark
            if (MenuData.StoryModeOn && (_totalAbducteesSpawned % 10 == 0))
            {
                GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.BarkState);
            }
        }
    }

    // Listens for when an Abductee spawns
    void SpawnEventHandler()
    {
        _totalAbducteesActive++; 
        ActiveAbducteesEvent?.Invoke(_totalAbducteesActive);   
    }

    // Keep Alien that lost the game on display
    void LoseEventHandler(GameObject loseAlien)
    {
        StopSpawning();
        ReturnAllAliensToPool(loseAlien);
        AbducteeWinLoseEvent?.Invoke(loseAlien);
    }

    IEnumerator SpawnAbductees()
    {
        while (true)
        {
            if (_currentLevel.Tutorial)
            {
                SpawnHumans();
                break;
            }

            yield return new WaitForSeconds(_currentLevel.SpawnRate);

            int type = SetAlienType();

            if (type != -1 && type != 2)
            {
                Transform destination = SetDestination(type);
                Transform spawnPoint = SetSpawnPoint();

                // Grab an Alien from the pool and spawn it
                GameObject alien = ObjectPooler.GetPooledObject(_aliensList[type]);
                alien.GetComponent<Alien>().Spawn(spawnPoint, destination);

                //Change spawn rate
                if (_totalAbducteesSpawned % _currentLevel.ChangeRateAfter == 0)
                {
                    _currentLevel.SpawnRate -= _currentLevel.SpawnRateChange;
                }
            }

            // Stop spawn coroutine if reached max for the level
            if (_totalAbducteesSpawned == _currentLevel.AmountGreenAliens + _currentLevel.AmountGreyAliens)
            {
                yield break;
            }
            // Stop spawn coroutine to play Story barks
            else if (_totalAbducteesSpawned % 10 == 0 && MenuData.StoryModeOn)
            {
                yield break;
            }
        }
    }

    // Spawn humans if tutorial
    public void SpawnHumans()
    {
        int i = 0;
        foreach (GameObject human in _humans)
        {
            Human humanData = human.GetComponent<Human>();
            humanData.Spawn(_humanSpawnPoints[i], _humanSpawnPoints[i]);
            human.SetActive(true);
            _totalHumansSpawned++;
            i++;
        }

        _totalAbducteesSpawned = _totalHumansSpawned;
    }

    // Decide which Alien to spawn based on level specs
    int SetAlienType()
    {   
        if (_currentLevel.Tutorial)
        {
            return (int)AbducteeType.Human;
        }
        // If both Green and Grey are allowed to spawn
        else if (_currentLevel.SpawnGreenAliens && _currentLevel.SpawnGreyAliens)
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
                return (int)AbducteeType.Green;
            }
            // If max amount has been reached for Green
            else if (_totalGreyAliensSpawned < _currentLevel.AmountGreyAliens &&
                _totalGreenAliensSpawned >= _currentLevel.AmountGreenAliens)
            {
                return (int)AbducteeType.Grey;
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
                return (int)AbducteeType.Green;
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
                return (int)AbducteeType.Grey;
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
        if (alienType == (int)AbducteeType.Green)
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

        _totalAbducteesSpawned = _totalGreenAliensSpawned + _totalGreyAliensSpawned;

        return destination;
    }

    // Set Alien's spawn point
    Transform SetSpawnPoint()
    {
        return _alienSpawnPoints[UnityEngine.Random.Range(0, _alienSpawnPoints.Length)];
    }

    // Randomly choose Alien type based on loaded chance selected
    int RandomSpawnChance()
    {
        int rnd = UnityEngine.Random.Range(1, _currentLevel.SpawnChanceGrey + 1);

        return (rnd == _currentLevel.SpawnChanceGrey) ? 1 : 0;
    } 
}
