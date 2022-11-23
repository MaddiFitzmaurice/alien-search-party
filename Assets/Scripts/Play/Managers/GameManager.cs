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
    public BarkState BarkState { get; private set; }
    public CutsceneState CutsceneState {get; private set; }
    public EndLevelState EndLevelState { get; private set; }

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
        BarkState = new BarkState();
        CutsceneState = new CutsceneState();
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
