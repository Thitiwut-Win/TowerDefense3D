using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isPaused = false;
    private int volume = 100;
    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public bool IsPausing()
    {
        return isPaused;
    }
    public void SetVolume(int vol)
    {
        volume = vol;
    }
    public int GetVolume()
    {
        return volume;
    }
    public void GameOver()
    {
        
    }
    public void Save()
    {
        if (LevelManager.Instance.IsEnemiesLeft())
        {
            PopupManager.Instance.Pop("Cannot save until all enemies die");
            return;
        }
        SaveSystem.Save();
    }
    public void Load()
    {
        SaveSystem.Load();
    }
    public void Exit()
    {
        Application.Quit();
    }
}