using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGreen : AlienBase
{
    [SerializeField]
    private int _resistTime;
    [SerializeField]
    private float _resistSpeed;

    protected override void StopMoving()
    {
        NavMeshAgent.speed = _resistSpeed;
        AlienAnim.SetFloat("Speed", NavMeshAgent.speed);
        Invoke("StopResisting", _resistTime);
    }

    private void StopResisting()
    {
        NavMeshAgent.isStopped = true;
        AlienAnim.SetFloat("Speed", NavMeshAgent.speed);
        AlienAnim.SetBool("isAbducted", NavMeshAgent.isStopped);
        Invoke("Abducted", AbductTime);
    }
}
