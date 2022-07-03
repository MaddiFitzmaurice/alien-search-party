using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienMove : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navMeshAgent.destination = target.position;
    }
}
