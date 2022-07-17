using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {

    }

    public virtual void OnTriggerStay(Collider other)
    {

    }

    public virtual void OnTriggerExit(Collider other)
    {

    }
}
