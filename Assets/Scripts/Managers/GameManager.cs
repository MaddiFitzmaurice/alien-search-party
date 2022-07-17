using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern so everything has access to this script
    public static GameManager Instance { get; private set; }

    // States and StateMachine
    public StateMachine GMStateMachine { get; private set; }
    public IntroState IntroState { get; private set; }
    public PlayState PlayState { get; private set; }
    public EndState EndState { get; private set; }

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
        IntroState = new IntroState();
        PlayState = new PlayState();
        EndState = new EndState();

        // Enter IntroState
        GMStateMachine = new StateMachine(IntroState);
    }

    void Start()
    {
        GMStateMachine.ChangeState(IntroState);
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
