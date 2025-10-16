using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera menuCamera;
    private bool isPaused = false;
    private int volume = 100;
    void Start()
    {
        mainCamera.gameObject.SetActive(false);
    }
    public void ChangeToMainCamera()
    {
        mainCamera.gameObject.SetActive(true);
        menuCamera.gameObject.SetActive(false);
    }
    public void ChnageToMenuCamera()
    {
        mainCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(true);
    }
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
    [SerializeField]
    private UnityEvent _gameOver;
    public UnityEvent onGameOver
    {
        get { return _gameOver; }
        set { _gameOver = value; }
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
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Exit()
    {
        Application.Quit();
    }
}