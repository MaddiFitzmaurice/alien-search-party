using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGrey : AlienBase
{
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
            if (Timer < AbductTime)
            {
                Abduct();
            }
            else if (Collider.enabled)
            {
                Caught();
            }

            Timer += Time.deltaTime;
        }
    }
}
