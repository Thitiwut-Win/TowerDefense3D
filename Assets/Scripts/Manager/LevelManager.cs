using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private TowerHologram selectedTower;
    [SerializeField] private List<Wave> waves;
    [SerializeField] public List<EnemyWaypoints> pathWaypoints;
    private int waveIndex = 0;
    public bool isLoop;
    private bool isAuto = false;
    private bool isSpawning = false;
    private int lives = 100;
    private int money = 500;
    private List<BaseEnemy> enemies = new List<BaseEnemy>();
    private int numbersOfHorde = 1;
    private int waitForHorde = 0;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    private IEnumerator StartWave()
    {
        isSpawning = true;
        Wave wave = waves[waveIndex];
        GameUI.Instance.SetWave(waveIndex + 1);
        GameUI.Instance.DisableCall();
        yield return new WaitForSeconds(wave.waveCountdown);
        for (int i = 0; i < wave.hordes.Count;)
        {
            numbersOfHorde = 1;
            waitForHorde = 0;
            StartCoroutine(StartHorde(wave.hordes[i++]));
            while (i < wave.hordes.Count && wave.hordes[i].eHordeStatus == EHordeStatus.WITH_LAST_HORDE)
            {
                numbersOfHorde++;
                StartCoroutine(StartHorde(wave.hordes[i++]));
            }
            yield return new WaitUntil(() => numbersOfHorde == waitForHorde);
            print(numbersOfHorde + " " + waitForHorde);
        }
        waveIndex++;
        if (waveIndex >= waves.Count && isLoop) waveIndex = 0;
        if (isAuto) StartCoroutine(StartWave());
        isSpawning = false;
        GameUI.Instance.EnableCall();
    }
    private IEnumerator StartHorde(Horde horde)
    {
        yield return new WaitForSeconds(horde.hordeCountdown);
        for (int i = 0; i < horde.count; i++)
        {
            NavMeshHit closestHit;
            Vector3 spawnPosition = pathWaypoints[horde.pathNumber].waypoints[0].position;
            if (NavMesh.SamplePosition(spawnPosition, out closestHit, 3, 1)) spawnPosition = closestHit.position;
            BaseEnemy enemy = PoolManager.Instance.Spawn<BaseEnemy>(horde.enemyPrefab.name, spawnPosition, Quaternion.identity);
            enemy.SetWaypoint(pathWaypoints[horde.pathNumber]);
            enemy.StartRunningWaypoints();
            enemy.onDead += OnEnemiesDied;
            enemies.Add(enemy);
            yield return new WaitForSeconds(horde.spawnInterval);
        }
        waitForHorde++;
    }
    public void SetSelectedTower(TowerHologram hologram)
    {
        selectedTower = hologram;
    }
    public TowerHologram GetSelectedTower()
    {
        return selectedTower;
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
        if (lives <= 0) GameManager.Instance.onGameOver.Invoke();
    }
    public void IncreaseMoney(int mon)
    {
        money += mon;
        GameUI.Instance.SetMoney(money);
    }
    public void CallWave()
    {
        if (!isSpawning) StartCoroutine(StartWave());
    }
    public void SetAuto()
    {
        isAuto = !isAuto;
    }
    public bool IsSpawning()
    {
        return isSpawning;
    }
    private void OnEnemiesDied(BaseEnemy enemy)
    {
        enemies.Remove(enemy);
        enemy.onDead -= OnEnemiesDied;
    }
    public bool IsEnemiesLeft()
    {
        return enemies.Count > 0;
    }
    #region Save & Load
    public void Save(ref LevelSaveData data)
    {
        data.Lives = lives;
        data.Money = money;
        data.WaveIndex = waveIndex;
    }
    public void Load(LevelSaveData data)
    {
        StopAllCoroutines();
        GameUI.Instance.EnableCall();
        isSpawning = false;

        lives = data.Lives;
        money = data.Money;
        waveIndex = data.WaveIndex;
        GameUI.Instance.SetWave(waveIndex + 1);

        GameUI.Instance.SetLives(lives);
        GameUI.Instance.SetMoney(money);

        foreach (BaseEnemy enemy in enemies)
        {
            if (enemy != null) PoolManager.Instance.Despawn(enemy);
        }
        enemies.Clear();
    }
    #endregion
}

[System.Serializable]
public struct LevelSaveData
{
    public int Lives;
    public int Money;
    public int WaveIndex;
}