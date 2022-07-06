using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGrey : AlienBase
{
     [SerializeField]
    private int abductTime;
    [SerializeField]
    private float speed;

    void Update()
    {
        if (!isUnderBeam)
        {
            if (!targetReached)
            {
                if (navMeshAgent.remainingDistance < 0.5f)
                {
                    targetReached = true;
                    ReachedTarget();
                }
                else 
                {
                    Move();
                    timer = 0;
                }
            }
        }
        else
        {
            if (timer < abductTime)
            {
                Abduct();
            }
            else 
            {
                Destroy();
            }

            timer += Time.deltaTime;
        }
    }

    protected override void Move()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
}
