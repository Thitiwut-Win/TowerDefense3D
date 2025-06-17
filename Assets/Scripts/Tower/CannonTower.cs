using UnityEngine;

public class CannonTower : BaseTower
{
    [SerializeField] private RotateToTarget rotateToTarget;
    void Update()
    {
        if (!rotateToTarget.hasTarget() && targetList.Count > 0) rotateToTarget.SetTarget(targetList[0].transform);
        else if (rotateToTarget.hasTarget() && targetList.Count == 0) rotateToTarget.SetTarget(null);
    }
}