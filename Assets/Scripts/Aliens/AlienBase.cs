using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AlienBase : MonoBehaviour
{
    protected NavMeshAgent NavMeshAgent;
    protected float Timer;
    [SerializeField]
    protected float AbductTime;
    [SerializeField]
    protected float Speed;
    
    // If Alien has reached target
    [HideInInspector]
    public bool TargetReached;
    [HideInInspector]
    // If Alien is under player's UFO abduction beam
    public bool IsUnderBeam;

    public float DetectionTime;

    protected void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        IsUnderBeam = false;
        TargetReached = false;
        Timer = 0;
    }

    protected virtual void Move()
    {
        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = Speed;
    }

    protected void Abduct()
    {
        NavMeshAgent.isStopped = true;
    }

    protected void ReachedTarget()
    {
        Invoke("WasDetected", DetectionTime);
        Invoke("Destroy", DetectionTime);
    }

    protected void WasDetected()
    {
        GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.EndState);
    }

    protected void OnDisable()
    {
        CancelInvoke();
    }
    
    protected void Destroy()
    {
        gameObject.SetActive(false);
    }

    public void Reset(Transform spawnPoint, Transform destination)
    {
        gameObject.transform.position = spawnPoint.position;
        gameObject.SetActive(true);
        NavMeshAgent.SetDestination(destination.position);
        IsUnderBeam = false;
        TargetReached = false;
        NavMeshAgent.isStopped = false;
        Timer = 0;
    }
}

