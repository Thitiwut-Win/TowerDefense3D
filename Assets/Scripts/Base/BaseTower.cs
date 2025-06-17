using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour, IHitable
{
    public TowerStat towerStat;
    protected List<BaseEnemy> targetList = new List<BaseEnemy>();
    public void GetHit(float dmg)
    {

    }
    public void Die()
    {

    }
    protected void Attack()
    {

    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out BaseEnemy baseEnemy))
        {
            targetList.Add(baseEnemy);
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out BaseEnemy baseEnemy))
        {
            if(targetList.Contains(baseEnemy)) targetList.Remove(baseEnemy);
        }
    }
}

[System.Serializable]
public class TowerStat
{
    [SerializeField] private Projectile prefab;
    public float maxHealth;
    public float health;
    public float range;
    public float attackSpeed;
    public float attackDamage;
    public float cost;
}