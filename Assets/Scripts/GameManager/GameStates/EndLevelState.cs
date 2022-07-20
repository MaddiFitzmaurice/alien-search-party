using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelState : BaseState
{
    public delegate void EndLevelStateEvent();
    public delegate void EndLevelWinStateEvent();
    public delegate void EndLevelLoseStateEvent();
    public event EndLevelStateEvent EnterEndLevelState;
    public event EndLevelWinStateEvent EnterEndLevelWinState;
    public event EndLevelLoseStateEvent EnterEndLevelLoseState;
    public override void Enter()
    {
        if (EnterEndLevelWinState != null && !GameManager.Instance.PlayState.Failed)
        {
            EnterEndLevelWinState();
        }
        else if (EnterEndLevelLoseState != null && GameManager.Instance.PlayState.Failed)
        {
            EnterEndLevelLoseState();
        }

        if (EnterEndLevelState != null)
        {
            EnterEndLevelState();
        }


        Debug.Log("End Level State Entered");
    }

    public override void LogicUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.StartLevelState);
        }
    }

    public override void Exit()
    {
        if (EnterEndLevelWinState != null && !GameManager.Instance.PlayState.Failed)
        {
            EnterEndLevelWinState();
        }
        else if (EnterEndLevelLoseState != null && GameManager.Instance.PlayState.Failed)
        {
            EnterEndLevelLoseState();
        }
        Debug.Log("End Level State Left");
    }
}
