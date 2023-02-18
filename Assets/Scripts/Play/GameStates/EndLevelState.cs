using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndLevelState : BaseState
{
    public static Action EnterEndLevelStateEvent;

    private int _screenDisplayed;

    public override void Enter()
    { 
        EnterEndLevelStateEvent?.Invoke();
    }

    public override void LogicUpdate()
    {
    }

    public override void Exit()
    {     
           
    }
}
