using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartLevelState : BaseState
{
    public static Action EnterStartLevelStateEvent;
    public static Action ExitStartLevelStateEvent;

    public override void Enter()
    {
        EnterStartLevelStateEvent?.Invoke();
        Debug.Log("Start Level State Entered");

        if (MenuData.StoryModeOn)
        {
            GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.CutsceneState);
        }
        else 
        {
            GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.PlayState);  
        }

    }

    public override void LogicUpdate()
    {
        
    }

    public override void Exit()
    {
        ExitStartLevelStateEvent?.Invoke();
        Debug.Log("Start Level State left");
    }
}
