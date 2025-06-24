using UnityEngine;
using UnityEngine.UI;

public class BuildUI : Singleton<BuildUI>
{
    [Header("Cannon Tower")]
    [SerializeField] private Button cannonTowerButton;
    [SerializeField] private TowerHologram cannonTowerHologram;
    [Header("Mage Tower")]
    [SerializeField] private Button mageTowerButton;
    [SerializeField] private TowerHologram mageTowerHologram;
    void Start()
    {
        cannonTowerButton.onClick.AddListener(OnCannonTowerClicked);
        mageTowerButton.onClick.AddListener(OnMageTowerClicked);
    }
    private void OnCannonTowerClicked()
    {
        LevelManager.Instance.SetSelectedTower(cannonTowerHologram);
    }
    private void OnMageTowerClicked()
    {
        LevelManager.Instance.SetSelectedTower(mageTowerHologram);
    }
}