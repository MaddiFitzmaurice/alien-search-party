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
    }
}
