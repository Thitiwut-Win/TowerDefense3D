using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    [SerializeField] private BaseEnemy baseEnemy;
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out BaseTower baseTower))
        {
            baseEnemy.targetList.Add(baseTower.transform);
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out BaseTower baseTower))
        {
            if(baseEnemy.targetList.Contains(baseTower.transform)) baseEnemy.targetList.Remove(baseTower.transform);
        }
    }
}