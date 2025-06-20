using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour, IHitable
{
    public TowerStat towerStat;
    [SerializeField] public RotateToTarget rotateToTarget;
    public List<BaseEnemy> targetList = new List<BaseEnemy>();
    protected bool isAttacking = false;
    protected bool isDead = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = LevelManager.Instance.GetVolume()/100f;
        audioSource.pitch = Random.Range(1f, 3f);
        audioSource.clip = audioClip;
        audioSource.time = 0.5f;
        audioSource.Play();
    }
    protected virtual void Update()
    {
        if (targetList.Count > 0 && targetList[0].IsDead())
        {
            targetList.RemoveAt(0);
            ReTarget();
        }
        if (!rotateToTarget.hasTarget() && targetList.Count > 0) rotateToTarget.SetTarget(targetList[0]);
        else if (rotateToTarget.hasTarget() && targetList.Count == 0) ReTarget();
    }
    public void GetHit(float dmg)
    {
        towerStat.health -= dmg;
        if (towerStat.health <= 0 && !isDead) Die();
    }
    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
    protected IEnumerator AttackCountdown()
    {
        yield return new WaitForSeconds(1f / towerStat.attackSpeed);
        isAttacking = false;
    }
    public void ReTarget()
    {
        rotateToTarget.SetTarget(null);
    }
}

[System.Serializable]
public class TowerStat
{
    [SerializeField] public Projectile prefab;
    public float maxHealth;
    public float health;
    public float range;
    public float attackSpeed;
    public float attackDamage;
    public int cost;
    public ETeam eTeam;
}