using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Human : Abductee
{
    private Transform[] _wayPoints;
    private int _currentWayPoint;

    void Start()
    {
        _currentWayPoint = 0;
    }

    public void OnDisable()
    {
        NavAgent.isStopped = true;
    }

    void Update()
    {
        if (NavAgent.remainingDistance < 0.1)
        {
            UpdateCurrentWayPoint();
            GoToNewDestination();
        }
    }

    public override void Spawn(Transform spawnPoint, Transform destination)
    {
        Transform[] wayPoints = destination.GetComponentsInChildren<Transform>();
        SetWayPoints(wayPoints);
        base.Spawn(spawnPoint, wayPoints[0]);
        SpawnEvent?.Invoke();
    }

    // Humans have multiple destinations to run to
    public void SetWayPoints(Transform[] wayPoints)
    {
        _wayPoints = wayPoints;
    }

    public void UpdateCurrentWayPoint()
    {
        _currentWayPoint++;
        if (_currentWayPoint == _wayPoints.Length)
        {
            _currentWayPoint = 0;
        }
    }

    public void GoToNewDestination()
    {
        NavAgent.destination = _wayPoints[_currentWayPoint].transform.position;
    }
}
