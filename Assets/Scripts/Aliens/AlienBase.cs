using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AlienBase : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    protected NavMeshAgent navMeshAgent;
    protected float timer;

    public bool isUnderBeam;

    protected void Awake()
    {
        navMeshAgent = new NavMeshAgent();
        navMeshAgent = GetComponent<NavMeshAgent>();
        isUnderBeam = false;
        timer = 0;
    }

    protected abstract void Move();

    protected void Abduct()
    {
        navMeshAgent.isStopped = true;
    }
    
    protected void Destroy()
    {
        gameObject.SetActive(false);
    }

    public void Reset(Transform _spawnPoint, Transform _destination)
    {
        gameObject.transform.position = _spawnPoint.position;
        gameObject.SetActive(true);
        navMeshAgent.SetDestination(_destination.position);
        isUnderBeam = false;
        navMeshAgent.isStopped = false;
        timer = 0;
    }

}

