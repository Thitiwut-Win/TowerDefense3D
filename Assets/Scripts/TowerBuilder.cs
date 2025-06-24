using UnityEngine;

public class TowerBuilder : Singleton<TowerBuilder>
{
    public void Build(BaseTower towerPrefab, Vector3 pos)
    {
        BaseTower baseTower = Instantiate(towerPrefab, pos, Quaternion.identity);
        baseTower.transform.SetParent(transform, true);
    }
}
