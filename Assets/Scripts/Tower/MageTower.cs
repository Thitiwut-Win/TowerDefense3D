using UnityEngine;

public class MageTower : BaseTower
{
    [SerializeField] private Particle particle;
    protected override void Update()
    {
        base.Update();
        if (rotateToTarget.hasTarget() && !isAttacking)
        {
            isAttacking = true;
            PoolManager.Instance.Spawn<Particle>(particle.name, targetList[0].transform.position, Quaternion.identity);
            rotateToTarget.Attack(null, 0.25f);
            StartCoroutine(AttackCountdown());
        }
    }
}