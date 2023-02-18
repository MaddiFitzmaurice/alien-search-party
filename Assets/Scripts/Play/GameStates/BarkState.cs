using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarkState : BaseState
{
    public static Action EnterBarkStateEvent;
    public static Action ExitBarkStateEvent;

    public override void Enter()
    {
        EnterBarkStateEvent?.Invoke();
    }

    public override void Exit()
    {
        ExitBarkStateEvent?.Invoke();
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
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    public override void PhysicsUpdate()
    {

    }
}
