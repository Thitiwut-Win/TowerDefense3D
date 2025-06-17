using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : BaseUnit
{
    [SerializeField] private NavMeshAgent agent;
    private EnemyWaypoints enemyWaypoints;
    private float offset = 0.1f;
    private int waypointIndex = 0;
    private int lastWaypointIndex;
    private Transform currentWayPoint => enemyWaypoints.waypoints[waypointIndex].transform;
    private bool hasReachedLastWayPoint => waypointIndex >= lastWaypointIndex;
    private Transform target = null;
    private List<Transform> targetList = new List<Transform>();
    private bool isAttacking = false;
    private bool isHit = false;
    private bool isDead = false;
    private enum EnemyState
    {
        WALKING, AGGRO, ATTACKING
    }
    private EnemyState enemyState;
    void Start()
    {
        
    }
    void Update()
    {
        // print(hasReachedWaypoint() + " " + waypointIndex);
        if (enemyState == EnemyState.WALKING)
        {
            if (!hasTarget())
            {
                enemyState = EnemyState.AGGRO;
                Aggro();
            }
            if (hasReachedWaypoint()) GoToNextWaypoint();
        }
        else if (enemyState == EnemyState.AGGRO)
        {
            if (hasTarget())
            {
                enemyState = EnemyState.WALKING;
            }
            else if (IsInAttackRange())
            {
                agent.isStopped = true;
                enemyState = EnemyState.ATTACKING;
            }
        }
        else
        {
            if (hasTarget())
            {
                agent.isStopped = false;
                enemyState = EnemyState.WALKING;
                ContinueWaypoint();
            }
            else if (!IsInAttackRange())
            {
                agent.isStopped = false;
                enemyState = EnemyState.AGGRO;
                Aggro();
            }
            else if (!isAttacking) Attack();
        }
    }
    public override void Die()
    {
        PoolManager.Instance.Despawn(this);
    }
    protected virtual void Attack()
    {
        isAttacking = true;
        StartCoroutine(AttackCooldown());
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f / stat.attackSpeed);
        isAttacking = false;
    }
    private void GoToNextWaypoint()
    {
        if (!hasReachedLastWayPoint)
        {
            waypointIndex++;
            ContinueWaypoint();
        }
        else
        {
            Die();
            LevelManager.Instance.DecreaseLives(stat.lives);
        }
    }
    public void StartRunningWaypoints()
    {
        agent.SetDestination(currentWayPoint.position);
        lastWaypointIndex = enemyWaypoints.waypoints.Count - 1;
    }
    private void ContinueWaypoint()
    {
        agent.SetDestination(currentWayPoint.position);
    }
    private bool hasReachedWaypoint()
    {
        Vector3 cur = new Vector3(currentWayPoint.position.x, 0, currentWayPoint.position.z);
        Vector3 tar = new Vector3(transform.position.x, 0, transform.position.z);
        return Vector3.Distance(cur, tar) <= offset;
    }
    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, target.position) <= stat.attackRange;
    }
    public void Aggro()
    {
        agent.SetDestination(target.position);
    }
    private bool hasTarget()
    {
        if (target == null && targetList.Count > 0) target = targetList[0];
        return target == null;
    }
    public void SetWaypoint(EnemyWaypoints wp)
    {
        enemyWaypoints = wp;
    }
    public override void OnSpawn()
    {
        enemyState = EnemyState.WALKING;
    }
    public override void OnDespawn()
    {
        
    }
}