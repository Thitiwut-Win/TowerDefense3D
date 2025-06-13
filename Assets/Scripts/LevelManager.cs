using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private int lives;
    public void DecreaseLives(int live)
    {
        lives -= live;
        if (lives <= 0) GameOver();
    }
    private void GameOver()
    {

    }
}