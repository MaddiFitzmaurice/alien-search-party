using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AlienBase : MonoBehaviour
{
    [SerializeField]
    protected Transform target;
    protected NavMeshAgent navMeshAgent;
    protected float timer;

    public bool isUnderBeam;

    protected void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = target.position;
        isUnderBeam = false;
        timer = 0;
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
            if (timer > 5) 
            {
                Destroy();
            }
            else 
            {
                timer += Time.deltaTime;
            }
        }
    }

    protected abstract void Move();

    protected abstract void Abduct();

    protected void Destroy()
    {
        gameObject.SetActive(false);
    }
}

