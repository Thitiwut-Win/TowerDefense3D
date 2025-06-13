using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseUnit
{
    [SerializeField] private List<Waypoint> waypoints;
    private float offset = 01f;
    private int waypointIndex = 0;
    private int lastWaypointIndex;
    private Transform currentWayPoint => waypoints[waypointIndex].transform;
    private bool hasReachedLastWayPoint => waypointIndex >= lastWaypointIndex;
    private SphereCollider _collider;
    void Update()
    {
        if (hasReachedWaypoint())
        {
            GoToNextWaypoint();
        }
    }
    public override void Die()
    {

    }
    private void MoveToTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction = Vector3.Normalize(direction);
    }
    private void GoToNextWaypoint()
    {
        MoveToTarget(currentWayPoint.transform.position);
        if (!hasReachedLastWayPoint)
        {
            waypointIndex++;
        }
        else
        {
            Die();
            LevelManager.Instance.DecreaseLives(stat.lives);
        }
    }
    private bool hasReachedWaypoint()
    {
        return Vector3.Distance(currentWayPoint.transform.position, transform.position) <= offset;
    }
    public override void OnSpawn()
    {
        _collider.enabled = true;
    }
    public override void OnDespawn()
    {
        _collider.enabled = false;
    }
}