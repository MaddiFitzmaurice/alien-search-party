using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndLevelState : BaseState
{
    public static Action EnterEndLevelStateEvent;
    public static Action<int> ShowEndLevelScreenEvent;

    private int _screenDisplayed;

    public override void Enter()
    { 
        EnterEndLevelStateEvent?.Invoke();

        Debug.Log("End Level State Entered");
    }

    public override void LogicUpdate()
    {
    }

    public override void Exit()
    {        
        Debug.Log("End Level State Left");
    }
}
