using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPlayState : BaseState
{
    public delegate void PlayStateEvent();
    public event PlayStateEvent EnterNoPlayState;
    public override void Enter()
    {
        if (EnterNoPlayState != null)
        {
            EnterNoPlayState();
        }
        Debug.Log("NoPlay State entered");
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
        Debug.Log("NoPlay State left");
    }
}
