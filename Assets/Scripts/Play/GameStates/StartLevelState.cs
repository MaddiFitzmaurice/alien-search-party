using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartLevelState : BaseState
{
    public static Action EnterStartLevelStateEvent;

    public override void Enter()
    {
        EnterStartLevelStateEvent?.Invoke();
        Debug.Log("Start Level State Entered");
    }

    public override void LogicUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.PlayState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Start Level State left");
    }
}
