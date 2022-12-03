using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGreen : Alien
{
    [SerializeField]
    private int _resistTime;
    [SerializeField]
    private float _resistSpeed;

    protected override void UnderBeam()
    {
        NavAgent.speed = _resistSpeed;
        Animator.SetFloat("Speed", NavAgent.speed);
        Invoke("StopResisting", _resistTime);
    }

    private void StopResisting()
    {
        StopMoving();
        Invoke("Abducted", AbductTime);
    }
}
