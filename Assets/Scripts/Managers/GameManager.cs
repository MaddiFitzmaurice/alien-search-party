using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern so everything has access to this script
    public static GameManager instance { get; private set; }

    public StateMachine gmStateMachine;

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this);
            return;
        }
    }

    void Start()
    {
        //gmStateMachine = new StateMachine();
    }

    void Update()
    {
        gmStateMachine.currentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        gmStateMachine.currentState.PhysicsUpdate();
    }
}
