using UnityEngine;

public class BaseTower : MonoBehaviour, IHitable
{
    public void GetHit(float dmg)
    {

    }
    public void Die()
    {
        
    }
}

[System.Serializable]
public class TowerStat
{
    public float maxHealth;
    public float health;
    public float range;
    public float attackSpeed;
    public float attackDamage;
    public float cost;
}