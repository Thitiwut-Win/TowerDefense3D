using UnityEngine;

public class TowerRange : MonoBehaviour
{
    [SerializeField] BaseTower baseTower;
    [SerializeField] private LayerMask layerMask;
    public void OnTriggerEnter(Collider collider)
    {
        Vector3 direc = transform.position - collider.transform.position;
        if (collider.TryGetComponent(out BaseEnemy baseEnemy) && baseEnemy.stat.eTeam != baseTower.towerStat.eTeam)
        {
            if (!Physics.Raycast(baseTower.rotateToTarget.transform.position, direc, baseTower.towerStat.range, layerMask))
            {
                baseTower.targetList.Add(baseEnemy);
                baseTower.ReTarget();
            }
        }
    }
    public void OnTriggerStay(Collider collider)
    {
        if (collider.TryGetComponent(out BaseEnemy baseEnemy) && baseEnemy.stat.eTeam != baseTower.towerStat.eTeam)
        {
            Vector3 direc = collider.transform.position - transform.position;
            direc.y = 0.5f - transform.position.y;
            // Debug.DrawRay(baseTower.rotateToTarget.transform.position, direc, Color.cyan);
            if (baseTower.targetList.Contains(baseEnemy))
            {
                if (Physics.Raycast(baseTower.rotateToTarget.transform.position, direc, baseTower.towerStat.range, layerMask))
                {
                    baseTower.targetList.Remove(baseEnemy);
                    baseTower.ReTarget();
                }
            }
            else
            {
                if (!Physics.Raycast(baseTower.rotateToTarget.transform.position, direc, baseTower.towerStat.range, layerMask))
                {
                    baseTower.targetList.Add(baseEnemy);
                    baseTower.ReTarget();
                }
            }
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out BaseEnemy baseEnemy) && baseEnemy.stat.eTeam != baseTower.towerStat.eTeam)
        {
            if (baseTower.targetList.Contains(baseEnemy))
            {
                baseTower.targetList.Remove(baseEnemy);
                baseTower.ReTarget();
            }
        }
    }
}