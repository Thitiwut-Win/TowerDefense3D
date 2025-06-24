using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : BaseUnit
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    private EnemyWaypoints enemyWaypoints;
    private float offset = 0.1f;
    private int waypointIndex = 0;
    private int lastWaypointIndex;
    private Transform currentWayPoint => enemyWaypoints.waypoints[waypointIndex].transform;
    private bool hasReachedLastWayPoint => waypointIndex >= lastWaypointIndex;
    [SerializeField] private Transform target = null;
    public List<Transform> targetList = new List<Transform>();
    private BoolTime isAttacking = new BoolTime();
    private BoolTime attackCD = new BoolTime();
    private BoolTime isHit = new BoolTime();
    private bool isDead = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip attackAudioClip;
    [SerializeField] private AudioClip getHitAudioClip;
    public float attackSoundOffset;
    public float getHitSoundOffset;
    private enum EnemyState
    {
        NONE, WALKING, AGGRO, ATTACKING
    }
    private EnemyState enemyState;
    void Start()
    {
        isAttacking.time = stat.attackTime;
        isHit.time = stat.dazeTime;
        attackCD.time = 1f / stat.attackSpeed;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (audioSource.clip != null && audioSource.time > 2.1f) audioSource.Stop();
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
            else if (hasReachedTarget())
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
                print(Vector3.Distance(transform.position, target.position));
                agent.isStopped = false;
                enemyState = EnemyState.AGGRO;
                Aggro();
            }
            else if (!attackCD.isWait)
            {
                isAttacking.isWait = true;
                attackCD.isWait = true;
                Attack();
            }
        }
        if (animator != null) SetAnimatorParameter();
    }
    public override void GetHit(float dmg)
    {
        isHit.isWait = true;
        audioSource.volume = LevelManager.Instance.GetVolume()/400f;
        audioSource.clip = getHitAudioClip;
        audioSource.pitch = UnityEngine.Random.Range(1f, 3f);
        audioSource.time = getHitSoundOffset;
        audioSource.Play();
        StartCoroutine(Cooldown(isHit));
        base.GetHit(dmg);
    }
    private IEnumerator Cooldown(BoolTime boolTime)
    {
        yield return new WaitForSeconds(boolTime.time);
        boolTime.isWait = false;
    }
    public override void Die()
    {
        isAttacking.isWait = false;
        isDead = true;
        PoolManager.Instance.Despawn(this);
    }
    protected virtual void Attack()
    {
        if (target != null)
        {
            BaseTower baseTower = target.GetComponent<BaseTower>();
            baseTower.GetHit(stat.attackDamage);
        }
        audioSource.volume = LevelManager.Instance.GetVolume()/300f;
        audioSource.clip = attackAudioClip;
        audioSource.pitch = UnityEngine.Random.Range(1f, 3f);
        audioSource.time = attackSoundOffset;
        audioSource.Play();
        StartCoroutine(Cooldown(isAttacking));
        StartCoroutine(Cooldown(attackCD));
    }
    private void GoToNextWaypoint()
    {
        if (!hasReachedLastWayPoint)
        {
            waypointIndex++;
            ContinueWaypoint();
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
        if (target == null && targetList.Count > 0)
        {
            for (int i = 0; i < targetList.Count && target == null; i++)
            {
                target = targetList[i];
            }
        }
        return target == null;
    }
    private bool hasReachedTarget()
    {
        return Vector3.Distance(transform.position, target.position) <= 1.5f;
    }
    public void SetWaypoint(EnemyWaypoints wp)
    {
        enemyWaypoints = wp;
    }
    public bool IsDead()
    {
        return isDead;
    }
    public override void OnSpawn()
    {
        enemyState = EnemyState.WALKING;
        isAttacking.isWait = false;
        attackCD.isWait = false;
        isHit.isWait = false;
        isDead = false;
        stat.health = stat.maxHealth;
    }
    public override void OnDespawn()
    {
        StopAllCoroutines();
        isDead = true;
        target = null;
        targetList.Clear();
        audioSource.Stop();
    }
    private void SetAnimatorParameter()
    {
        animator.SetBool("IsWalking", enemyState != EnemyState.ATTACKING);
        animator.SetBool("IsAttacking", isAttacking.isWait);
        animator.SetBool("AttackCD", attackCD.isWait);
        animator.SetBool("IsHit", isHit.isWait);
        animator.SetBool("IsDead", isDead);
    }
}

[System.Serializable]
public class BoolTime
{
    public bool isWait;
    public float time;
}