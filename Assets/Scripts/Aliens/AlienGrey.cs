using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGrey : AlienBase
{
     [SerializeField]
    private int _abductTime;
    [SerializeField]
    private float _speed;

    void Update()
    {
        if (!IsUnderBeam)
        {
            if (!TargetReached)
            {
                if (NavMeshAgent.remainingDistance < 0.5f)
                {
                    TargetReached = true;
                    ReachedTarget();
                }
                else 
                {
                    Move();
                    Timer = 0;
                }
            }
        }
        else
        {
            if (Timer < _abductTime)
            {
                Abduct();
            }
            else 
            {
                Destroy();
            }

            Timer += Time.deltaTime;
        }
    }

    protected override void Move()
    {
        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = _speed;
    }
}
