using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CutsceneState : BaseState
{
    public static Action EnterCutsceneStateEvent;
    public static Action ExitCutsceneStateEvent;

    public override void Enter()
    {
        EnterCutsceneStateEvent?.Invoke();
    }

    public override void Exit()
    {
        ExitCutsceneStateEvent?.Invoke();
    }

    public override void LogicUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.PlayState);  
        }
    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    public override void OnTriggerExit(Collider other)
    {

    }

    public override void OnTriggerStay(Collider other)
    {

    }

    public override void PhysicsUpdate()
    {

    }
}
