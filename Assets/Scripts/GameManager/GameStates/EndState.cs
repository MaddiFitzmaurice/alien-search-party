using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : BaseState
{
    public delegate void EndStateEvent();
    public event EndStateEvent EnterEndState;
    public override void Enter()
    {
        if (EnterEndState != null)
        {
            EnterEndState();
        }
        Debug.Log("End State entered");
    }

    public override void LogicUpdate()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("End State left");
    }
}
