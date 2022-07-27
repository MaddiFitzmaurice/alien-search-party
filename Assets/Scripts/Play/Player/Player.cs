using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    [HideInInspector]
    public Rigidbody Rb;
    [HideInInspector]
    public ParticleSystem Beam;

    public Animator PlayerAnim;

    // State Machine and States
    private StateMachine _playerStateMachine;
    private PlayerControlState _playerControlState;
    private PlayerNoControlState _playerNoControlState;

    // Player Data
    public float MoveSpeed;
    public float BeamSpeed;

    void Awake()
    {
        // Set up components
        Rb = GetComponent<Rigidbody>();
        Beam = GetComponentInChildren<ParticleSystem>();

        // Set up state machine and states
        _playerControlState = new PlayerControlState(this);
        _playerNoControlState = new PlayerNoControlState(this);

        _playerStateMachine = new StateMachine(_playerNoControlState);
    }

    void Start()
    {
        // Set up event triggers for state changes
        GameManager.Instance.PlayState.EnterPlayState += EnterPlayerControlState;
        GameManager.Instance.StartLevelState.EnterStartLevelState += EnterPlayerNoControlState;
        GameManager.Instance.EndLevelState.EnterEndLevelState += EnterPlayerNoControlState;
    }

    void OnDisable()
    {
        // Unsubscribe from event triggers
        GameManager.Instance.PlayState.EnterPlayState -= EnterPlayerControlState;
        GameManager.Instance.StartLevelState.EnterStartLevelState -= EnterPlayerNoControlState;
        GameManager.Instance.EndLevelState.EnterEndLevelState -= EnterPlayerNoControlState;
    }

    // Update State Functions
    void Update()
    {
        _playerStateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        _playerStateMachine.CurrentState.PhysicsUpdate();
    }

    // Collider State Functions
    void OnTriggerEnter(Collider other)
    {
        _playerStateMachine.CurrentState.OnTriggerEnter(other);
    }   

    void OnTriggerStay(Collider other)
    {
        _playerStateMachine.CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        _playerStateMachine.CurrentState.OnTriggerExit(other);
    }

    // Event Trigger Functions
    void EnterPlayerControlState()
    {
        ChangeState(_playerControlState);
    }

    void EnterPlayerNoControlState()
    {
        ChangeState(_playerNoControlState);
    }

    // Change States
    void ChangeState(BaseState newState)
    {
        _playerStateMachine.ChangeState(newState);
    }
}
