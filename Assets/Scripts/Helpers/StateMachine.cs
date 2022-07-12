using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    public BaseState currentState { get; private set; }

    public StateMachine(BaseState _startState)
    {
        Assert.IsNotNull(_startState);
        currentState = _startState;
    }

    public void ChangeState(BaseState _newState)
    {
        Assert.IsNotNull(_newState);
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
