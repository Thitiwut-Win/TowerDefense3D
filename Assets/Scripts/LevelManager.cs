using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] public List<EnemyWaypoints> pathWaypoints;
    private int waveIndex = 0;
    public bool isLoop;
    private bool isAuto;
    private bool isPaused = false;
    private int lives = 100;
    private int money = 500;
    private int volume = 100;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    private IEnumerator StartWave()
    {
        Wave wave = waves[waveIndex];
        yield return new WaitForSeconds(wave.waveCountdown);
        foreach (Horde horde in wave.hordes)
        {
            yield return new WaitForSeconds(horde.hordeCountdown);
            yield return StartCoroutine(StartHorde(horde));
        }
        waveIndex++;
        if (waveIndex >= waves.Count && isLoop) waveIndex = 0;
        if (isAuto) StartCoroutine(StartWave());
    }
    private IEnumerator StartHorde(Horde horde)
    {
        for (int i = 0; i < horde.count; i++)
        {
            NavMeshHit closestHit;
            Vector3 spawnPosition = pathWaypoints[horde.pathNumber].waypoints[0].position;
            if (NavMesh.SamplePosition(spawnPosition, out closestHit, 3, 1)) spawnPosition = closestHit.position;
            BaseEnemy enemy = PoolManager.Instance.Spawn<BaseEnemy>(horde.enemyPrefab.name, spawnPosition, Quaternion.identity);
            enemy.SetWaypoint(pathWaypoints[horde.pathNumber]);
            enemy.StartRunningWaypoints();
            yield return new WaitForSeconds(horde.spawnInterval);
        }
    }
    public int GetLives()
    {
        return lives;
    }
    public int GetMoney()
    {
        return money;
    }
    public void DecreaseLives(int live)
    {
        lives -= live;
        GameUI.Instance.SetLives(Math.Max(lives, 0));
        if (lives <= 0) GameOver();
    }
    public void IncreaseMoney(int mon)
    {
        money += mon;
        GameUI.Instance.SetMoney(money);
    }
    public void SetVolume(int vol)
    {
        volume = vol;
    }
    public void CallWave()
    {
        StartCoroutine(StartWave());
    }
    public int GetVolume()
    {
        return volume;
    }
    public void SetAuto(bool auto)
    {
        isAuto = auto;
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
    private void GameOver()
    {

    }
}