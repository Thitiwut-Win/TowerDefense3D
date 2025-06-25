using System.Collections.Generic;
using UnityEngine;

public class TowerBuilderManager : Singleton<TowerBuilderManager>
{
    private List<BaseTower> towers = new List<BaseTower>();
    private Dictionary<BaseTower, BaseTower> prefabDict = new Dictionary<BaseTower, BaseTower>();
    public void Build(BaseTower towerPrefab, Vector3 pos)
    {
        BaseTower baseTower = Instantiate(towerPrefab, pos, Quaternion.identity);
        baseTower.transform.SetParent(transform, true);
        towers.Add(baseTower);
        prefabDict[baseTower] = towerPrefab;
    }
    #region Save & Load
    public void Save(ref SceneTowerData data)
    {
        data.Towers = new List<TowerSaveData>();
        for (int i = 0; i < towers.Count; i++)
        {
            TowerSaveData saveData = new TowerSaveData
            {
                Position = towers[i].transform.position,
                Health = towers[i].towerStat.health,
                TowerPrefab = prefabDict[towers[i]]
            };
            data.Towers.Add(saveData);
        }
    }
    public void Load(SceneTowerData data)
    {
        foreach (BaseTower baseTower in towers)
        {
            if (baseTower != null) Destroy(baseTower.gameObject);
        }
        towers.Clear();
        prefabDict.Clear();

        foreach (TowerSaveData saveData in data.Towers)
        {
            if (saveData.TowerPrefab != null)
            {
                BaseTower baseTower = Instantiate(saveData.TowerPrefab, saveData.Position, Quaternion.identity);
                baseTower.towerStat.health = saveData.Health;
                baseTower.transform.SetParent(transform, true);
                towers.Add(baseTower);
                prefabDict[baseTower] = saveData.TowerPrefab;
            }
        }
    }
    #endregion
}

[System.Serializable]
public struct TowerSaveData
{
    public Vector3 Position;
    public float Health;
    public BaseTower TowerPrefab;
}
[System.Serializable]
public struct SceneTowerData
{
    public List<TowerSaveData> Towers;
}