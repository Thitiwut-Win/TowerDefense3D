using UnityEngine;

public class CannonTower : BaseTower
{
    protected override void Update()
    {
        base.Update();
        if (rotateToTarget.hasTarget() && !isAttacking)
        {
            isAttacking = true;
            rotateToTarget.Attack(towerStat.prefab);
            StartCoroutine(AttackCountdown());
        }
    }
}