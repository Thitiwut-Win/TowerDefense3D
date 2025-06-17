using UnityEngine;

public class BaseTower : MonoBehaviour, IHitable
{
    public void GetHit(float dmg)
    {

    }
    public void Die()
    {

    }
    protected void Attack()
    {

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