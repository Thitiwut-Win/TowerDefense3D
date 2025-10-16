using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveText;
    void Start()
    {
        livesText.SetText(LevelManager.Instance.GetLives().ToString());
        moneyText.SetText(LevelManager.Instance.GetMoney().ToString());
    }
    public void SetLives(int lives)
    {
        livesText.SetText(lives.ToString());
    }
    public void SetMoney(int money)
    {
        moneyText.SetText(money.ToString());
    }
    public void SetWave(int wave)
    {
        waveText.SetText("Wave : " + wave.ToString());
    }
}