using TMPro;
using UnityEngine;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI moneyText;
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
}