using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienMove : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private NavMeshAgent navMeshAgent;

    public bool isUnderBeam;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        isUnderBeam = false;
    }

    void Update()
    {
        if (!isUnderBeam) 
        {
            navMeshAgent.destination = target.position;
        }
        else {
            navMeshAgent.isStopped = true;
        }
    }
}
