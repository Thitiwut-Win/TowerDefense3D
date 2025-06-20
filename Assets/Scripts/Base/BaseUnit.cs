using System;
using UnityEngine;

public abstract class BaseUnit : PoolableObject, IHitable
{
    public UnitStat stat;
    public virtual void GetHit(float dmg)
    {
        stat.health -= Math.Max(dmg - stat.armor, 1);
        if (stat.health <= 0)
        {
            Die();
            LevelManager.Instance.IncreaseMoney(stat.money);
        }
    }
    public abstract void Die();
}

[System.Serializable]
public class UnitStat
{
    public float maxHealth;
    public float health;
    public float attackSpeed;
    public float attackDamage;
    public float attackRange;
    public float attackTime;
    public float aggroRange;
    public float moveSpeed;
    public float armor;
    public int money;
    public int lives;
    public float dazeTime;
    public ETeam eTeam;
}