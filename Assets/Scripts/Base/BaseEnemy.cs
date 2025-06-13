using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : BaseUnit
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Waypoint> waypoints;
    private float offset = 0.1f;
    private int waypointIndex = 0;
    private int lastWaypointIndex;
    private Transform currentWayPoint => waypoints[waypointIndex].transform;
    private bool hasReachedLastWayPoint => waypointIndex >= lastWaypointIndex;
    private SphereCollider _collider;
    [SerializeField] private SphereCollider aggroCollider;
    private Transform target = null;
    private enum EnemyState
    {
        WALKING, ATTACKING
    }
    private EnemyState enemyState;
    void Start()
    {
        _collider = GetComponent<SphereCollider>();
    }
    void Update()
    {
        if (enemyState == EnemyState.WALKING)
        {
            if (target == null && hasReachedWaypoint()) GoToNextWaypoint();
            else if (target != null && IsInAttackRange()) enemyState = EnemyState.ATTACKING;
        }
        else
        {
            if (target != null)
            {
                Attack();
            }
            else enemyState = EnemyState.WALKING;
        }
    }
    public override void Die()
    {

    }
    public virtual void Attack()
    {

    }
    private void GoToNextWaypoint()
    {
        if (!hasReachedLastWayPoint)
        {
            waypointIndex++;
            agent.SetDestination(currentWayPoint.transform.position);
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
    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, target.position) <= stat.attackRange;
    }
    public void Aggro(Transform t)
    {
        target = t;
        agent.SetDestination(target.position);
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