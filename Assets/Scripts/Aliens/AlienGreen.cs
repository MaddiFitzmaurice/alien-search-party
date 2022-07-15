using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGreen : AlienBase
{
    [SerializeField]
    private int _resistTime;
    [SerializeField]
    private float _resistSpeed;

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
            if (Timer < _resistTime)
            {
                Resist();
            }
            else if (Timer > _resistTime && Timer < _resistTime + AbductTime)
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

    private void Resist()
    {
        NavMeshAgent.speed = _resistSpeed;
    }
}
