using System;
using UnityEngine;

public abstract class BaseUnit : PoolableObject, IHitable
{
    public UnitStat stat;
    public virtual void GetHit(float dmg)
    {
        stat.health -= Math.Max(dmg - stat.armor, 1);
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
    public float aggroRange;
    public float moveSpeed;
    public float armor;
    public float money;
    public int lives;
}