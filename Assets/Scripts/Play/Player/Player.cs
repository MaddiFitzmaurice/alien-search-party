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
    [HideInInspector]
    public Animator PlayerAnim;

    public CapsuleCollider BeamTrigger;

    public AudioSource AudioSourceEngine;
    public AudioSource AudioSourceBeam;
    public AudioSource AudioSourceAbduct;

    // State Machine and States
    private StateMachine _playerStateMachine;
    private PlayerControlState _playerControlState;
    private PlayerNoControlState _playerNoControlState;

    // Player Data
    public float MoveSpeed;
    public float BeamSpeed;

    [SerializeField]
    private Vector3 _resetPosition; 

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

    void OnEnable()
    {
        // Set up event triggers for state changes
        StartLevelState.EnterStartLevelStateEvent += EnterPlayerNoControlState;
        StartLevelState.EnterStartLevelStateEvent += ResetPlayerPosition;
        PlayState.EnterPlayStateEvent += EnterPlayerControlState;
        EndLevelState.EnterEndLevelStateEvent += EnterPlayerNoControlState;
    }

    void OnDisable()
    {
        // Unsubscribe from event triggers
        StartLevelState.EnterStartLevelStateEvent -= EnterPlayerNoControlState;
        StartLevelState.EnterStartLevelStateEvent -= ResetPlayerPosition;
        PlayState.EnterPlayStateEvent -= EnterPlayerControlState;
        EndLevelState.EnterEndLevelStateEvent -= EnterPlayerNoControlState;
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

    public void DoCoroutine(string name)
    {
        StartCoroutine(name);
    }

    public void CancelCoroutine(string name)
    {
        StopCoroutine(name);
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

    void ResetPlayerPosition()
    {
        transform.position = _resetPosition;
    }
}
