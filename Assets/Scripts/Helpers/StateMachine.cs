using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    public BaseState CurrentState { get; private set; }

    public StateMachine(BaseState startState)
    {
        Assert.IsNotNull(startState);
        CurrentState = startState;
    }

    public void ChangeState(BaseState newState)
    {
        Assert.IsNotNull(newState);
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
