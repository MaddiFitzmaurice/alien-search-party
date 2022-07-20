using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelState : BaseState
{
    public delegate void StartLevelStateEvent();
    public event StartLevelStateEvent EnterStartLevelState;
    public override void Enter()
    {
        if (EnterStartLevelState != null)
        {
            EnterStartLevelState();
        }
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
