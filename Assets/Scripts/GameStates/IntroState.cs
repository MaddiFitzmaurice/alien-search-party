using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : BaseState
{
    public override void Enter()
    {
        Debug.Log("Introduction State entered");
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
        Debug.Log("Introduction State left");
    }
}
