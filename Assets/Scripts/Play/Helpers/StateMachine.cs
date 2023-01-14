using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    public BaseState CurrentState { get; private set; }

    public StateMachine()
    {
        CurrentState = null;
    }

    public void ChangeState(BaseState newState)
    {
        Assert.IsNotNull(newState);
        if (CurrentState != null)
        {        
            CurrentState.Exit();
        }
        CurrentState = newState;
        CurrentState.Enter();
    }
}
