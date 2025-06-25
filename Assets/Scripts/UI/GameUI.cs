using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button callWaveButton;
    void Start()
    {
        livesText.SetText(LevelManager.Instance.GetLives().ToString());
        moneyText.SetText(LevelManager.Instance.GetMoney().ToString());
        callWaveButton.onClick.AddListener(OnCallWave);
    }
    public void SetLives(int lives)
    {
        livesText.SetText(lives.ToString());
    }
    public void SetMoney(int money)
    {
        moneyText.SetText(money.ToString());
    }
    private void OnCallWave()
    {
        if (LevelManager.Instance.IsSpawning())
        {
            PopupManager.Instance.Pop("Cannot call next wave while the current wave is spawning.");
        }
        else
        {
            LevelManager.Instance.CallWave();
        }
    }
    public void EnableCall()
    {
        callWaveButton.gameObject.SetActive(true);
    }
    public void DisableCall()
    {
        callWaveButton.gameObject.SetActive(false);
    }
}