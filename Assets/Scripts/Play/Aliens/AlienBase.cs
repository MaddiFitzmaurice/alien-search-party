using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class AlienBase : MonoBehaviour
{
    public static Action<int> AlienSpawnEvent;
    public static Action<int> AlienAbductEvent;
    public static Action<GameObject> AlienReachedDestEvent;
    private SkinnedMeshRenderer _meshRenderer;
    protected Collider Collider;
    protected NavMeshAgent NavMeshAgent;
    
    [SerializeField]
    protected float AbductTime;
    [SerializeField]
    protected float Speed;

    [SerializeField]
    protected Animator AlienAnim;
    [SerializeField]
    protected AudioSource AudioSource;
    [SerializeField]
    protected AudioClip SpawnSound;

    private Vector3 _endRotation = new Vector3(0, 180, 0);

    // Aliens active in scene
    private static int _active = 0;

    private bool _canAbduct;
    private bool _wasAbducted;

    protected void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public virtual void OnEnable()
    {   
        // Avoid duplicate addition
        if (!_wasAbducted)
        {
            _active++;
        }

        Reveal(false);
    }

    public virtual void OnDisable()
    {
        // Avoid duplicate subtraction
        if (!_wasAbducted)
        {
            _active--;
        }

        CancelInvoke();
    }

    public void Spawn(Transform spawnPoint, Transform destination)
    {
        gameObject.transform.position = spawnPoint.position;
        gameObject.SetActive(true);
        NavMeshAgent.SetDestination(destination.position);
        Move();
    }

    protected virtual void Move()
    {
        CancelInvoke();
        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = Speed;
        AlienAnim.SetFloat("Speed", NavMeshAgent.speed);
        AlienAnim.SetBool("isAbducted", NavMeshAgent.isStopped);
    }

    protected virtual void StopMoving()
    {
        NavMeshAgent.isStopped = true;
        AlienAnim.SetFloat("Speed", NavMeshAgent.speed);
        AlienAnim.SetBool("isAbducted", NavMeshAgent.isStopped);
        Invoke("Abducted", AbductTime);
    }

    protected void Abducted()
    {
        _wasAbducted = true;
        _active--;
        AlienAbductEvent?.Invoke(GetActiveAliensNum());
        gameObject.SetActive(false);
    }

    // When Alien reaches destination
    protected void ReachedTarget()
    {
        NavMeshAgent.isStopped = true;
        AlienAnim.SetBool("isStopped", true);
        transform.eulerAngles = _endRotation;
        AlienReachedDestEvent?.Invoke(this.gameObject);
        GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.EndLevelState);
    }

    public void OnTriggerEnter(Collider other)
    {
        // If Alien is under Player's beam
        if (other.tag == "Player" && _canAbduct)
        {
            StopMoving();
        }
        
        // If Alien reaches an end point
        if (other.tag == "EndPoint")
        {
            ReachedTarget();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Reveal Alien when it leaves spawn point
        if (other.tag == "SpawnPoint")
        {
            if (_wasAbducted)
            {
                _active++;
                _wasAbducted = false;
            }

            Reveal(true);
            AudioSource.PlayOneShot(SpawnSound);
            AlienSpawnEvent?.Invoke(GetActiveAliensNum());
        }

        // If Alien is no longer under Player's beam
        if (other.tag == "Player")
        {
            Move();
        }
    }

    public int GetActiveAliensNum()
    {
        return _active;
    }

    public void Reveal(bool reveal)
    {
        _meshRenderer.enabled = reveal;
        _canAbduct = reveal;
    }
}

