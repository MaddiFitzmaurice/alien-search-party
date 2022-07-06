using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AlienBase : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected float timer;
    
    // If Alien has reached target
    public bool targetReached;

    // If Alien is under player's UFO abduction beam
    public bool isUnderBeam;

    public float detectionTime;

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        isUnderBeam = false;
        targetReached = false;
        timer = 0;
    }

    protected abstract void Move();

    protected void Abduct()
    {
        navMeshAgent.isStopped = true;
    }

    protected void ReachedTarget()
    {
        Invoke("Destroy", detectionTime);
    }

    protected void OnDisable()
    {
        CancelInvoke();
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
        targetReached = false;
        navMeshAgent.isStopped = false;
        timer = 0;
    }
}

