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
    public float time;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = target.position;
        isUnderBeam = false;
        time = 0;
    }

    void Update()
    {
        if (!isUnderBeam) 
        {
            navMeshAgent.isStopped = false;
        }
        else 
        {
            navMeshAgent.isStopped = true;
            if (time > 5) 
            {
                Destroy();
            }
            else 
            {
                time += Time.deltaTime;
            }
        }
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
