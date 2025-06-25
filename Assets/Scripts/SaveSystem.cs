using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static SaveData _saveData;
    [System.Serializable]
    public struct SaveData
    {
        public LevelSaveData levelSaveData;
        public SceneTowerData sceneTowerData;
    }
    public static string SaveName()
    {
        string saveFile = "D:\\UnitySave\\" + Application.productName + "\\save.save";
        return saveFile;
    }
    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveName(), JsonUtility.ToJson(_saveData, true));
    }
    private static void HandleSaveData()
    {
        LevelManager.Instance.Save(ref _saveData.levelSaveData);
        TowerBuilderManager.Instance.Save(ref _saveData.sceneTowerData);
    }
    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveName());
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);

        HandleLoadData();
    }
    private static void HandleLoadData()
    {
        LevelManager.Instance.Load(_saveData.levelSaveData);
        TowerBuilderManager.Instance.Load(_saveData.sceneTowerData);
    }
}