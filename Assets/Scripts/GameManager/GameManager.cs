using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern so everything has access to this script
    public static GameManager Instance { get; private set; }

    // States and StateMachine
    public StateMachine GMStateMachine { get; private set; }
    public StartLevelState StartLevelState { get; private set; }
    public PlayState PlayState { get; private set; }
    public EndLevelState EndLevelState { get; private set; }

    public Level[] Levels;
    // Ensure level is no greater than 3
    [SerializeField]
    private int _level;
    public int Level
    {
        get { return _level; }
        set 
        {
            _level = value;
            if (_level > Levels.Length - 1)
            {
                _level = Levels.Length - 1;
            }
        }
    }

    void Awake()
    {
        // Set up singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
            return;
        }

        // Set up states
        StartLevelState = new StartLevelState();
        PlayState = new PlayState();
        EndLevelState = new EndLevelState();

        // Enter IntroState
        GMStateMachine = new StateMachine(StartLevelState);
    }

    void Start()
    {
        GMStateMachine.ChangeState(StartLevelState);
    }

    void Update()
    {
        GMStateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        GMStateMachine.CurrentState.PhysicsUpdate();
    }
}
