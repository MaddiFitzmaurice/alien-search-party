using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class Alien : Abductee
{
    public static Action<GameObject> AlienReachedDestEvent;

    [SerializeField]
    protected AudioSource AudioSource;

    private Vector3 _endRotation = new Vector3(0, 180, 0);

    private bool _canAbduct;
    private bool _wasAbducted;

    public virtual void OnEnable()
    {   
        Reveal(false);
    }

    public void Reveal(bool reveal)
    {
        MeshRenderer.enabled = reveal;
        _canAbduct = reveal;
    }

    protected override void Abducted()
    {
        base.Abducted();
        _wasAbducted = true;
    }

    // When Alien reaches destination
    protected void ReachedTarget()
    {
        NavAgent.isStopped = true;
        Animator.SetBool("isStopped", true);
        Animator.SetFloat("Speed", NavAgent.speed);
        transform.eulerAngles = _endRotation;
        AlienReachedDestEvent?.Invoke(this.gameObject);
        GameManager.Instance.GMStateMachine.ChangeState(GameManager.Instance.EndLevelState);
    }

    public override void OnTriggerEnter(Collider other)
    {
        // If Alien is under Player's beam
        if (other.tag == "Player" && _canAbduct)
        {
            UnderBeam();
        }
        
        // If Alien reaches an end point
        if (other.tag == "EndPoint")
        {
            ReachedTarget();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        // Reveal Alien when it leaves spawn point
        if (other.tag == "SpawnPoint")
        {
            if (_wasAbducted)
            {
                _wasAbducted = false;
            }

            Reveal(true);
            AudioSource.Play();
            SpawnEvent?.Invoke();
        }
    }
}

