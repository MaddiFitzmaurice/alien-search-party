using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Abductee : MonoBehaviour
{
    public static Action SpawnEvent;
    public static Action AbductEvent;

    // Abductee data
    [SerializeField]
    protected float AbductTime;

    [SerializeField]
    protected float Speed;

    protected Animator Animator;

    protected NavMeshAgent NavAgent;

    protected SkinnedMeshRenderer MeshRenderer;

    public void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        MeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Animator = GetComponentInChildren<Animator>();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public virtual void Spawn(Transform spawnPoint, Transform destination)
    {
        gameObject.transform.position = spawnPoint.position;
        gameObject.SetActive(true);
        NavAgent.SetDestination(destination.position);
        Move();
    }

    protected virtual void Move()
    {
        CancelInvoke();
        NavAgent.isStopped = false;
        NavAgent.speed = Speed;
        Animator.SetFloat("Speed", NavAgent.speed);
        Animator.SetBool("isAbducted", NavAgent.isStopped);
    }

    protected virtual void StopMoving()
    {
        NavAgent.isStopped = true;
        Animator.SetFloat("Speed", NavAgent.speed);
        Animator.SetBool("isAbducted", NavAgent.isStopped);
    }

    protected virtual void UnderBeam()
    {
        StopMoving();
        Invoke("Abducted", AbductTime);
    }

    protected virtual void Abducted()
    {
        AbductEvent?.Invoke();
        gameObject.SetActive(false);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // If Abductee is under Player's beam
        if (other.tag == "Player")
        {
            UnderBeam();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Move();
        }
    }
}
